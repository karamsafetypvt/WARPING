using CRUDWithRepository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Interfaces
{
    public interface IWarpingMachineRepository
    {
        Task<IEnumerable<WarpingMachine>> GetAll();
        Task<WarpingMachine> GetById(int id);
        Task Add(WarpingMachine model);
        Task Update(WarpingMachine model);
        Task DeleteById(int id);
    }
}
