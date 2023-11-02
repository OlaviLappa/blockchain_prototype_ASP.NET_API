using blockchain_prototype;
using blockchain_prototype.Entities;
using blockchain_test_API.Modules;
using Microsoft.AspNetCore.Mvc;

namespace blockchain_test_API.Controllers
{
    public delegate void TransactionSending(Transaction transaction);

    [ApiController]
    [Route("blockchain/api/token-sending")]
    public class TokenSendingController : Controller
    {
        private readonly SystemInitialization _systemInitialization;
        private readonly WalletHelper _walletHelper;
        private TransactionSending _transactionSending;

        public TokenSendingController(SystemInitialization systemInitialization, WalletHelper walletHelper)
        {
            _systemInitialization = systemInitialization;
            _walletHelper = walletHelper;
        }

        [HttpPost]
        public async Task<IActionResult> SendToken(Wallet from, string to, decimal amount)
        {
            _transactionSending = new TransactionSending(_systemInitialization.AddTransactionToPool);
            string msg = await _walletHelper.TrySignTransaction(from, to, amount, _transactionSending);

            return Json(msg);
        }
    }
}