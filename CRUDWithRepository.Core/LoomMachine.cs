using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class LoomMachine
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Enter After")]
        [MaxLength(200)]
        [DisplayName("After")]
        public string After { get; set; }
        [Required(ErrorMessage = "Please Enter Before")]
        [MaxLength(200)]
        [DisplayName("Before")]
        public string Before { get; set; }
        [Required(ErrorMessage = "Please select a Status")]
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
