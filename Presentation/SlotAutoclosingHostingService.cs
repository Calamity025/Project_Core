using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Presentation
{
    internal class SlotAutoclosingHostingService : IHostedService
    {
        private Timer _timer;

        public SlotAutoclosingHostingService(IServiceProvider services)
        {
            Services = services;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Routine, null, 0, 120000);

            return Task.CompletedTask;
        }

        private void Routine(Object state)
        {
            Routine();
        }

        private async Task Routine()
        {
            using (var scope = Services.CreateScope())
            {
                var slotManagement = scope.ServiceProvider.GetService<ISlotManagementService>();
                var slotRepresentation = scope.ServiceProvider.GetService<ISlotRepresentationService>();
                var profileService = scope.ServiceProvider.GetService<IProfileManagementService>();

                var expiredSlots = await slotRepresentation.GetExpiredSlots();
                await slotManagement.CloseSlots(expiredSlots);
                await profileService.AddToWonSlotsList(expiredSlots);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();

            return Task.CompletedTask;
        }
    }
}
