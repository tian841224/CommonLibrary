using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs
{
    public class BasicDto
    {
        [Required]
        public int Id { get; set; }
    }
}
