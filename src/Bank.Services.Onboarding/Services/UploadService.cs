using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Interfaces;
using Bank.Core.Models;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Services.Onboarding
{
    public class UploadService : IUploadService
    {
        private readonly IOutputLogger _outputLogger;
        private readonly IActivityLogger _activityLogger;

        private readonly BankDbContext _db;

        private readonly string[] allowedFileTypes = new string[] { "passport", "proof-of-address", "selfie", "signature" };
        
        public UploadService(IOutputLogger outputlogger,
                             IActivityLogger activityLogger,
                             BankDbContext db)
        {
            _outputLogger = outputlogger;
            _activityLogger = activityLogger;
            _db = db;
        }

        public async Task<ServiceResponse> UploadFile(UploadFileRequest request)
        {
            ServiceResponse response = new ServiceResponse();

            if (new List<string>(allowedFileTypes).Contains(request.FileType))
            {
                _activityLogger.StartExtendedActivity("uploadController.postFile", "postFile");

                var s3client = new AmazonS3Client(request.AmazonBucketRegion);

                _activityLogger.LogFormat("s3client created pointing to region [{0}]", request.AmazonBucketRegion.DisplayName);

                byte countUploaded = 0;
                byte countErrors = 0;

                _activityLogger.LogFormat("customerId: [{0}], phoneUid: [{1}]", request.CustomerId, request.PhoneUid);

                string bucketName = "user-documents-" + request.FileType;

                string fileKey = string.Format("customer_{0}/{1}/", request.CustomerId.ToString().PadLeft(10, '0'), request.PhoneUid);
                
                string fileName;

                Core.Models.Upload newUpload = new Upload() {
                    CustomerId = request.CustomerId,
                    DocumentId = request.DocumentId,
                    UploadTypeId = request.UploadTypeId,
                    DocumentUrl = fileKey,
                    DocumentAwsReference = JsonConvert.SerializeObject(new { fileKey= fileKey, bucketName = bucketName, bucketRegion = request.AmazonBucketRegion.ToString() })
                };

                foreach (IFormFile file in request.Files)
                {
                    fileName = fileKey + file.FileName;

                    try
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            _db.Uploads.Add(newUpload);

                            _db.SaveChanges();
                            
                            file.CopyTo(newMemoryStream);

                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = fileName,
                                BucketName = bucketName,
                                ServerSideEncryptionMethod = ServerSideEncryptionMethod.AES256
                            };

                            var fileTransferUtility = new TransferUtility(s3client);

                            await fileTransferUtility.UploadAsync(uploadRequest);

                            var doc = _db.Documents.Find(newUpload.DocumentId);

                            //TODO: This depends on which types of documents are automatically approved or not
                            doc.DocumentStatusId = BankTypes.CONST_DOC_STATUS_PENDING_APPROVAL.DocumentStatusId;

                            var newDocDetails = new DocumentDetail()
                            {
                                DocumentId = doc.DocumentId,
                                Detail = "Document uploaded to S3: " + newUpload.DocumentUrl,
                                DocumentStatusId = BankTypes.CONST_DOC_STATUS_PENDING_APPROVAL.DocumentStatusId
                            };

                            _db.SaveChanges();

                            countUploaded++;

                            _activityLogger.LogFormat("File [{0}] uploaded successfully", fileName);
                        }
                    }
                    catch (Exception s3ex)
                    {
                        countErrors++;

                        if (newUpload.UploadId > 0)
                        {
                            //some exception happened after DB insertion, during amazon upload; let's remove the entity:
                            try
                            {
                                _db.Uploads.Remove(newUpload);

                                _db.SaveChanges();
                            }
                            catch { }
                        }

                        _activityLogger.LogFormat("File [{0}] upload ERROR: [{1}]", fileName, s3ex.Message);
                        
                        _activityLogger.LogError(s3ex);

                        _outputLogger.Log($"{nameof(UploadService)} {nameof(UploadFile)} error: {s3ex.Message}");
                    }
                }

                response.Data = string.Format("{0} files uploaded successfully{1}",
                                              countUploaded,
                                              countErrors > 0 ? string.Format(", {0} with errors", countErrors) : ".");

                //response.ResponseLog = _activityLogger.FinishExtendedActivity();

            }
            else
            {
                response.AddError(String.Format("Invalid fileType [{0}]", request.FileType));
            }

            return response;
        }
    }
}
