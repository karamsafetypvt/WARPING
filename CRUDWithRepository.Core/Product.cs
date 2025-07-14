using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class Product
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Enter Product Name")]
        [MaxLength(200)]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        [MaxLength(500)]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Runnage")]
        public int? Runnage { get; set; }
        [Required(ErrorMessage = "Please select a Status")]
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
    public class ERPProductDTO
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
