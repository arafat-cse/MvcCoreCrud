using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopManage.Models
{
    public class Detail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailId { get; set; }
        //[Required, Display(Name = "Student Name")]
        public string? ProductName { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual User? User { get; set; }
        public virtual Product? Product { get; set; }

    }
}
