using blockchain_prototype.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace blockchain_test_API.Controllers
{
    [ApiController]
    [Route("blockchain/api/recovery-wallet")]
    public class RecoveryController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> GetRecovery([FromBody] SeedPhrase seedPhrase)
        {
            string message = "";

            List<string> seedPhraseToList = seedPhrase.seedPhrase.Split(' ').ToList();
            Wallet wallet = new Wallet(seedPhraseToList);

            blockchain_test_API.Data.Data data = new blockchain_test_API.Data.Data();
            string[] allPublicKeys = data.GetAllPublicKeys();
            
            for (int i = 0; i < allPublicKeys.Length; i++)
            {
                if (allPublicKeys[i] == wallet.PublicKeyHex)
                {
                    message = "Кошелёк восстановлен!";
                    break;
                }

                else
                {
                    message = "Необходимый ключ не найден";
                }
            }

            await Task.Delay(2000);

            if(message == "Кошелёк восстановлен!")
            {
                string walletJson = JsonConvert.SerializeObject(wallet);
                return Content(walletJson, "application/json");
            }

            else
            {
                return null;
            }
        }
    }

    public class SeedPhrase
    {
        public string seedPhrase { get; set; }
    }
}