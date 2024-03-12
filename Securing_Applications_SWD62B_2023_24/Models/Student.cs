using Securing_Applications_SWD62B_2023_24.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Securing_Applications_SWD62B_2023_24.Models
{
    /// <summary>
    /// Student will apply to join MCAST
    /// </summary>
    public class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        /// <summary>
        /// A student applying to join MCAST, must be at least 15 years old
        /// </summary>
        [Required]
        [AgeValidation(15)]
        public DateTime? DoB { get; set; }
    }
}
