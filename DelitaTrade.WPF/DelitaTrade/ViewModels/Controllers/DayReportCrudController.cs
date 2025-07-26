using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.WpfViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Controllers
{
    public class DayReportCrudController(IServiceProvider serviceProvider) : IDayReportCrudController
    {
        public event Action<DayReportViewModel>? OnCreated;
        public event Action<WpfDayReportIdViewModel>? OnDeleted;

        public async Task<DayReportViewModel> CreateDayReportAsync(WpfDayReportViewModel dayReport)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            var userController = scope.GetService<UserController>();
            var newDayReport = new DayReportViewModel()
            {
                Date = dayReport.Date,
                TransmissionDate = DateTime.Now,
                User = userController.CurrentUser
            };

            var createdDayReport = await service.CreateAsync(newDayReport);
            OnCreated?.Invoke(newDayReport);
            return createdDayReport;              
        }

        public async Task DeleteDayReportByIdAsync(int dayReportId)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            var userController = scope.GetService<UserController>();
            await service.DeleteAsync(userController.CurrentUser, dayReportId);
            OnDeleted?.Invoke(new WpfDayReportIdViewModel() { Id = dayReportId });
        }

        public async Task<IEnumerable<DayReportHeaderViewModel>> ReadAllHeaders()
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            var userController = scope.GetService<UserController>();
            return await service.GetAllDatesAsync(userController.CurrentUser);
        }

        public async Task<DayReportBanknotesViewModel> ReadDayReportBanknotesByIdAsync(int Id)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            var userController = scope.GetService<UserController>();
            return await service.GetBanknotesReadonlyAsync(userController.CurrentUser, Id);
        }

        public async Task<DayReportViewModel> ReadDayReportByIdAsync(int Id)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            var userController = scope.GetService<UserController>();
            return await service.GetByIdAsync(userController.CurrentUser, Id);
        }

        public async Task UpdateDayReportAsync(DayReportViewModel dayReport)
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IDayReportService>();
            await service.UpdateAsync(dayReport);
        }
    }
}
