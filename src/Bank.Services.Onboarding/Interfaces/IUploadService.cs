using Microsoft.AspNetCore.Http;
using Bank.Core;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IUploadService
    {
        Task<ServiceResponse> UploadFile(UploadFileRequest request);
    }
}
