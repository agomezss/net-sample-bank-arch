using System;
using System.Collections.Generic;

namespace Bank.Core
{
    public class Error
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }

    public class ServiceResponse
    {
        public object Data;
        private List<Error> _errors = new List<Error>();
        
        public List<Error> GetErrors()
        {
            return _errors;
        }

        public string GetAllErrors()
        {
            string result = "";

            foreach (var err in _errors)
            {
                result += err.ToString() + Environment.NewLine;
            }

            return result.TrimEnd(Environment.NewLine.ToCharArray());
        }

        public ServiceResponse AddError(Exception ex)
        {
            _errors.Add(new Error { Description = ex?.Message });

            if (ex.InnerException != null)
            {
                _errors.Add(new Error { Description = ex.InnerException.Message });
            }

            return this;
        }

        public ServiceResponse AddError(string description, string code = null)
        {
            _errors.Add(new Error { Code = code, Description = description });

            return this;
        }

        public bool HasErrors()
        {
            return _errors.Count > 0;
        }
    }
}
