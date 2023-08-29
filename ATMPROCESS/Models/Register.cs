using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ATMPROCESS.Models
{
    public class Register
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumber { get; set; }
        [Required(ErrorMessage = "Need To Enter Name  ")]
        public int Pin { get; set; }
        public string? Name { get; set; }

        [Required(ErrorMessage = "Need To Enter Phone Number")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Phone Number")]
        public long PhoneNumber { get; set; }

        [Required(ErrorMessage = "Amount 2000 only")]

        public decimal InitialAmount { get; set; }



    }
}
