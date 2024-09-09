using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs
{
    public class BasicDto
    {
        [Required]
        public virtual int Id { get; set; }
    }
}
