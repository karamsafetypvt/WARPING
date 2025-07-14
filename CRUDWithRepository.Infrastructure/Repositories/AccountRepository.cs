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
    public class AccountRepository : IAccountRepository
    {
        private readonly MyAppDBContext _db;
        public AccountRepository(MyAppDBContext db)
        {
            _db = db;
        }
        public async Task<User> Login(string userName, string password)
        { 
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }
             
            var user = await _db.UserMaster
                .Where(u => u.EmployeeCode == userName && u.Password == password && u.IsActive == 1)
                .Select(u => new User
                {
                    ID = u.ID,
                    UserName = u.UserName,
                    Password = u.Password, 
                    UserRole = u.UserRole,
                    Email = u.Email,
                    EmployeeCode = u.EmployeeCode
                })
                .FirstOrDefaultAsync();

            return user;
        }


    }
}
