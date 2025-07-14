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
    public class WarpingMachineRepository : IWarpingMachineRepository
    {
        private readonly MyAppDBContext _db;
        public WarpingMachineRepository(MyAppDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<WarpingMachine>> GetAll()
        {
            var Machine = await _db.WarpingMachine.OrderByDescending(x => x.ID).ToListAsync();
            return Machine;
        }
        public async Task<WarpingMachine> GetById(int id)
        {
            var warpingMachine = await _db.WarpingMachine.FindAsync(id);
            if (warpingMachine == null)
            {
                throw new Exception($"Warping Machine with ID {id} not found.");
            }
            return warpingMachine;
        }
        public async Task Add(WarpingMachine model)
        {
            await _db.WarpingMachine.AddAsync(model);
            await _db.SaveChangesAsync();
        }
        public async Task Update(WarpingMachine model)
        {
            var Machine = await _db.WarpingMachine.FindAsync(model.ID);
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
            var Machine = await _db.WarpingMachine.FindAsync(id);
            if (Machine != null)
            {
                _db.WarpingMachine.Remove(Machine);
                await _db.SaveChangesAsync();
            }
        }
    }
}
