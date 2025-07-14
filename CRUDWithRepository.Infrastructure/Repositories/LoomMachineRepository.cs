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
    public class LoomMachineRepository : ILoomMachineRepository
    {
        private readonly MyAppDBContext _db;
        public LoomMachineRepository(MyAppDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LoomMachine>> GetAll()
        {
            var Machine = await _db.LoomMachine.OrderByDescending(x=>x.ID).ToListAsync();
            return Machine;
        }
        public async Task<LoomMachine> GetById(int id)
        {
            var loomMachine = await _db.LoomMachine.FindAsync(id);
            if (loomMachine == null)
            {
                throw new Exception($"Loom Machine with ID {id} not found.");
            }
            return loomMachine;
        }
        public async Task Add(LoomMachine model)
        {
            await _db.LoomMachine.AddAsync(model);
            await _db.SaveChangesAsync();
        }
        public async Task Update(LoomMachine model)
        {
            var Machine = await _db.LoomMachine.FindAsync(model.ID);
            if (Machine != null)
            {
                Machine.After = model.After;
                Machine.Before = model.Before;
                Machine.Status = model.Status;
                _db.Update(Machine);
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteById(int id)
        {
            var Machine = await _db.LoomMachine.FindAsync(id);
            if (Machine != null)
            {
                _db.LoomMachine.Remove(Machine);
                await _db.SaveChangesAsync();
            }
        }
    }
}
