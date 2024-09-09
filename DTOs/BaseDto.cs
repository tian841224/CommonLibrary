using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.DTOs
{
    public class BaseDto
    {
        [Required]
        public virtual int Id { get; set; }
    }
}
