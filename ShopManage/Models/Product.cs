using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopManage.Models
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        //[Required, Display(Name = "Subject Name")]
        public string? ProductName { get; set; }
        public virtual IList<Detail>? Details { get; set; }

    }
}
