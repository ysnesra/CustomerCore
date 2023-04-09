using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerCore.ViewModels
{

    public class WithinTwoHundredYearsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
        
            value = (DateTime)value;

            if (DateTime.Now.AddYears(-200).CompareTo(value) <= 0 && DateTime.Now.AddYears(-15).CompareTo(value) >= 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage = "Doğum yılı 200 yıl öncesine kadar girilebilir. !", memberNames: new List<string> { validationContext.MemberName });
            }
        }
    }

}
