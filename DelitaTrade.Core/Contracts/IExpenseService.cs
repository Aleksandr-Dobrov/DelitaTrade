using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using DelitaTrade.Core.ViewModels.ExpenseModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IExpenseService
    {
        Task<ExpenseUpdateModel?> GetExpenseByIdAsync(UserViewModel user, int expenseId);
        Task<IEnumerable<PaymentViewModel>> GetAllExpensesAsync(UserViewModel user, int dayReportId);
        Task<ExpenseViewModel> GetExpenseByDayReportIdAsync(UserViewModel user, int dayReportId);
        Task AddExpenseAsync(UserViewModel user, ExpenseInputModel expense);
        Task UpdateExpenseAsync(UserViewModel user, ExpenseUpdateModel expense);
        Task DeleteExpenseAsync(UserViewModel user, int expenseId, int dayReportId);
    }
}
