using CRUDWithRepository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Interfaces
{
    public interface IDashboardRepository
    { 
        Task<DashboardModel> GetAllData(int? ID,string? UserName); 
    }
}
