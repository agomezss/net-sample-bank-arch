using Newtonsoft.Json;
using Bank.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bank.Services.Onboarding.ViewModels
{
    public class PostProspectInformationRequest : OnboardingBaseRequest, IValidatableObject
    {
        public PostProspectInformationRequest() : base()
        {
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [JsonProperty("_dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        //public Country birthplace { get; set; }

        [MaxLength(50)]
        public string DocumentNumber { get; set; }

        public string DocumentType { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string MobileCountryCode { get; set; }

        [JsonIgnore]
        public string MobileLocalAreaCode
        {
            get
            {
                var fullNumber = MobileNumber.Trim().Replace(".", "").Replace("-", "");

                if (string.IsNullOrEmpty(fullNumber)) { return null; }

                if (fullNumber.Contains('(') && fullNumber.Contains(')'))
                {
                    var areaCode = fullNumber.Substring(fullNumber.IndexOf('('), fullNumber.IndexOf(')')).Replace("(", "").Replace(")", "").Trim();

                    return areaCode;
                }
                else
                {
                    //let's guess area code is always 2 digits for now...
                    return fullNumber.Substring(0, 2);
                }
            }
        }

        [JsonIgnore]
        public int MobilePhoneNumber
        {
            get
            {
                var fullNumber = MobileNumber.Trim().Replace(".", "").Replace("-", "");

                if (string.IsNullOrEmpty(fullNumber)) { return -1; }

                if (fullNumber.Contains('(') && fullNumber.Contains(')'))
                {
                    fullNumber = fullNumber.Substring(fullNumber.IndexOf(')') + 1).Trim();

                    return Convert.ToInt32(fullNumber);
                }
                else
                {
                    //let's guess area code is always 2 digits for now...
                    return Convert.ToInt32(fullNumber.Substring(2));
                }
            }
        }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        public string Passcode { get; set; }

        public Country BirthPlace { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool flagIsBrazilian = BirthPlace.Abbreviation == "BRA";

            /*
             Validation rule: "Must be 18 years or older"
             */
            int age = DateTime.Today.Year - DateOfBirth.Value.Year;

            if (DateTime.Today.AddYears(age * -1) < DateOfBirth) { age--; }

            if (age < 18)
            {
                yield return new ValidationResult("You must be +18 years old to open an account", new[] { "dateOfBirth" });
            }

            // TODO: Rewrite and Fix
            /*
            DB-driven validations
            */
            //using (var db = new BankDbContext())
            //{
            //    /*
            //    Validation rule: "When brazilian, CPF is required and must be unique"
            //    */
            //    if (flagIsBrazilian)
            //    {
            //        if (string.IsNullOrEmpty(DocumentNumber))
            //        {
            //            yield return new ValidationResult("CPF is required", new[] { "documentNumber" });

            //        }
            //        else
            //        {
            //            if (BrazilianDocumentsValidator.isValidCpf(DocumentNumber))
            //            {

            //                var doc = db.Documents.FirstOrDefault(d => d.DocumentNumber == DocumentNumber);

            //                if (doc != null)
            //                {
            //                    yield return new ValidationResult("Document already in use", new[] { "documentNumber" });
            //                }
            //            }
            //            else
            //            {
            //                yield return new ValidationResult("Invalid CPF number", new[] { "documentNumber" });
            //            }
            //        }
            //    }

            //    /*
            //    Validation rule: "Email must be unique"
            //    */
            //    var customer = db.Customers.FirstOrDefault(c => c.Email == Email);

            //    if (customer != null)
            //    {
            //        yield return new ValidationResult("Email already in use", new[] { "email" });
            //    }

            //    /*
            //    Validation rule: "Phone must be unique"
            //    */

            //    var conn = db.Database.GetDbConnection();
            //    var cmd = conn.CreateCommand();

            //    //We are using the phone unique id as user name, this is not about the phone number, but the uid
            //    cmd.CommandText = "SELECT count(Id) FROM AspNetUsers WHERE userName = @phoneUid";

            //    cmd.Parameters.Add(new SqlParameter("@phoneUid", PhoneUid));

            //    if (conn.State != System.Data.ConnectionState.Open)
            //    {
            //        conn.Open();
            //    }

            //    var phoneCount = Convert.ToInt32(cmd.ExecuteScalar());

            //    if (phoneCount > 0)
            //    {
            //        yield return new ValidationResult("Mobile phone already in use", new[] { "phone" });
            //    }
            //}
        }
    }
}
