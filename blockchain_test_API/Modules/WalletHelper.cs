using blockchain_prototype.Entities;
using blockchain_test_API.Controllers;
using System;

namespace blockchain_test_API.Modules
{
    public class WalletHelper
    {
        private List<string> _seedPhrase;

        public WalletHelper() { }

        public WalletHelper(List<string> seedPhrase)
        {
            _seedPhrase = seedPhrase;
        }

        public (Wallet, List<string>) CreateNewWallet()
        {
            Random random = new Random();

            int[] seedNumbers = random.GenerateSeedNumbersRandom(12);
            List<string> seedPhrase = random.GenerateSeedPhraseRandom(seedNumbers);
            Wallet wallet = new Wallet(seedPhrase);

            SavePublicKey(wallet.PublicKeyHex);

            return (wallet, seedPhrase);
        }

        private void SavePublicKey(string publicKeyHex)
        {
            using (StreamWriter writehex = new StreamWriter("Data/public-keys.txt", true))
            {
                writehex.WriteLine(publicKeyHex);
            }
        }

        public async Task<Wallet> RecoveryWalletAsync<T>(T type)
        {
            string message = "";

            List<string> seedPhraseToList = new List<string>();

            if (type is SeedPhrase)
            {
                SeedPhrase seedPhrase = type as SeedPhrase;
                seedPhraseToList = seedPhrase.seedPhrase.Split(' ').ToList();
            }

            else if (type is string)
            {
                string seedPhrase = type as string;
                seedPhraseToList = seedPhrase.Split(' ').ToList();
            }

            Wallet wallet = new Wallet(seedPhraseToList);

            blockchain_test_API.Data.Data data = new blockchain_test_API.Data.Data();
            string[] allPublicKeys = await data.GetAllPublicKeysAsync();

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

            return wallet;
        }
    }
}