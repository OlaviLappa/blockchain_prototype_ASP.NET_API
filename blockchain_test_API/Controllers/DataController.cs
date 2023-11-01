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

    [Route("blockchain/api/create-transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly SystemInitialization _systemInitialization;

        public TransactionController(SystemInitialization systemInitialization)
        {
            _systemInitialization = systemInitialization;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction()
        {
            ///// ДАННЫЙ КОД ПРЕДСТАВЛЕН В РАМКАХ ТЕСТИРОВАНИЯ!

            //Я допустим создал новый кошелёк и сгенерировалась мне такая прикольная фраза
            /*WalletHelper walletHelper = new WalletHelper();
            Wallet wallet = walletHelper.CreateNewWallet().Item1;

            Transaction transaction = new Transaction(wallet);
            ITransaction additional = new Hex();

            Signature<ITransaction> sign = new Signature<ITransaction>(additional);
            byte[] signature = sign.SignTransaction(transaction, wallet.PrivateKeyHex);
            transaction.Signature = signature;*/

            await _systemInitialization.Launch();

            await Task.Delay(2000);

            return Ok("Транзакция успешно добавлена в пул.");
        }
    }
}