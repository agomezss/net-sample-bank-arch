
using Microsoft.AspNetCore.Hosting;
using Bank.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Core.Models
{
    public static class BankTypes
    {
        private static bool flagInitialized = false;

        public static List<Country> Countries { get; set; }
        public static List<DocumentType> DocumentTypes { get; set; }
        public static List<DocumentStatus> DocumentStatus { get; set; }
        public static List<CustomerStatus> CustomerStatus { get; set; }
        public static List<OnboardingStep> OnboardingSteps { get; set; }
        public static List<UploadType> UploadTypes { get; set; }

        #region Country Constants

        private static Country _CONST_COUNTRY_BRAZIL;

        public static Country CONST_COUNTRY_BRAZIL
        {
            get
            {
                if (_CONST_COUNTRY_BRAZIL == null)
                {
                    _CONST_COUNTRY_BRAZIL = Countries.FirstOrDefault(c => c.Abbreviation == "BRA");
                }

                return _CONST_COUNTRY_BRAZIL;
            }
        }

        #endregion

        #region Document Type Constants

        private static DocumentType _CONST_DOC_TYPE_CPF;

        public static DocumentType CONST_DOC_TYPE_CPF
        {
            get
            {
                if (_CONST_DOC_TYPE_CPF == null)
                {
                    _CONST_DOC_TYPE_CPF = DocumentTypes.FirstOrDefault(c => c.Description == "CPF");
                }

                return _CONST_DOC_TYPE_CPF;
            }
        }

        private static DocumentType _CONST_DOC_TYPE_PASSPORT;

        public static DocumentType CONST_DOC_TYPE_PASSPORT
        {
            get
            {
                if (_CONST_DOC_TYPE_PASSPORT == null)
                {
                    _CONST_DOC_TYPE_PASSPORT = DocumentTypes.FirstOrDefault(c => c.Description.ToUpper() == "PASSPORT");  //TODO: This should consider country too, it's wrong
                }

                return _CONST_DOC_TYPE_PASSPORT;
            }
        }
        
        private static DocumentType _CONST_DOC_TYPE_PROOF_OF_ADDRESS;

        public static DocumentType CONST_DOC_TYPE_PROOF_OF_ADDRESS
        {
            get
            {
                if (_CONST_DOC_TYPE_PROOF_OF_ADDRESS == null)
                {
                    _CONST_DOC_TYPE_PROOF_OF_ADDRESS = DocumentTypes.FirstOrDefault(c => c.Description.ToUpper() == "PROOF OF ADDRESS");  //TODO: This should consider country too, it's wrong
                }

                return _CONST_DOC_TYPE_PROOF_OF_ADDRESS;
            }
        }
        
        private static DocumentType _CONST_DOC_TYPE_SELFIE;

        public static DocumentType CONST_DOC_TYPE_SELFIE
        {
            get
            {
                if (_CONST_DOC_TYPE_SELFIE == null)
                {
                    _CONST_DOC_TYPE_SELFIE = DocumentTypes.FirstOrDefault(c => c.Description.ToUpper() == "SELFIE");  //TODO: This should consider country too, it's wrong
                }

                return _CONST_DOC_TYPE_SELFIE;
            }
        }

        #endregion

        #region Customer Status Constants

        private static CustomerStatus _CONST_STATUS_INCOMPLETE;

        public static CustomerStatus CONST_STATUS_INCOMPLETE
        {
            get
            {
                if (_CONST_STATUS_INCOMPLETE == null)
                {
                    _CONST_STATUS_INCOMPLETE = CustomerStatus.FirstOrDefault(c => c.Description == "INCOMPLETE");
                }

                return _CONST_STATUS_INCOMPLETE;
            }
        }

        private static CustomerStatus _CONST_STATUS_COMPLETE;

        public static CustomerStatus CONST_STATUS_COMPLETE
        {

            get
            {

                if (_CONST_STATUS_COMPLETE == null)
                {
                    _CONST_STATUS_COMPLETE = CustomerStatus.FirstOrDefault(c => c.Description == "COMPLETE");
                }

                return _CONST_STATUS_COMPLETE;
            }
        }

        private static CustomerStatus _CONST_STATUS_APPROVED;

        public static CustomerStatus CONST_STATUS_APPROVED
        {

            get
            {

                if (_CONST_STATUS_APPROVED == null)
                {
                    _CONST_STATUS_APPROVED = CustomerStatus.FirstOrDefault(c => c.Description == "APPROVED");
                }

                return _CONST_STATUS_APPROVED;
            }
        }

        private static CustomerStatus _CONST_STATUS_REJECTED;

        public static CustomerStatus CONST_STATUS_REJECTED
        {

            get
            {

                if (_CONST_STATUS_REJECTED == null)
                {
                    _CONST_STATUS_REJECTED = CustomerStatus.FirstOrDefault(c => c.Description == "REJECTED");
                }

                return _CONST_STATUS_REJECTED;
            }
        }

        private static CustomerStatus _CONST_STATUS_ACCOUNT_CANCELED;

        public static CustomerStatus CONST_STATUS_ACCOUNT_CANCELED
        {

            get
            {

                if (_CONST_STATUS_ACCOUNT_CANCELED == null)
                {
                    _CONST_STATUS_ACCOUNT_CANCELED = CustomerStatus.FirstOrDefault(c => c.Description == "ACCOUNT CANCELED");
                }

                return _CONST_STATUS_ACCOUNT_CANCELED;
            }
        }

        private static CustomerStatus _CONST_STATUS_ACCOUNT_ON_HOLD;

        public static CustomerStatus CONST_STATUS_ACCOUNT_ON_HOLD
        {

            get
            {

                if (_CONST_STATUS_ACCOUNT_ON_HOLD == null)
                {
                    _CONST_STATUS_ACCOUNT_ON_HOLD = CustomerStatus.FirstOrDefault(c => c.Description == "ACCOUNT ON HOLD");
                }

                return _CONST_STATUS_ACCOUNT_ON_HOLD;
            }
        }

        private static CustomerStatus _CONST_STATUS_SUSPENDED;

        public static CustomerStatus CONST_STATUS_SUSPENDED
        {

            get
            {

                if (_CONST_STATUS_SUSPENDED == null)
                {
                    _CONST_STATUS_SUSPENDED = CustomerStatus.FirstOrDefault(c => c.Description == "SUSPENDED");
                }

                return _CONST_STATUS_SUSPENDED;
            }
        }

        #endregion

        #region Document Status Constants

        private static DocumentStatus _CONST_DOC_STATUS_RECEIVED;

        public static DocumentStatus CONST_DOC_STATUS_RECEIVED
        {
            get
            {
                if (_CONST_DOC_STATUS_RECEIVED == null)
                {
                    _CONST_DOC_STATUS_RECEIVED = DocumentStatus.FirstOrDefault(c => c.Description == "RECEIVED");
                }

                return _CONST_DOC_STATUS_RECEIVED;
            }
        }

        private static DocumentStatus _CONST_DOC_STATUS_PENDING_APPROVAL;

        public static DocumentStatus CONST_DOC_STATUS_PENDING_APPROVAL
        {
            get
            {
                if (_CONST_DOC_STATUS_PENDING_APPROVAL == null)
                {
                    _CONST_DOC_STATUS_PENDING_APPROVAL = DocumentStatus.FirstOrDefault(c => c.Description == "PENDING APPROVAL");
                }

                return _CONST_DOC_STATUS_PENDING_APPROVAL;
            }
        }

        private static DocumentStatus _CONST_DOC_STATUS_APPROVED;

        public static DocumentStatus CONST_DOC_STATUS_APPROVED
        {
            get
            {
                if (_CONST_DOC_STATUS_APPROVED == null)
                {
                    _CONST_DOC_STATUS_APPROVED = DocumentStatus.FirstOrDefault(c => c.Description == "APPROVED");
                }

                return _CONST_DOC_STATUS_APPROVED;
            }
        }

        private static DocumentStatus _CONST_DOC_STATUS_REJECTED;

        public static DocumentStatus CONST_DOC_STATUS_REJECTED
        {
            get
            {
                if (_CONST_DOC_STATUS_REJECTED == null)
                {
                    _CONST_DOC_STATUS_REJECTED = DocumentStatus.FirstOrDefault(c => c.Description == "REJECTED");
                }

                return _CONST_DOC_STATUS_REJECTED;
            }
        }


        #endregion

        #region Onboarding Step Constants

        private static OnboardingStep _CONST_STEP_PERSONAL_INFORMATION_REQUESTED;

        public static OnboardingStep CONST_STEP_PERSONAL_INFORMATION_REQUESTED
        {

            get
            {

                if (_CONST_STEP_PERSONAL_INFORMATION_REQUESTED == null)
                {
                    _CONST_STEP_PERSONAL_INFORMATION_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "PERSONAL INFORMATION REQUESTED");
                }

                return _CONST_STEP_PERSONAL_INFORMATION_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_PERSONAL_INFORMATION_CONFIRMED;

        public static OnboardingStep CONST_STEP_PERSONAL_INFORMATION_CONFIRMED
        {

            get
            {

                if (_CONST_STEP_PERSONAL_INFORMATION_CONFIRMED == null)
                {
                    _CONST_STEP_PERSONAL_INFORMATION_CONFIRMED = OnboardingSteps.FirstOrDefault(c => c.Description == "PERSONAL INFORMATION CONFIRMED");
                }

                return _CONST_STEP_PERSONAL_INFORMATION_CONFIRMED;
            }
        }

        private static OnboardingStep _CONST_STEP_MOBILE_PHONE_CONFIRMATION_REQUESTED;

        public static OnboardingStep CONST_STEP_MOBILE_PHONE_CONFIRMATION_REQUESTED
        {

            get
            {

                if (_CONST_STEP_MOBILE_PHONE_CONFIRMATION_REQUESTED == null)
                {
                    _CONST_STEP_MOBILE_PHONE_CONFIRMATION_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "MOBILE PHONE CONFIRMATION REQUESTED");
                }

                return _CONST_STEP_MOBILE_PHONE_CONFIRMATION_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_MOBILE_PHONE_CONFIRMED;

        public static OnboardingStep CONST_STEP_MOBILE_PHONE_CONFIRMED
        {

            get
            {

                if (_CONST_STEP_MOBILE_PHONE_CONFIRMED == null)
                {
                    _CONST_STEP_MOBILE_PHONE_CONFIRMED = OnboardingSteps.FirstOrDefault(c => c.Description == "MOBILE PHONE CONFIRMED");
                }

                return _CONST_STEP_MOBILE_PHONE_CONFIRMED;
            }
        }

        private static OnboardingStep _CONST_STEP_IN_WAITING_LIST;

        public static OnboardingStep CONST_STEP_IN_WAITING_LIST
        {

            get
            {

                if (_CONST_STEP_IN_WAITING_LIST == null)
                {
                    _CONST_STEP_IN_WAITING_LIST = OnboardingSteps.FirstOrDefault(c => c.Description == "IN WAITING LIST");
                }

                return _CONST_STEP_IN_WAITING_LIST;
            }
        }

        private static OnboardingStep _CONST_STEP_ADDRESS_REQUESTED;

        public static OnboardingStep CONST_STEP_ADDRESS_REQUESTED
        {

            get
            {

                if (_CONST_STEP_ADDRESS_REQUESTED == null)
                {
                    _CONST_STEP_ADDRESS_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "ADDRESS REQUESTED");
                }

                return _CONST_STEP_ADDRESS_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_ADDRESS_PROVIDED;

        public static OnboardingStep CONST_STEP_ADDRESS_PROVIDED
        {

            get
            {

                if (_CONST_STEP_ADDRESS_PROVIDED == null)
                {
                    _CONST_STEP_ADDRESS_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "ADDRESS PROVIDED");
                }

                return _CONST_STEP_ADDRESS_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_DOCUMENT_ID_UPLOAD_REQUESTED;

        public static OnboardingStep CONST_STEP_DOCUMENT_ID_UPLOAD_REQUESTED
        {

            get
            {

                if (_CONST_STEP_DOCUMENT_ID_UPLOAD_REQUESTED == null)
                {
                    _CONST_STEP_DOCUMENT_ID_UPLOAD_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "DOCUMENT ID UPLOAD REQUESTED");
                }

                return _CONST_STEP_DOCUMENT_ID_UPLOAD_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_DOCUMENT_ID_PROVIDED;

        public static OnboardingStep CONST_STEP_DOCUMENT_ID_PROVIDED
        {

            get
            {

                if (_CONST_STEP_DOCUMENT_ID_PROVIDED == null)
                {
                    _CONST_STEP_DOCUMENT_ID_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "DOCUMENT ID PROVIDED");
                }

                return _CONST_STEP_DOCUMENT_ID_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED;

        public static OnboardingStep CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED
        {

            get
            {

                if (_CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED == null)
                {
                    _CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "PROOF OF ADDRESS UPLOAD REQUESTED");
                }

                return _CONST_STEP_PROOF_OF_ADDRESS_UPLOAD_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_PROOF_OF_ADDRESS_PROVIDED;

        public static OnboardingStep CONST_STEP_PROOF_OF_ADDRESS_PROVIDED
        {

            get
            {

                if (_CONST_STEP_PROOF_OF_ADDRESS_PROVIDED == null)
                {
                    _CONST_STEP_PROOF_OF_ADDRESS_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "PROOF OF ADDRESS PROVIDED");
                }

                return _CONST_STEP_PROOF_OF_ADDRESS_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_CUSTOMER_SELFIE_REQUESTED;

        public static OnboardingStep CONST_STEP_CUSTOMER_SELFIE_REQUESTED
        {

            get
            {

                if (_CONST_STEP_CUSTOMER_SELFIE_REQUESTED == null)
                {
                    _CONST_STEP_CUSTOMER_SELFIE_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "CUSTOMER SELFIE REQUESTED");
                }

                return _CONST_STEP_CUSTOMER_SELFIE_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_CUSTOMER_SELFIE_PROVIDED;

        public static OnboardingStep CONST_STEP_CUSTOMER_SELFIE_PROVIDED
        {

            get
            {

                if (_CONST_STEP_CUSTOMER_SELFIE_PROVIDED == null)
                {
                    _CONST_STEP_CUSTOMER_SELFIE_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "CUSTOMER SELFIE PROVIDED");
                }

                return _CONST_STEP_CUSTOMER_SELFIE_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_UPLOADS_PROVIDED;

        public static OnboardingStep CONST_STEP_UPLOADS_PROVIDED
        {

            get
            {

                if (_CONST_STEP_UPLOADS_PROVIDED == null)
                {
                    _CONST_STEP_UPLOADS_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "UPLOADS PROVIDED");
                }

                return _CONST_STEP_UPLOADS_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_TERMS_AND_CONDITIONS_REQUESTED;

        public static OnboardingStep CONST_STEP_TERMS_AND_CONDITIONS_REQUESTED
        {

            get
            {

                if (_CONST_STEP_TERMS_AND_CONDITIONS_REQUESTED == null)
                {
                    _CONST_STEP_TERMS_AND_CONDITIONS_REQUESTED = OnboardingSteps.FirstOrDefault(c => c.Description == "TERMS & CONDITIONS REQUESTED");
                }

                return _CONST_STEP_TERMS_AND_CONDITIONS_REQUESTED;
            }
        }

        private static OnboardingStep _CONST_STEP_TERMS_AND_CONDTITIONS_PROVIDED;

        public static OnboardingStep CONST_STEP_TERMS_AND_CONDTITIONS_PROVIDED
        {

            get
            {

                if (_CONST_STEP_TERMS_AND_CONDTITIONS_PROVIDED == null)
                {
                    _CONST_STEP_TERMS_AND_CONDTITIONS_PROVIDED = OnboardingSteps.FirstOrDefault(c => c.Description == "TERMS & CONDTITIONS PROVIDED");
                }

                return _CONST_STEP_TERMS_AND_CONDTITIONS_PROVIDED;
            }
        }

        private static OnboardingStep _CONST_STEP_IN_BACKGROUND_CHECKING;

        public static OnboardingStep CONST_STEP_IN_BACKGROUND_CHECKING
        {

            get
            {

                if (_CONST_STEP_IN_BACKGROUND_CHECKING == null)
                {
                    _CONST_STEP_IN_BACKGROUND_CHECKING = OnboardingSteps.FirstOrDefault(c => c.Description == "IN BACKGROUND CHECKING");
                }

                return _CONST_STEP_IN_BACKGROUND_CHECKING;
            }
        }

        private static OnboardingStep _CONST_STEP_BACKGROUND_CHECKING_APPROVED;

        public static OnboardingStep CONST_STEP_BACKGROUND_CHECKING_APPROVED
        {

            get
            {

                if (_CONST_STEP_BACKGROUND_CHECKING_APPROVED == null)
                {
                    _CONST_STEP_BACKGROUND_CHECKING_APPROVED = OnboardingSteps.FirstOrDefault(c => c.Description == "BACKGROUND CHECKING APPROVED");
                }

                return _CONST_STEP_BACKGROUND_CHECKING_APPROVED;
            }
        }

        private static OnboardingStep _CONST_STEP_BACKGROUNG_CHECKING_REJECTED;

        public static OnboardingStep CONST_STEP_BACKGROUNG_CHECKING_REJECTED
        {

            get
            {

                if (_CONST_STEP_BACKGROUNG_CHECKING_REJECTED == null)
                {
                    _CONST_STEP_BACKGROUNG_CHECKING_REJECTED = OnboardingSteps.FirstOrDefault(c => c.Description == "BACKGROUNG CHECKING REJECTED");
                }

                return _CONST_STEP_BACKGROUNG_CHECKING_REJECTED;
            }
        }

        private static OnboardingStep _CONST_STEP_SENDING_CUSTOMER_TO_FOREX_BANK;

        public static OnboardingStep CONST_STEP_SENDING_CUSTOMER_TO_FOREX_BANK
        {

            get
            {

                if (_CONST_STEP_SENDING_CUSTOMER_TO_FOREX_BANK == null)
                {
                    _CONST_STEP_SENDING_CUSTOMER_TO_FOREX_BANK = OnboardingSteps.FirstOrDefault(c => c.Description == "SENDING CUSTOMER TO FOREX BANK");
                }

                return _CONST_STEP_SENDING_CUSTOMER_TO_FOREX_BANK;
            }
        }

        private static OnboardingStep _CONST_STEP_SENDING_CUSTOMER_TO_US_BANK;

        public static OnboardingStep CONST_STEP_SENDING_CUSTOMER_TO_US_BANK
        {

            get
            {

                if (_CONST_STEP_SENDING_CUSTOMER_TO_US_BANK == null)
                {
                    _CONST_STEP_SENDING_CUSTOMER_TO_US_BANK = OnboardingSteps.FirstOrDefault(c => c.Description == "SENDING CUSTOMER TO US BANK");
                }

                return _CONST_STEP_SENDING_CUSTOMER_TO_US_BANK;
            }
        }

        private static OnboardingStep _CONST_STEP_ONBOARDING_FINISHED;

        public static OnboardingStep CONST_STEP_ONBOARDING_FINISHED
        {

            get
            {

                if (_CONST_STEP_ONBOARDING_FINISHED == null)
                {
                    _CONST_STEP_ONBOARDING_FINISHED = OnboardingSteps.FirstOrDefault(c => c.Description == "ONBOARDING FINISHED");
                }

                return _CONST_STEP_ONBOARDING_FINISHED;
            }
        }

        #endregion

        #region Upload Type Constants

        private static UploadType _CONST_UPLOAD_TYPE_IMAGE;

        public static UploadType CONST_UPLOAD_TYPE_IMAGE
        {
            get
            {
                if (_CONST_UPLOAD_TYPE_IMAGE == null)
                {
                    _CONST_UPLOAD_TYPE_IMAGE = UploadTypes.FirstOrDefault(c => c.Description == "IMAGE");
                }

                return _CONST_UPLOAD_TYPE_IMAGE;
            }
        }

        private static UploadType _CONST_UPLOAD_TYPE_VIDEO;

        public static UploadType CONST_UPLOAD_TYPE_VIDEO
        {
            get
            {
                if (_CONST_UPLOAD_TYPE_VIDEO == null)
                {
                    _CONST_UPLOAD_TYPE_VIDEO = UploadTypes.FirstOrDefault(c => c.Description == "VIDEO");
                }

                return _CONST_UPLOAD_TYPE_VIDEO;
            }
        }

        private static UploadType _CONST_UPLOAD_TYPE_SIGNATURE;

        public static UploadType CONST_UPLOAD_TYPE_SIGNATURE
        {
            get
            {
                if (_CONST_UPLOAD_TYPE_SIGNATURE == null)
                {
                    _CONST_UPLOAD_TYPE_SIGNATURE = UploadTypes.FirstOrDefault(c => c.Description == "SIGNATURE");
                }

                return _CONST_UPLOAD_TYPE_SIGNATURE;
            }
        }

        #endregion

        public static void Initialize(IHostingEnvironment environment)
        {
            try
            {
                if (flagInitialized == false)
                {
                    var fileCollection = new BankTypesCollection();

                    fileCollection.Load(environment);

                    Countries = fileCollection.Countries;
                    DocumentTypes = fileCollection.DocumentTypes;
                    CustomerStatus = fileCollection.CustomerStatus;
                    OnboardingSteps = fileCollection.OnboardingSteps;
                    DocumentStatus = fileCollection.DocumentStatus;
                    UploadTypes = fileCollection.UploadTypes;

                    flagInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error reading BankTypes collection: {0}{1}{2}",
                  ex.Message,
                  Environment.NewLine,
                  ex.StackTrace
                ));
            }
        }

        public static BankTypesCollection GetTypes()
        {
            return new BankTypesCollection()
            {
                Countries = BankTypes.Countries,
                CustomerStatus = BankTypes.CustomerStatus,
                DocumentTypes = BankTypes.DocumentTypes,
                DocumentStatus = BankTypes.DocumentStatus,
                OnboardingSteps = BankTypes.OnboardingSteps,
                UploadTypes = BankTypes.UploadTypes
            };
        }
    }
}
