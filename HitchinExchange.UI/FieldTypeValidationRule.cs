using System.Windows.Controls;
namespace HitchinExchange.UI
{
    public class FieldTypeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var res = new ValidationResult(true, null);

            var fieldType = value as FieldType;

            return res;
        }
    }
}