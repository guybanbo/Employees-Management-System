using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Models
{
    public class Employee
    {
        [RegularExpression(@"^\d{9}$", ErrorMessage = "The ID must contain 9 digits.")]
        [Required]
        public string ID { get; set; }  // ID as a string
        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Salary must be non-negative.")]
        [Required]
        public float Salary { get; set; }
        [Required]
        public string ProfileImage { get; set; } // Image path as a string
    }
}
