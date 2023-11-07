using blockchain_test_API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace blockchain_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/get-balance")]
    public class WalletBalanceController : Controller
    {
        private readonly WalletHelper _walletHelper;

        public WalletBalanceController(WalletHelper walletHelper)
        {
            _walletHelper = walletHelper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBalance(string address)
        {
            decimal balance = _walletHelper.GetBalance(address);
            await Task.Delay(3000);

            return Json(balance);
        }
    }
}