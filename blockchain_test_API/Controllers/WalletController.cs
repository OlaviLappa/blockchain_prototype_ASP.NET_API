using blockchain_prototype;
using blockchain_prototype.Entities;
using blockchain_test_API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace blockchain_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/create-new-wallet")]
    public class WalletController : Controller
    {
        private readonly SystemInitialization _systemInitialization;

        public WalletController(SystemInitialization systemInitialization)
        {
            _systemInitialization = systemInitialization;
        }

        [HttpPost]
        public async Task<IActionResult> GetSeedNumbers()
        {
            WalletHelper walletHelper = new WalletHelper();

            Wallet wallet = walletHelper.CreateNewWallet().Item1;
            List<string> seedPhrase = walletHelper.CreateNewWallet().Item2;

            await Task.Delay(2000);

            return Json(wallet);
        }
    }
}