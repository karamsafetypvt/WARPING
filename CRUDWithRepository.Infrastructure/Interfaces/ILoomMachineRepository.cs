using CRUDWithRepository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Interfaces
{
    public interface ILoomMachineRepository
    {
        Task<IEnumerable<LoomMachine>> GetAll();
        Task<LoomMachine> GetById(int id);
        Task Add(LoomMachine model);
        Task Update(LoomMachine model);
        Task DeleteById(int id);
    }
}
