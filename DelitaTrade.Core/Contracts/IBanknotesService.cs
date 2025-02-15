using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Contracts
{
    public interface IBanknotesService
    {
        Task AddMoneyAsync(int dayReportId, decimal banknote, int count);
        Task RemoveMoneyAsync(int dayReportId, decimal banknote, int count);
    }
}
