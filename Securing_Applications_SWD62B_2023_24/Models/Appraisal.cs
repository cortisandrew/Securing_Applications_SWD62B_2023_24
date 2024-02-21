using System.ComponentModel.DataAnnotations;

namespace Securing_Applications_SWD62B_2023_24.Models
{
    public class Appraisal
    {
        public int Id { get; set; }

        // [MinLength(1)]
        public string Comment { get; set; } = "No new information.";

    }
}
