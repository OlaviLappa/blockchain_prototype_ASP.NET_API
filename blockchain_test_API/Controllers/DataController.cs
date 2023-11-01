using Microsoft.AspNetCore.Mvc;
using blockchain_prototype;

namespace Blockch_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/node-initialization")]
    public class DataController : ControllerBase
    {
        private readonly SystemInitialization systemInitialization;
        private bool initializationInProgress = false;

        public DataController(SystemInitialization systemInitialization)
        {
            this.systemInitialization = systemInitialization;
        }

        [HttpGet]
        public async Task<IActionResult> GetDataFromConsoleAppAsync()
        {
            if (initializationInProgress)
            {
                return BadRequest("Инициализация уже запущена.");
            }

            initializationInProgress = true;

            try
            {
                await StartProccess(systemInitialization);
                return Ok("Запуск обработки транзакций..."); 
            }

            finally
            {
                await Task.Delay(Timeout.InfiniteTimeSpan);
            }
        }

        private async Task StartProccess(SystemInitialization systemInitialization)
        {
            try
            {
                await systemInitialization.Launch();
            }

            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}