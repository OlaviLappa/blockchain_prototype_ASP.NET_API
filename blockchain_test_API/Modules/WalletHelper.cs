using blockchain_prototype.Entities;
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
    }
}
