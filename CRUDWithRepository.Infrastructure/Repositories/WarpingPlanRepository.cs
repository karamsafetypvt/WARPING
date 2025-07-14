using CRUDWithRepository.Core;
using CRUDWithRepository.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CRUDWithRepository.Infrastructure.Repositories
{
    public class WarpingPlanRepository : IWarpingPlanRepository
    {
        private readonly MyAppDBContext _db;
        public WarpingPlanRepository(MyAppDBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<WarpingPlan>> GetAll(int? UserRole, string? CreatedBy, int? filterId, DateTime? filterDate, int? filterStatus)
        {
            var query = _db.WarpingPlanMaster.AsQueryable();

            if (UserRole == 4)
            {
                query = query;
            }
            else if (UserRole == 1)
            {
                query = query.Where(x => x.CreatedByL1 == CreatedBy);
            }

            else if (UserRole == 2)
            {
                query = query.Where(x => x.CreatedByL2 == CreatedBy || x.Status == 1);
            }

            else if (UserRole == 3)
            {
                query = query.Where(x => x.CreatedByL3 == CreatedBy || x.Status == 2);
            }
            if (filterId.HasValue)
            {
                query = query.Where(x => x.Id == filterId);
            }

            if (filterDate.HasValue)
            {
                query = query.Where(x => x.Date == filterDate);
            }

            if (filterStatus.HasValue)
            {
                query = query.Where(x => x.Status == filterStatus);
            }

            var data = await query.OrderByDescending(x => x.Date).ToListAsync();
            return data;
        }


        public async Task<WarpingPlan> GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ID", nameof(id));
            }
            var warpingPlan = await _db.WarpingPlanMaster
                .Include(wp => wp.WarpingPlanDetails)
                .FirstOrDefaultAsync(wp => wp.Id == id);

            if (warpingPlan == null)
            {
                throw new KeyNotFoundException($"Warping Plan with ID {id} not found.");
            }

            return warpingPlan;
        }


        public async Task Add(WarpingPlan model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var existingPlan = await _db.WarpingPlanMaster
                .FirstOrDefaultAsync(x => x.Date == model.Date);

            if (existingPlan != null)
            {
                throw new InvalidOperationException("A WarpingPlan with the same date already exists.");
            }
            for (int i = 0; i < model.WarpingPlanDetails.Count; i++)
            {
                var detail = model.WarpingPlanDetails[i];
                var runnage= await _db.Products.Where(x => x.ID == Convert.ToInt32(detail.PlannedProductId)).Select(x => x.Runnage).FirstOrDefaultAsync();
                //detail.PlannedQty_InMtr = runnage * detail.PlannedQty;
                detail.PlannedQty_InMtr = ((detail.PlannedQty*1000)/ runnage)/3;
            }

            await _db.WarpingPlanMaster.AddAsync(model);
            await _db.SaveChangesAsync();
        }



        public async Task Update(WarpingPlan model)
        {
            var data = await _db.WarpingPlanMaster
                .Include(wp => wp.WarpingPlanDetails)
                .FirstOrDefaultAsync(wp => wp.Id == model.Id);

            if (data != null)
            {
                if (data.WarpingPlanDetails != null)
                {
                    _db.WarpingPlanDetails.RemoveRange(data.WarpingPlanDetails);
                }

                //data.Date = model.Date;
                data.Status = model.Status;
                if (data.Status == 1)
                {
                    data.CreatedDateL1 = model.CreatedDateL1;
                    data.CreatedByL1 = model.CreatedByL1;
                }
                if (data.Status == 2)
                {
                    data.CreatedDateL2 = model.CreatedDateL2;
                    data.CreatedByL2 = model.CreatedByL2;
                }
                if (data.Status == 3)
                {
                    data.CreatedDateL3 = model.CreatedDateL3;
                    data.CreatedByL3 = model.CreatedByL3;
                }
                if (model.WarpingPlanDetails != null && model.WarpingPlanDetails.Any())
                {
                    data.WarpingPlanDetails = model.WarpingPlanDetails;
                }

                _db.Update(data);
                await _db.SaveChangesAsync();
            }
        }
        public async Task ClosePlan(WarpingPlan model)
        {
            var data = await _db.WarpingPlanMaster.FindAsync(model.Id);
            if (data != null)
            {
                data.Status = model.Status;
                _db.Update(data);
                await _db.SaveChangesAsync();
            }
        }
        public async Task<Product> GetPrevProductByMachineNo(string machineId)
        {
            var product = await (from wp in _db.WarpingPlanMaster
                                 join det in _db.WarpingPlanDetails on wp.Id equals det.WarpingPlanId
                                 join pm in _db.Products on Convert.ToInt32(det.PlannedProductId) equals pm.ID
                                 where det.MachineNo == machineId && wp.Status == 5
                                 orderby wp.Date descending
                                 select new Product
                                 {
                                     ID = pm.ID,
                                     ProductName = pm.ProductName
                                 }).FirstOrDefaultAsync();

            return product;
        }
        public async Task<IEnumerable<WarpingPlanReport>> GetReport(int? UserRole, string? CreatedBy, int? filterId, string? Machine, DateTime? FromDate, DateTime? ToDate, int? filterStatus)
        {
           
            var warpingPlanIds = _db.WarpingPlanDetails
                         .Where(det => string.IsNullOrEmpty(Machine) || det.MachineNo == Machine)
                         .Select(det => det.WarpingPlanId)
                         .Distinct();

            var query = from wp in _db.WarpingPlanMaster
                        join det in _db.WarpingPlanDetails on wp.Id equals det.WarpingPlanId
                        join lm in _db.LoomMachine on Convert.ToInt32(det.MachineNo) equals lm.ID into loomJoin
                        from lm in loomJoin.DefaultIfEmpty()
                        join Preitm in _db.Products on Convert.ToInt32(det.PreviousProductId) equals Preitm.ID into preProductJoin
                        from preProduct in preProductJoin.DefaultIfEmpty()
                        join planneditm in _db.Products on Convert.ToInt32(det.PlannedProductId) equals planneditm.ID into plannedProductJoin
                        from planneditm in plannedProductJoin.DefaultIfEmpty()
                        join wm in _db.WarpingMachine on Convert.ToInt32(det.WarpingMachineId) equals wm.ID into warpingMachineJoin
                        from wm in warpingMachineJoin.DefaultIfEmpty()
                        where (!filterId.HasValue || wp.Id == filterId) // Filter by ID 
                            && (!FromDate.HasValue || wp.Date >= FromDate) // Filter by Date Range
                            && (!ToDate.HasValue || wp.Date <= ToDate)
                            && (!filterStatus.HasValue || wp.Status == filterStatus) // Filter by Status
                            && (!warpingPlanIds.Any() || warpingPlanIds.Contains(wp.Id)) // Apply the machine filter indirectly
                        orderby wp.Date descending
                        select new
                        {
                            wp.Id,
                            wp.Date,
                            wp.Status,
                            MachineDetails = new
                            {
                                MachineNo = lm.After + (lm.Before != null ? " (" + lm.Before + ")" : ""),
                                WarpingMachine = wm.After + (wm.Before != null ? " (" + wm.Before + ")" : "")
                            },
                            ProductDetails = new
                            {
                                PreviousProduct = preProduct != null ? preProduct.ProductName : "NA",
                                PlannedProduct = planneditm != null ? planneditm.ProductName : "NA"
                            },
                            det.Description,
                            det.PlannedQty,
                            det.PlannedQty_InMtr,
                            det.AchievedQty,
                            det.AchievedRemark,
                            det.Remark
                        };

            var result = await query.ToListAsync();

            // Group the results by Warping Plan Master (Date and Status) to construct WarpingPlanReport
            var groupedResult = result
                .GroupBy(r => new { r.Id, r.Date, r.Status })
                .Select(g => new WarpingPlanReport
                {
                    ID = g.Key.Id,
                    Date = g.Key.Date,
                    Status = g.Key.Status,
                    Detail = g.Select(det => new DetailsReport
                    {
                        MachineNo = det.MachineDetails.MachineNo,
                        PreviousProduct = det.ProductDetails.PreviousProduct,
                        PlannedProduct = det.ProductDetails.PlannedProduct,
                        Description = det.Description,
                        PlannedQty = det.PlannedQty,
                        PlannedQty_InMtr = det.PlannedQty_InMtr,
                        WarpingMachine = det.MachineDetails.WarpingMachine,
                        AchievedQty = det.AchievedQty,
                        AchievedRemark = det.AchievedRemark,
                        Remark = det.Remark
                    }).ToList()
                })
                .ToList();

            return groupedResult;
        }


    }
}
