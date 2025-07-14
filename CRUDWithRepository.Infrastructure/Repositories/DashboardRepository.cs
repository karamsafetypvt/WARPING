using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly MyAppDBContext _db;
        public DashboardRepository(MyAppDBContext db)
        {
            _db = db;
        }
        public async Task<DashboardModel> GetAllData(int? UserRole, string? UserName)
        {
            var dashboardData = new DashboardModel();
             
            var query = _db.WarpingPlanMaster.AsQueryable();
             
            if (UserRole == 4)
            { 
                query = query;
            }
            else if (UserRole == 1)
            { 
                query = query.Where(x => x.CreatedByL1 == UserName);
            }
            else if (UserRole == 2)
            { 
                query = query.Where(x => x.CreatedByL2 == UserName || x.Status == 1);
            }
            else if (UserRole == 3)
            { 
                query = query.Where(x => x.CreatedByL3 == UserName || x.Status == 1 || x.Status == 2);
            }
             
            var counts = await query
                .GroupBy(w => w.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

             
            dashboardData.Level1 = counts.FirstOrDefault(x => x.Status == 1)?.Count ?? 0;
            dashboardData.Level2 = counts.FirstOrDefault(x => x.Status == 2)?.Count ?? 0;
            dashboardData.Level3 = counts.FirstOrDefault(x => x.Status == 3)?.Count ?? 0;
            dashboardData.Completed = counts.FirstOrDefault(x => x.Status == 4)?.Count ?? 0;
            dashboardData.Closed = counts.FirstOrDefault(x => x.Status == 5)?.Count ?? 0;
            dashboardData.AllRequest = counts.FirstOrDefault()?.Count ?? 0;
            

            dashboardData.CreatedBy = UserName;

            return dashboardData;
        }

    }
}
