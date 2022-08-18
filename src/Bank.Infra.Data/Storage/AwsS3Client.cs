using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Bank.Core.Exceptions;
using Bank.Core.Interfaces;
using Bank.Core.Models.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bank.Infra.Data.Storage
{
    /// <summary>
    /// Client implementation for Amazon AWS S3 Storage.
    /// Act as an generic adapter from IStorageClient to specific Amazon S3 Storage.
    /// </summary>
    public class AwsS3Client : IStorageClient
    {
        private IAmazonS3 S3Client { get; set; }
        ILogger Logger { get; set; }
        string BucketName { get; set; }

        public AwsS3Client(IConfiguration configuration, ILogger<AwsS3Client> logger, IAmazonS3 s3Client)
        {
            Logger = logger;
            S3Client = s3Client;
            BucketName = configuration["AppS3BucketKey"];

            if (string.IsNullOrEmpty(BucketName))
            {
                logger.LogCritical("Missing configuration for S3 bucket. The AppS3Bucket configuration must be set to a S3 bucket.");
                throw new Exception("Missing configuration for S3 bucket. The AppS3Bucket configuration must be set to a S3 bucket.");
            }

            logger.LogInformation($"Configured to use bucket {BucketName}");
        }

        private async Task<ListObjectsV2Response> ListS3(string bucket = null)
        {
            try
            {
                return await S3Client.ListObjectsV2Async(new ListObjectsV2Request
                {
                    BucketName = bucket ?? BucketName
                });
            }
            catch (AmazonS3Exception ex)
            {
                var stEx = new StorageException();
                stEx.SetErrorCode(string.IsNullOrEmpty(ex.ErrorCode) ? -1 : int.Parse(ex.ErrorCode));
                throw stEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<GetObjectResponse> GetFromS3(string key)
        {
            try
            {
                return await S3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = key
                });
            }
            catch (AmazonS3Exception ex)
            {
                var stEx = new StorageException();
                stEx.SetErrorCode(string.IsNullOrEmpty(ex.ErrorCode) ? -1 : int.Parse(ex.ErrorCode));
                throw stEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task PutIntoS3(string key, Stream stream)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = key,
                    InputStream = stream
                };

                var response = await S3Client.PutObjectAsync(putRequest);

                Logger.LogInformation($"Uploaded object {key} to bucket {BucketName}. Request Id: {response.ResponseMetadata.RequestId}");

            }
            catch (AmazonS3Exception ex)
            {
                var stEx = new StorageException();
                stEx.SetErrorCode(string.IsNullOrEmpty(ex.ErrorCode) ? -1 : int.Parse(ex.ErrorCode));
                throw stEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<DeleteObjectResponse> DeleteFromS3(string key)
        {
            try
            {
                var response = await S3Client.DeleteObjectAsync(BucketName, key);
                Logger.LogInformation($"Deleted object {key} from bucket {BucketName}.");

                return response;
            }
            catch (AmazonS3Exception ex)
            {
                var stEx = new StorageException();
                stEx.SetErrorCode(string.IsNullOrEmpty(ex.ErrorCode) ? -1 : int.Parse(ex.ErrorCode));
                throw stEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IStorageClient WithBucket(string bucket)
        {
            BucketName = bucket;
            return this;
        }

        public List<StorageItem> List(string key = null)
        {
            var data = ListS3(key).GetAwaiter().GetResult();

            var list = new List<StorageItem>();
            if (data == null) return list;

            foreach (var item in data.S3Objects)
            {
                list.Add(
                    new StorageItem
                    {
                        Tag = item.ETag,
                        StorageName = item.BucketName,
                        Key = item.Key,
                        LastModified = item.LastModified,
                        OwnerName = item.Owner?.DisplayName,
                        OwnerId = item.Owner?.Id,
                        Size = item.Size
                    });
            }

            return list;
        }

        public byte[] Get(string key)
        {
            var data = GetFromS3(key);

            using (MemoryStream ms = new MemoryStream())
            {
                data.Result.ResponseStream.CopyTo(ms);
                return ms.ToArray();
            }
        }

        public bool Put(string key, Stream stream)
        {
            var data = PutIntoS3(key, stream);
            return data.IsCompleted;
        }

        public bool Delete(string key)
        {
            var data = DeleteFromS3(key);
            return data.IsCompleted;
        }

        public string GetWorkingBucketName()
        {
            return BucketName;
        }
    }
}
