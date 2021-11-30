using System.Globalization;
using System.Windows.Controls;

namespace Haidu_Claudiu_Lab
{
    public class StringNotEmpty : ValidationRule
    {
        public override ValidationResult Validate(object value,
        CultureInfo cultureinfo)
        {
            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult(false, "String cannot be empty") : new ValidationResult(true, null);
        }
    }
  
    public class StringMinLengthValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureinfo)
        {
            return value.ToString().Length < 3 ? new ValidationResult(false, "String must have at least 3 characters!") : new ValidationResult(true, null);
        }
    }
}