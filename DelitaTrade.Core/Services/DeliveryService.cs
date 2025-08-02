using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.CompanyConstants;
using static DelitaTrade.Common.DelitaDbConstants;
using static DelitaTrade.Common.ExceptionMessages;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Services
{
    public class DeliveryService(IRepository repo, IInvoiceInDayReportService invoiceInDayReportService) : BaseService, IDeliveryService
    {
        public async Task<DeliveryViewModel> AddDeliveryAsync(DeliveryInputModel deliveryInput, UserViewModel user)
        {
            if(IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            } 

            var dayReport = await repo.All<DayReport>()
                .Include(d => d.Vehicle)
                .Include(d => d.IdentityUser)
                .FirstOrDefaultAsync(d => d.Id == deliveryInput.DayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            var newDelivery = new Delivery()
            {
                DayReport = dayReport,
                Vehicle = dayReport.Vehicle,
                Employee = dayReport.IdentityUser,
                DeliveryAddress = await repo.GetByIdAsync<CompanyObject>(deliveryInput.DeliveryAddressId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)))
            };

            await repo.AddAsync(newDelivery);            
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newDelivery);
            await repo.Include(newDelivery.DeliveryAddress, o => o.Address);

            return new DeliveryViewModel
            {
                Id = newDelivery.Id,
                EmployeeName = $"{dayReport.IdentityUser.Name} {dayReport.IdentityUser.LastName}",
                CompanyObjectId = newDelivery.DeliveryAddressId,
                CompanyObjectName = newDelivery.DeliveryAddress.Name,
                Address = $"{newDelivery.DeliveryAddress.Address?.Town} {newDelivery.DeliveryAddress.Address?.StreetName} {newDelivery.DeliveryAddress.Address?.Number}"                 
            };
        }

        public async Task AddInvoiceAsync(UserViewModel user, InvoiceInputModel invoice, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var companyObject = await repo.GetByIdAsync<CompanyObject>(invoice.CompanyObjectId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject))); 
            var company = await repo.GetByIdAsync<Company>(companyObject.CompanyId) ?? throw new ArgumentNullException(NotFound(nameof(Company))); 
            var delivery = await repo.GetByIdAsync<Delivery>(deliveryId) ?? throw new ArgumentNullException(NotFound(nameof(Delivery)));
            var dayReport = await repo.GetByIdAsync<DayReport>(delivery.DayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            var companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name
            };

            var newInvoice = new InvoiceViewModel()
            {
                Company = companyViewModel,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = companyObject.Id,
                    Name = companyObject.Name,
                    Company = companyViewModel
                },
                DayReport = new DayReportViewModel()
                {
                    Id = dayReport.Id,
                    Date = dayReport.Date,
                    User = new UserViewModel() 
                    {
                        Id = dayReport.IdentityUserId,
                        Name = string.Empty
                    }
                },
                Number = invoice.Number,
                Amount = invoice.Amount,
                Weight = invoice.Weight,
                PayMethod = companyObject.IsBankPay ? PayMethod.Bank : PayMethod.Cash
            };

            var addedInvoice = await invoiceInDayReportService.CreateAsync(newInvoice);

            var newInvoiceInDayReport = await repo.GetByIdAsync<InvoiceInDayReport>(addedInvoice.IdInDayReport) ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));

            delivery.Payments.Add(newInvoiceInDayReport);
            await repo.SaveChangesAsync();
        }

        public async Task AddOldInvoiceAsync(UserViewModel user, OldInvoiceInputModel oldInvoice, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var companyObject = await repo.GetByIdAsync<CompanyObject>(oldInvoice.CompanyObjectId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
            var company = await repo.GetByIdAsync<Company>(companyObject.CompanyId) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            var delivery = await repo.GetByIdAsync<Delivery>(deliveryId) ?? throw new ArgumentNullException(NotFound(nameof(Delivery)));
            var dayReport = await repo.GetByIdAsync<DayReport>(delivery.DayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            var companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name
            };

            var newInvoice = new InvoiceViewModel()
            {
                Company = companyViewModel,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = companyObject.Id,
                    Name = companyObject.Name,
                    Company = companyViewModel
                },
                DayReport = new DayReportViewModel()
                {
                    Id = dayReport.Id,
                    Date = dayReport.Date,
                    User = new UserViewModel()
                    {
                        Id = dayReport.IdentityUserId,
                        Name = string.Empty
                    }
                },
                Number = oldInvoice.Number,
                Amount = oldInvoice.Amount,
                PayMethod = oldInvoice.PaymentType,                
            };

            var addedInvoice = await invoiceInDayReportService.CreateAsync(newInvoice);

            var newInvoiceInDayReport = await repo.GetByIdAsync<InvoiceInDayReport>(addedInvoice.IdInDayReport) ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));

            delivery.Payments.Add(newInvoiceInDayReport);
            await repo.SaveChangesAsync();
        }

        public async Task AddCreditNoteAsync(UserViewModel user, CreditNoteInputModel creditNote, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var companyObject = await repo.GetByIdAsync<CompanyObject>(creditNote.CompanyObjectId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
            var company = await repo.GetByIdAsync<Company>(companyObject.CompanyId) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            var delivery = await repo.GetByIdAsync<Delivery>(deliveryId) ?? throw new ArgumentNullException(NotFound(nameof(Delivery)));
            var dayReport = await repo.GetByIdAsync<DayReport>(delivery.DayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            decimal income = 0;

            if (creditNote.CreditNoteType == CreditNoteMethod.NotDeducted) 
            {
                income = creditNote.Amount * -1;
            }

            var companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name
            };

            var newCreditNote = new InvoiceViewModel()
            {
                Company = companyViewModel,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = companyObject.Id,
                    Name = companyObject.Name,
                    Company = companyViewModel
                },
                DayReport = new DayReportViewModel()
                {
                    Id = dayReport.Id,
                    Date = dayReport.Date,
                    User = new UserViewModel()
                    {
                        Id = dayReport.IdentityUserId,
                        Name = string.Empty
                    }
                },
                Number = creditNote.Number,
                Amount = creditNote.Amount,                
                Income = income,
                PayMethod = PayMethod.CreditNote
            };

            var addedCreditNote = await invoiceInDayReportService.CreateAsync(newCreditNote);

            var newInvoiceInDayReport = await repo.GetByIdAsync<InvoiceInDayReport>(addedCreditNote.IdInDayReport) ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));

            newInvoiceInDayReport.IsCompleted = true;

            delivery.Payments.Add(newInvoiceInDayReport);
            await repo.SaveChangesAsync();
        }

        public async Task<int> ImportPaymentsToDayReportAsync(UserViewModel user, int dayReportId, DayReportJsonImportModel dayReportJson)
        {               
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            int count = 0;

            var dayReport = await repo.All<DayReport>()
                    .Include(d => d.Vehicle)
                    .Include(d => d.IdentityUser)
                    .Include(d => d.Deliveries)
                    .ThenInclude(dl => dl.DeliveryAddress)
                    .Include(d => d.Deliveries)
                    .ThenInclude(dl => dl.Payments)
                    .ThenInclude(p => p.Invoice)
                    .ThenInclude(i => i.CompanyObject)
                    .ThenInclude(o => o.Company)
                    .FirstOrDefaultAsync(d => d.Id == dayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            foreach (var invoice in dayReportJson.Invoices)
            {
                if (invoice.Company == null || invoice.CompanyObject == null) 
                {
                    continue;
                }
                if (await repo.All<Invoice>().AnyAsync(i => i.Number == invoice.Number))
                {
                    continue;
                }

                var delivery = dayReport.Deliveries.Where(d => d.DeliveryAddress.Name.ToLower() == invoice.CompanyObject.ToLower()).FirstOrDefault();
                var companyObjects = await repo.All<CompanyObject>()
                            .Where(o => o.Name.ToLower() == invoice.CompanyObject.ToLower())
                            .ToListAsync() 
                        ?? await repo.All<CompanyObject>()
                            .Where(o => invoice.CompanyObject.Contains(o.Name) && o.IsActive)
                            .ToListAsync()
                        ?? await repo.All<CompanyObject>()
                            .Where(o => o.Name.Contains(invoice.CompanyObject) && o.IsActive)
                            .ToListAsync();

                Company? company = await repo.All<Company>()
                        .Include(c => c.Objects)
                        .SingleOrDefaultAsync(c => c.Name.ToLower() == invoice.Company.GetCompanyName()!.ToLower());
                CompanyObject? companyObject = null;

                if (companyObjects.Count > 1 && company != null)
                {
                    if (companyObjects.Where(o => o.CompanyId == company.Id).Count() == 1)
                    {
                        companyObject = companyObjects.First();
                    }
                }
                else if (companyObjects.Count > 1 && company == null)
                {
                    throw new InvalidOperationException("Company not found");
                }
                else if (companyObjects.Count == 1 && company == null)
                {
                    companyObject = companyObjects.First();
                    company = await repo.All<Company>()
                         .FirstOrDefaultAsync(c => c.Id == companyObject.CompanyId);
                }
                else if (companyObjects.Count == 1 && company != null)
                {
                    companyObject = companyObjects.First();
                }
                else if (companyObjects.Count == 0 && company != null)
                {   
                    decimal comparerCoefficient = 0.85m;

                    Dictionary<decimal, CompanyObject> equalsResult = new Dictionary<decimal, CompanyObject>();
                    foreach (var obj in company.Objects)
                    {
                        int equals = 0;
                        List<string> args = [.. obj.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)];
                        foreach (var arg in args)
                        {
                            if (invoice.CompanyObject.ToLower().Contains(arg.ToLower()))
                            {
                                equals++;
                            }
                        }
                        equalsResult[(decimal)equals / args.Count] = obj;
                    }

                    if (equalsResult.OrderByDescending(o => o.Key).First().Key > comparerCoefficient)
                    {
                        companyObject = equalsResult.OrderByDescending(o => o.Key).First().Value;
                    }                    
                }

                if (companyObject != null && companyObject.Name.ToLower() != invoice.CompanyObject.ToLower())
                {
                    companyObject.Name = invoice.CompanyObject;
                }

                if (company == null)
                {
                    company = new Company()
                    {
                        Type = invoice.Company.GetCompanyType(),
                        Name = invoice.Company.GetCompanyName() ?? throw new ArgumentNullException(nameof(invoice.Company)),
                    };
                    await repo.AddAsync(company);
                }

                if (companyObject == null)
                {
                    companyObject = new CompanyObject()
                    {
                        Name = invoice.CompanyObject,
                        Company = company,
                        Trader = await repo.All<Trader>().Where(t => t.Name == DefaultTraderName).FirstAsync()                        
                    };
                    await repo.AddAsync(companyObject);
                }

                if (delivery == null)
                {

                    delivery = new Delivery()
                    {
                        DayReport = dayReport,
                        Employee = dayReport.IdentityUser,
                        DeliveryAddress = companyObject,
                        Vehicle = dayReport.Vehicle
                    };

                    dayReport.Deliveries.Add(delivery);

                }

                if (delivery.Payments.All(p => p.Invoice.Number != invoice.Number))
                {
                    var newInvoice = new Invoice()
                    {
                        Number = invoice.Number ?? invoice.DeliveryNumber![2..],
                        Company = company,
                        CompanyObject = companyObject,
                        Weight = decimal.Parse(invoice.Weight ?? throw new ArgumentNullException(nameof(invoice.Weight))),
                        Amount = decimal.Parse(invoice.Amount ?? throw new ArgumentNullException(nameof(invoice.Amount)))                       
                    };

                    var newPayment = new InvoiceInDayReport()
                    {
                        DayReport = dayReport,
                        Invoice = newInvoice,
                        PayMethod = companyObject.IsBankPay ? PayMethod.Bank : PayMethod.Cash
                    };

                    dayReport.AddInvoiceToTotals(newPayment);

                    delivery.Payments.Add(newPayment);
                    dayReport.Invoices.Add(newPayment);
                
                }

                await repo.SaveChangesAsync();
                count++;
            }
            return count;
        }

        public async Task CompleteAllAsync(UserViewModel user, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, DriverRole) == false) 
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var payments = await repo.AllReadonly<Delivery>()
                .Where(d => d.Id == deliveryId                         
                         && d.EmployeeId == user.Id)
                .Select(d => d.Payments
                    .Where(i => i.IsCompleted == false
                            && (i.PayMethod == PayMethod.Bank
                            || i.PayMethod == PayMethod.Cash
                            || i.PayMethod == PayMethod.OldPayCash))
                    .Select(i => new InvoiceViewModel()
                    {
                        Id = i.Invoice.Id,
                        IdInDayReport = i.Id,
                        Company = new CompanyViewModel() 
                        {
                            Id = i.Invoice.CompanyId,
                            Name = i.Invoice.Company.Name,
                        },
                        CompanyObject = new CompanyObjectViewModel()
                        {
                            Id = i.Invoice.CompanyObjectId,
                            Name = i.Invoice.CompanyObject.Name,
                            Company = new CompanyViewModel()
                            {
                                Id = i.Invoice.CompanyId,
                                Name = i.Invoice.Company.Name,
                            }
                        },
                        DayReport = new DayReportViewModel()
                        {
                            Id = i.DayReportId,
                            Date = i.DayReport.Date,
                            User = new UserViewModel()
                            {
                                Id = i.DayReport.IdentityUserId,
                                Name = string.Empty
                            }
                        },
                        Number = i.Invoice.Number,
                        Amount = i.Invoice.Amount,
                        Weight = i.Invoice.Weight,
                        Income = i.Income,
                        PayMethod = i.PayMethod,
                        IsPaid = i.Invoice.IsPaid                
                    })).FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(Delivery)));
           
            foreach (var item in payments)
            {
                if (item != null) 
                {
                    if (item.PayMethod == PayMethod.Cash
                        || item.PayMethod == PayMethod.OldPayCash)
                    {
                        item.Income = item.Amount;
                    }
                    item.IsCompleted = true;

                    await invoiceInDayReportService.UpdateAsync(item);
                }
            }            
        }

        public async Task<DeliveryViewModel?> GetByIdAsync(UserViewModel user, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }
                        
            var delivery = repo.AllReadonly<Delivery>()
                .Where(d => d.Id == deliveryId);
            if (user.Roles.Contains(DriverRole))
            {
                delivery = delivery.Where(d => d.EmployeeId == user.Id);
            }

            var result = await delivery.Select(d => new DeliveryViewModel()
            {
                Id = deliveryId,
                dayReportId = d.DayReportId,
                EmployeeName = $"{d.Employee.Name} {d.Employee.LastName}",
                CompanyObjectName = d.DeliveryAddress.Name,
                CompanyObjectId = d.DeliveryAddressId,
                IsBank = d.DeliveryAddress.IsBankPay,
                Address = d.DeliveryAddress.Address != null ? $"{d.DeliveryAddress.Address.Town} {d.DeliveryAddress.Address.StreetName} {d.DeliveryAddress.Address.Number}" : null,
                TotalIncome = d.Payments.Sum(i => i.Income),
                Payments = d.Payments
                    .Select(p => new PaymentViewModel()
                    {
                        Id = p.Id,
                        CompanyName = $"{p.Invoice.Company.Name} {p.Invoice.Company.Type}",
                        CompanyObjectName = p.Invoice.CompanyObject.Name,
                        InvoiceNumber = p.Invoice.Number,
                        Amount = p.Invoice.Amount,
                        Balance = p.Invoice.Amount - Math.Abs(p.Invoice.InvoicesInDayReports.Sum(i => i.Income)),
                        Income = p.Income,
                        Weight = p.Invoice.Weight,
                        PayMethod = p.PayMethod,
                        IsBank = p.Invoice.CompanyObject.IsBankPay,
                        IsCompleted = p.IsCompleted
                    })
            }).FirstOrDefaultAsync();

            result?.CalculateTotals();

            return result;
        }

        public async Task<int?> GetVehicleIdFromDeliveryAsync(int deliveryId)
        {
            return await repo.AllReadonly<Delivery>()
                    .Where(d => d.Id == deliveryId)
                    .Select(d => d.VehicleId)
                    .FirstOrDefaultAsync();
        }

        public Task<bool> IsCompleteAsync(UserViewModel user, int deliveryId)
        {
            return repo.AllReadonly<Delivery>()
                    .Where(d => d.Id == deliveryId
                            && d.DayReport.IdentityUserId == user.Id)
                    .Select(d => d.Payments.All(i => i.IsCompleted)).FirstOrDefaultAsync();                    
        }
        
        public async Task<int> GetDayReportIdAsync(int deliveryId)
        {
            return await repo.AllReadonly<Delivery>()
                    .Where(d => d.Id == deliveryId)
                    .Select(d => d.DayReportId)
                    .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(UserViewModel user, int deliveryId)
        {
            if (IsAtLeastInOneRole(user, AdminRole, LogisticsManagerRole) == false) 
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var delivery = await repo.All<Delivery>()
                    .Include(d => d.Payments)
                    .ThenInclude(p => p.Invoice)
                    .Where(d => d.Id == deliveryId
                             && d.Payments.All(d => d.IsCompleted == false))
                    .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Remove is prohibited");

            foreach (var invoiceToDelete in delivery.Payments)
            {
                var company = new CompanyViewModel()
                {
                    Name = string.Empty
                };

                var invoiceToDeleteViewModel = new InvoiceViewModel()
                {                    
                    Number = invoiceToDelete.Invoice.Number,
                    IdInDayReport = invoiceToDelete.Id,
                    DayReport = new DayReportViewModel() 
                    {
                        Id = invoiceToDelete.DayReportId,
                        Date = DateTime.Now,
                        User = user,
                    },
                    Company = company,
                    CompanyObject = new CompanyObjectViewModel()
                    {
                        Company = company,
                        Name = string.Empty
                    }
                };

                await invoiceInDayReportService.DeleteAsync(invoiceToDeleteViewModel);
            }

            repo.Remove(delivery);
            await repo.SaveChangesAsync();
        }
    }
}
