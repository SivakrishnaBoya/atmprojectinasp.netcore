using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMPROCESS.Models
{
    public class MiniStatement
    {
        [Key]
        public int MinistatementId { get; set; }
        [ForeignKey("Register")]
        public int AccountNumber { get; set; }
        public DateTime DateTime { get; set; }
        public string? status { get; set; }
    }
}
