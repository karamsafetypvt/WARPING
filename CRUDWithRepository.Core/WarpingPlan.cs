using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Core
{
    public class WarpingPlan
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please select a Date")]
        public DateTime? Date { get; set; } 
        public DateTime? CreatedDateL1 { get; set; } 
        public string? CreatedByL1 { get; set; } 
        public DateTime? CreatedDateL2 { get; set; } 
        public string? CreatedByL2 { get; set; } 
        public DateTime? CreatedDateL3 { get; set; } 
        public string? CreatedByL3 { get; set; } 

        public DateTime? UpdatedDate { get; set; } 
        public string? UpdatedBy { get; set; } 
        public int? Status { get; set; }
        [NotMapped]
        public int? Role { get; set; }
        //public ICollection<WarpingPlanDetails> WarpingPlanDetails { get; set; }
        public List<WarpingPlanDetails> WarpingPlanDetails { get; set; }
    }
    public class WarpingPlanReport
    { 
        public int? ID { get; set; } 
        public DateTime? Date { get; set; } 
        public int? Status { get; set; } 
        public List<DetailsReport> Detail { get; set; }
    }
    public class DetailsReport
    { 
        public string MachineNo { get; set; }
        public string PreviousProduct { get; set; }
        public string PlannedProduct { get; set; }
        public string Description { get; set; }
        public int PlannedQty { get; set; }
        public int? PlannedQty_InMtr { get; set; }
        public string WarpingMachine { get; set; } 
        public string? Remark { get; set; }
        public int? AchievedQty { get; set; }
        public string? AchievedRemark { get; set; } 
    }
}
