using blockchain_prototype.Entities;
using blockchain_test_API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace blockchain_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/create-new-wallet")]
    public class WalletCreatorController : Controller
    {
        private WalletHelper _walletHelper;

        public WalletCreatorController(WalletHelper walletHelper)
        {
            _walletHelper = walletHelper;
        }

        [HttpPost]
        public async Task<IActionResult> GetSeedNumbers()
        {
            Wallet wallet = _walletHelper.CreateNewWallet().Item1;
            List<string> seedPhrase = _walletHelper.CreateNewWallet().Item2;

            await Task.Delay(2000);

            return Json(wallet);
        }
    }
}