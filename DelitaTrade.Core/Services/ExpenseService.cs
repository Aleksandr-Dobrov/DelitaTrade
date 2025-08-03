using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Core.ViewModels.ExpenseModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.Constants.CompanyConstants;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.Constants.FormatConstant.DateTimeFormat;
using static DelitaTrade.Common.DelitaDbConstants;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class ExpenseService(IRepository repo, IInvoiceInDayReportService invoiceInDayReportService) : BaseService, IExpenseService
    {
        public async Task AddExpenseAsync(UserViewModel user, ExpenseInputModel expense)
        {

            if (IsAtLeastInOneRole(user, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            if (expense.ExpenseId == 0 && expense.Expense == null)
            {
                throw new InvalidOperationException("Expense is required");
            }

            var dayReport = await repo.GetByIdAsync<DayReport>(expense.DayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));
            
            CompanyObject companyObject;
            Company? company;

            if (expense.ExpenseId.HasValue)
            {
                companyObject = await repo.GetByIdAsync<CompanyObject>(expense.ExpenseId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
                company = await repo.GetByIdAsync<Company>(companyObject.CompanyId) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            }
            else
            {
                var licensePlate = await repo.AllReadonly<Vehicle>()
                        .Where(v => v.Id == expense.VehicleId)
                        .Select(v => v.LicensePlate)
                        .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(Vehicle)));

                company = await repo.All<Company>()
                        .Where(c => c.Name == licensePlate)
                        .FirstOrDefaultAsync();

                if (company == null)
                {
                    company = new Company()
                    {
                        Name = licensePlate,
                        Type = ExpenseCompanyType
                    };

                    await repo.AddAsync(company);
                    await repo.SaveChangesAsync();
                }

                companyObject = new CompanyObject()
                {
                    Name = expense.Expense ?? throw new InvalidOperationException("Expense is required"),
                    Company = company,
                    Trader = await repo.All<Trader>().Where(t => t.Name == DefaultTraderName).FirstAsync()
                };

                await repo.AddAsync(companyObject);
                await repo.SaveChangesAsync();
            }

            var companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name
            };

            var newExpense = new InvoiceViewModel()
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
                Number = await GetExpenseNumber(),
                Income = expense.ExpenseAmount * -1,
                PayMethod = PayMethod.Expense
            };

            var crestedExpense = await invoiceInDayReportService.CreateAsync(newExpense);
            var newInvoiceInDayReport = await repo.GetByIdAsync<InvoiceInDayReport>(crestedExpense.IdInDayReport) ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));

            newInvoiceInDayReport.IsCompleted = true;
            

            await repo.SaveChangesAsync();
        }

        public async Task UpdateExpenseAsync(UserViewModel user, ExpenseUpdateModel expense)
        {
            if (IsAtLeastInOneRole(user, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var updatedExpense = await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.Invoice)
                .Include(i => i.DayReport)
                .Where(i => i.Id == expense.Id
                && i.DayReportId == expense.DayReportId
                && i.DayReport.IdentityUserId == user.Id)
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));


            if (updatedExpense.PayMethod != PayMethod.Expense)
            {
                throw new InvalidOperationException("You can not delete this expense.");
            }

            CompanyObject companyObject;
            Company? company;

            if (expense.ExpenseId.HasValue)
            {
                companyObject = await repo.GetByIdAsync<CompanyObject>(expense.ExpenseId) ?? throw new ArgumentNullException(NotFound(nameof(CompanyObject)));
                company = await repo.GetByIdAsync<Company>(companyObject.CompanyId) ?? throw new ArgumentNullException(NotFound(nameof(Company)));
            }
            else
            {
                var licensePlate = await repo.AllReadonly<Vehicle>()
                        .Where(v => v.Id == expense.VehicleId)
                        .Select(v => v.LicensePlate)
                        .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(Vehicle)));

                company = await repo.All<Company>()
                        .Where(c => c.Name == licensePlate)
                        .FirstOrDefaultAsync();

                if (company == null)
                {
                    var newCompany = new Company()
                    {
                        Name = licensePlate,
                        Type = ExpenseCompanyType
                    };

                    await repo.AddAsync(newCompany);
                    await repo.SaveChangesAsync();

                    company = await repo.AllReadonly<Company>()
                        .Where(c => c.Name == licensePlate)
                        .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Can not create new expense");
                }

                var newCompanyObject = new CompanyObject()
                {
                    Name = expense.Expense ?? throw new InvalidOperationException("Expense is required"),
                    Company = company,
                    Trader = await repo.All<Trader>().Where(t => t.Name == DefaultTraderName).FirstAsync()
                };

                await repo.AddAsync(newCompanyObject);
                await repo.SaveChangesAsync();

                companyObject = await repo.AllReadonly<CompanyObject>()
                        .Where(o => o.Name == expense.Expense
                                && o.CompanyId == company.Id)
                        .FirstOrDefaultAsync() ?? throw new InvalidOperationException("Can not create new expense");
            }

            var companyViewModel = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name
            };

            var expenseToUpdate = new InvoiceViewModel()
            {
                Id = updatedExpense.InvoiceId,
                IdInDayReport = updatedExpense.Id,
                Company = companyViewModel,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = companyObject.Id,
                    Name = companyObject.Name,
                    Company = companyViewModel
                },
                DayReport = new DayReportViewModel()
                {
                    Id = expense.DayReportId,
                    Date = updatedExpense.DayReport.Date,
                    User = new UserViewModel()
                    {
                        Id = user.Id,
                        Name = string.Empty
                    }
                },
                Number = updatedExpense.Invoice.Number,
                Income = expense.ExpenseAmount * -1,
                PayMethod = PayMethod.Expense,
                IsCompleted = true                
            };

            await invoiceInDayReportService.UpdateAsync(expenseToUpdate);
        }

        public async Task DeleteExpenseAsync(UserViewModel user, int expenseId, int dayReportId)
        {
            if (IsAtLeastInOneRole(user, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var expense = await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.DayReport)
                .Include(i => i.Invoice)
                .ThenInclude(i => i.CompanyObject)
                .ThenInclude(i => i.Company)
                .Where(i => i.Id == expenseId 
                && i.DayReportId == dayReportId
                && i.DayReport.IdentityUserId == user.Id)
                .FirstOrDefaultAsync() ?? throw new ArgumentNullException(NotFound(nameof(InvoiceInDayReport)));


            if (expense.PayMethod != PayMethod.Expense)
            {
                throw new InvalidOperationException("You can not delete this expense.");
            }

            var companyViewModel = new CompanyViewModel()
            {
                Id = expense.Invoice.CompanyObject.CompanyId,
                Name = expense.Invoice.CompanyObject.Company.Name
            };

            var expenseToDelete = new InvoiceViewModel()
            {
                Id = expense.InvoiceId,
                IdInDayReport = expense.Id,
                Company = companyViewModel,
                CompanyObject = new CompanyObjectViewModel()
                {
                    Id = expense.Invoice.CompanyObjectId,
                    Name = expense.Invoice.CompanyObject.Name,
                    Company = companyViewModel
                },
                DayReport = new DayReportViewModel()
                {
                    Id = dayReportId,
                    Date = expense.DayReport.Date,
                    User = new UserViewModel()
                    {
                        Id = user.Id,
                        Name = string.Empty
                    }
                },
                Number = expense.Invoice.Number,
            };

            await invoiceInDayReportService.DeleteAsync(expenseToDelete);
        }

        public async Task<ExpenseUpdateModel?> GetExpenseByIdAsync(UserViewModel user, int expenseId)
        {
            return await repo.AllReadonly<InvoiceInDayReport>()
                .Include(i => i.DayReport)
                .Where(i => i.Id == expenseId
                && i.DayReport.IdentityUserId == user.Id)
                .Select(i => new ExpenseUpdateModel()
                {
                    Id = i.Id,
                    DayReportId = i.DayReportId,
                    ExpenseId = i.Invoice.CompanyObjectId,
                    ExpenseAmount = i.Income * -1,
                    VehicleId = i.DayReport.VehicleId ?? 0,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PaymentViewModel>> GetAllExpensesAsync(UserViewModel user, int dayReportId)
        {
            return await repo.AllReadonly<InvoiceInDayReport>()
                .Where(i => i.DayReportId == dayReportId 
                && i.DayReport.IdentityUserId == user.Id
                && i.PayMethod == PayMethod.Expense)
                .Select(i => new PaymentViewModel()
                {
                    Id = i.Id,
                    InvoiceNumber = i.Invoice.Number,
                    Income = i.Income,
                    CompanyObjectName = i.Invoice.CompanyObject.Name,
                    CompanyName = i.Invoice.Company.Name
                })
                .ToListAsync();
        }

        public async Task<ExpenseViewModel> GetExpenseByDayReportIdAsync(UserViewModel user, int dayReportId)
        {
            if (IsAtLeastInOneRole(user, DriverRole) == false)
            {
                throw new UnauthorizedAccessException(nameof(DelitaUser));
            }

            var dayReport = await repo.GetByIdAsync<DayReport>(dayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

            if (dayReport.IdentityUserId != user.Id)
            {
                throw new UnauthorizedAccessException("You are not allowed to view this day report.");
            }

            var expenseModel = new ExpenseViewModel()
            {
                DayReportId = dayReportId,
                ExpenseDate = dayReport.Date.ToString(AppDateFormat),
                Payments = await GetAllExpensesAsync(user, dayReportId)
            };

            expenseModel.TotalExpense = expenseModel.Payments.Sum(p => p.Income);

            return expenseModel;
        }

        private async Task<string> GetExpenseNumber()
        {
            string number = $"E{DateTime.Now.Date:yy-MM-dd}".Replace("-", "");

            var numbers = await repo.AllReadonly<Invoice>()
                    .Where(i => i.Number.Contains(number))
                    .ToListAsync();
            return $"{number}{numbers.Count:D3}"; //TODO This could generate identical numbers if more than one users try to add expense at the same time. Find another way to generate unique numbers from SQL Server.
        }
    }
}
