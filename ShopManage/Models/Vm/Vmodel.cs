using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopManage.Models.Vm
{
    public class Vmodel
    {
        public Vmodel()
        {
            this.ProductList = new List<Detail>();
        }
        public int UserId { get; set; }
        [Required, StringLength(50), Display(Name = "Student Name")]
        public string? UserName { get; set; }
        public int Age { get; set; }
        [Required, Column(TypeName = "date"), Display(Name = "Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string? ImageFile { get; set; }
        public IFormFile? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public List<Detail> ProductList { get; set; }
    }
}
