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
    public class WarpingPlanDetails
    {
        public int Id { get; set; } 
        public int? WarpingPlanId { get; set; } 
        public string MachineNo { get; set; }
        public int? PreviousProductId { get; set; } 
        public string PlannedProductId { get; set; }
        public string Description { get; set; }
        public int PlannedQty { get; set; } 
        public int? PlannedQty_InMtr { get; set; } 
        public int? WarpingMachineId { get; set; } 
        public int? Status { get; set; } 
        public string? Remark { get; set; }
        public int? AchievedQty { get; set; }
        public string? AchievedRemark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        
    }
}
