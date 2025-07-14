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
    public class UserRepository : IUserRepository
    {
        private readonly MyAppDBContext _db;
        public UserRepository(MyAppDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var User = await _db.UserMaster.Where(x=>x.UserRole!=4).OrderByDescending(x => x.ID).ToListAsync();
            return User;
        }
        public async Task<User> GetById(int id)
        { 
            var user = await _db.UserMaster.FindAsync(id);
            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }
            return user;
        }
        public async Task Add(User model)
        {
            var existingUser = await _db.UserMaster.FirstOrDefaultAsync(p => p.EmployeeCode == model.EmployeeCode);
            if (existingUser == null)
            {
                await _db.UserMaster.AddAsync(model);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User with the same employee code already exists.");
            }
        }
        public async Task Update(User model)
        {
            var User = await _db.UserMaster.FindAsync(model.ID);
            if (User != null)
            {
                User.UserName = model.UserName;
                User.Password = model.Password;
                User.UserRole = model.UserRole;
                User.Email = model.Email;
                User.EmployeeCode = model.EmployeeCode;
                User.Mobile = model.Mobile;
                User.UserRole = model.UserRole;
                User.IsActive = model.IsActive;
                User.UpdatedDate = model.UpdatedDate;
                User.UpdatedBy = model.UpdatedBy;
                _db.Update(User);
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteById(int id)
        {
            var User = await _db.UserMaster.FindAsync(id);
            if (User != null)
            {
                _db.UserMaster.Remove(User);
                await _db.SaveChangesAsync();
            }
        }
    }
}
