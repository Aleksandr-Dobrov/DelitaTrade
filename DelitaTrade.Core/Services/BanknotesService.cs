using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class BanknotesService(IRepository repo) : IBanknotesService
    {
        public async Task AddMoneyAsync(int dayReportId, decimal banknote, int count)
        {
            if (count > 0)
            {
                var dayReport = await repo.GetByIdAsync<DayReport>(dayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

                dayReport.Banknotes[banknote] += count;
                repo.Update(dayReport);
                await repo.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Incorrect Value");
            }
        }

        public async Task RemoveMoneyAsync(int dayReportId, decimal banknote, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Incorrect Value");
            }
            else
            {
                var dayReport = await repo.GetByIdAsync<DayReport>(dayReportId) ?? throw new ArgumentNullException(NotFound(nameof(DayReport)));

                if (count > dayReport.Banknotes[banknote])
                {

                    dayReport.Banknotes[banknote] = 0;
                }
                else
                {
                    dayReport.Banknotes[banknote] -= count;
                }
                repo.Update(dayReport);
                await repo.SaveChangesAsync();
            }            
        }
    }
}
