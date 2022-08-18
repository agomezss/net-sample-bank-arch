using Amazon;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text;

namespace Bank.Services.Onboarding.ViewModels
{
    public class UploadFileRequest
    {
        public int CustomerId { get; set; }

        public string PhoneUid { get; set; }

        public string FileType { get; set; }

        public int DocumentId { get; set; }
        
        public int UploadTypeId { get; set; }

        public RegionEndpoint AmazonBucketRegion { get; set; }

        public IList<IFormFile> Files { get; set; }
    }
}
