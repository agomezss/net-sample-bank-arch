
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Bank.Core.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using System.Linq;

namespace Bank.Core.Models
{
    public class BankTypesCollection
    {
        public BankTypesCollection()
        {
        }

        public List<Country> Countries { get; set; }
        public List<DocumentType> DocumentTypes { get; set; }
        public List<DocumentStatus> DocumentStatus { get; set; }
        public List<CustomerStatus> CustomerStatus { get; set; }
        public List<OnboardingStep> OnboardingSteps { get; set; }
        public List<UploadType> UploadTypes { get; set; }

        public void Load(IHostingEnvironment environment = null)
        {
            BankTypesCollection fileCollection = new BankTypesCollection();

            // gets type lists from file cache
            string path = Path.Combine(environment.ContentRootPath, "Resources", "BankTypes.json");

            if (!File.Exists(path))
            {
                //TODO: load from database, save file
                using (var db = new BankDbContext(environment))
                {
                    fileCollection.Countries = db.Countries.ToList<Country>();
                    fileCollection.CustomerStatus = db.CustomerStatus.ToList<CustomerStatus>();
                    fileCollection.DocumentTypes = db.DocumentTypes.ToList<DocumentType>();
                    fileCollection.DocumentStatus = db.DocumentStatus.ToList<DocumentStatus>();
                    fileCollection.OnboardingSteps = db.OnboardingSteps.ToList<OnboardingStep>();
                    fileCollection.UploadTypes = db.UploadTypes.ToList<UploadType>();
                }

                File.WriteAllText(path, JsonConvert.SerializeObject(fileCollection));
            }
            else
            {
                try
                {
                    string fileText = File.ReadAllText(path);

                    fileCollection = JsonConvert.DeserializeObject<BankTypesCollection>(fileText);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("Error reading BankTypes collection [{0}]: {1}{2}{3}",
                      path,
                      ex.Message,
                      Environment.NewLine,
                      ex.StackTrace
                    ));
                }
            }
            
            //we should avoid null values, so...

            if (fileCollection.Countries != null)
            {
                Countries = fileCollection.Countries;
            }

            if (fileCollection.DocumentTypes != null)
            {
                DocumentTypes = fileCollection.DocumentTypes;
            }

            if (fileCollection.CustomerStatus != null)
            {
                CustomerStatus = fileCollection.CustomerStatus;
            }

            if (fileCollection.OnboardingSteps != null)
            {
                OnboardingSteps = fileCollection.OnboardingSteps;
            }
                
            if (fileCollection.DocumentStatus != null)
            {
                DocumentStatus = fileCollection.DocumentStatus;
            }
                
            if (fileCollection.UploadTypes != null)
            {
                UploadTypes = fileCollection.UploadTypes;
            }
        }
    }
}
