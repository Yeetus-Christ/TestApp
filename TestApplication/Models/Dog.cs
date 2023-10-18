using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{
    public class Dog
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public float TailLength { get; set; }

        [Required]
        public float Weight { get; set; }
    }
}
