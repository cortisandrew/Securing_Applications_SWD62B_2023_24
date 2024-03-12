using System.ComponentModel.DataAnnotations;

namespace Securing_Applications_SWD62B_2023_24.Helpers
{
    public class AgeValidationAttribute : ValidationAttribute
    {
        private int minAge = 0;
        public AgeValidationAttribute(int minAge, string errorMesasge="Invalid Age") :base(errorMesasge)
        {
            this.minAge = minAge;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is int)
            {
                return false;
                //throw new NotImplementedException("We have not implemented this validation");
            }

            if (!(value is DateTime))
            {
                // Not a valid data type
                return false;
            }

            // The only option is that I have a DateTime value
            DateTime? valueAsDateTime = value as DateTime?;

            if (valueAsDateTime == null)
            {
                return false;
            }

            TimeSpan? age = DateTime.Now - valueAsDateTime;

            // e.g. minAge = 15
            // 15 * 365 days = 15 years
            if (age > TimeSpan.FromDays(365 * minAge))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
