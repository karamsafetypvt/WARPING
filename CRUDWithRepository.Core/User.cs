using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Enter User Name")]
        [MaxLength(50)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [MaxLength(50)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Employee Code")]
        [MaxLength(50)]
        [DisplayName("Employee Code")]
        public string EmployeeCode { get; set; }

        [Required(ErrorMessage = "Please select a User Role")]
        [DisplayName("User Role")]
        public int UserRole { get; set; }

        [Required(ErrorMessage = "Please Enter an Email")]
        [MaxLength(150)]
        [DisplayName("Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile No.")]
        [MaxLength(15)]
        [DisplayName("Mobile No.")]
        [Phone]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Please select a Status")]

        [DisplayName("Status")]
        public int? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }

        
    }
}
