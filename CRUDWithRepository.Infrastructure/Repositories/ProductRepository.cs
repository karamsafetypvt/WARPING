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
    public class ProductRepository : IProductRepository
    {
        private readonly MyAppDBContext _db;
        public ProductRepository(MyAppDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            var product = await _db.Products.OrderByDescending(x => x.ID).ToListAsync();
            return product;
        }
        
        public async Task<IEnumerable<ERPProductDTO>> GetERP_Items()
        {
            try
            {
                string sqlQuery = @"
        SELECT * 
        FROM OPENQUERY(PROD, 'SELECT 
            msi.segment1 AS ProductName,
            msi.description, 
               msi.Inventory_Item_Status_Code  AS Status
        FROM BOM_OPERATIONAL_ROUTINGS BOR,
             BOM_OPERATION_SEQUENCES_V BOS,
             MTL_SYSTEM_ITEMS MSI
        WHERE BOS.routing_sequence_id = BOR.routing_sequence_id
          AND BOR.assembly_item_id = MSI.inventory_item_id
          AND BOS.DISABLE_DATE IS NULL
          AND BOR.organization_id = ''86''
          AND BOR.organization_id = MSI.organization_id
          AND BOS.standard_operation_code = ''T002''
        ORDER BY msi.segment1')";
                 
                var result = await _db.Set<ERPProductDTO>()
                                      .FromSqlRaw(sqlQuery)
                                      .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return new List<ERPProductDTO>();
            }
        }


        public async Task<Product> GetById(int id)
        { 
            var product = await _db.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception($"Product with ID {id} not found.");
            }
            return product;
        }
        public async Task Add(Product model)
        { 
            var existingProduct = await _db.Products
                                           .FirstOrDefaultAsync(p => p.ProductName == model.ProductName); 
            if (existingProduct == null)
            {
                await _db.Products.AddAsync(model);
                await _db.SaveChangesAsync();
            }
            else
            { 
                throw new Exception("Product with the same name already exists.");
            }
        }

        public async Task Update(Product model)
        {
            var product = await _db.Products.FindAsync(model.ID);
            if (product != null)
            {
                //product.ProductName = model.ProductName;
                //product.Description = model.Description;
                product.Runnage = model.Runnage;
                product.Status = model.Status;
                product.UpdatedDate = model.UpdatedDate;
                product.UpdatedBy = model.UpdatedBy;
                _db.Update(product);
                await _db.SaveChangesAsync();
            }
        }
        public async Task DeleteById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
        }
    }
}
