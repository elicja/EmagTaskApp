using EmagTask.Implementation.App;
using EmagTask.Implementation.App.Helper;
using EmagTask.Interface.App;
using EmagTask.Model.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EmagTask
{
    public class Emag
    {
        [FunctionName("Emag")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"The function was run at {DateTime.Now}");

            AppSettings appSetting = AppHelper.GetAppSettings("AppSettings.json");

            IOrderInvoicesHandler orderInvoicesHandler = new OrderInvoicesHandler(appSetting);

            await orderInvoicesHandler.HandleOrdersInvoices();
        }

    }
}
