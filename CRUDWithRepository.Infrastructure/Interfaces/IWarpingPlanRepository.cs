using CRUDWithRepository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Interfaces
{
    public interface IWarpingPlanRepository
    {
        Task<IEnumerable<WarpingPlan>> GetAll(int? UserRole,string? CreatedBy, int? filterId, DateTime? filterDate, int? filterStatus); 
        Task<WarpingPlan> GetById(int id);
        Task Add(WarpingPlan model);
        Task Update(WarpingPlan model);
        Task ClosePlan(WarpingPlan model);
        Task<Product> GetPrevProductByMachineNo(string machineId);
        Task<IEnumerable<WarpingPlanReport>> GetReport(int? UserRole, string? CreatedBy, int? filterId, string? Machine, DateTime? FromDate,DateTime? ToDate, int? filterStatus);
    }
}
