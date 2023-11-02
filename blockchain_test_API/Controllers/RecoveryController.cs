using blockchain_prototype.Entities;
using blockchain_test_API.Modules;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace blockchain_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/recovery-wallet")]
    public class RecoveryController : Controller
    {
        private WalletHelper _walletHelper;

        public RecoveryController(WalletHelper walletHelper)
        {
            _walletHelper = walletHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostRecovery([FromBody] SeedPhrase seedPhrase)
        {
            Wallet wallet = await _walletHelper.RecoveryWalletAsync<SeedPhrase>(seedPhrase);
            string walletJson = JsonConvert.SerializeObject(wallet);

            return Content(walletJson, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetRecovery(string seedPhrase)
        {
            Wallet wallet = await _walletHelper.RecoveryWalletAsync<string>(seedPhrase);
            string walletJson = JsonConvert.SerializeObject(wallet);

            return Content(walletJson, "application/json");
        }
    }
}