using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Core.Validations
{
    public class Validation<T> : AbstractValidator<T> where T : class
    {
        public Validation()
        {
        }
    }
}
