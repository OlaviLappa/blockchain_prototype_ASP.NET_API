using blockchain_prototype.Entities;
using blockchain_prototype.Transaction;
using blockchain_test_API.Controllers;

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

            if(message == "Кошелёк восстановлен!")
            {
                return wallet;
            }

            else
            {
                return null;
            }
        }

        public async Task<string> TrySignTransaction(string from, string to, decimal amount, TransactionSending transactionSend)
        {
            string message = string.Empty;
            string failMessage = "Fail!";

            Wallet senderWallet = new Wallet();
            Wallet recepientWallet = new Wallet();
            Random rand = new Random();

            bool fromAddress = CheckAddress(from);
            bool toAddress = CheckAddress(to);

            if (from == to)
            {
                message = "Перевод на один и тот же адрес невозможен";
                await Task.Delay(5000);

                return message;
            }

            if (fromAddress && toAddress)
            {
                senderWallet.Address = from;
                recepientWallet.Address = to;
                senderWallet.PrivateKeyHex = "24bfc73dee31400b36b7be5472ce290a4b49f88571f2effc046a8b8960283352";
                senderWallet.PublicKeyHex = "5030937adc53413b7ed0fba9f081fbc9fc25b3b2b39543a87d09472e9f8eefc70a8f303ea283b2b6d0d80ad8e6ca28ef0afabfdf4a6e4efcf2ffa1cda5f5ac82";

                senderWallet.Balance = new BalanceHandler().GetCurrentBalance(senderWallet);
                recepientWallet.Balance = new BalanceHandler().GetCurrentBalance(recepientWallet);

                Transaction transaction = new Transaction(senderWallet, recepientWallet, amount, TransactionType.Transfer);

                ITransaction additional = new Hex();
                Signature<ITransaction> sign = new Signature<ITransaction>(additional);
                byte[] signature = sign.SignTransaction(transaction, senderWallet.PrivateKeyHex);
                transaction.Signature = signature;

                SendTransaction(transactionSend, transaction);

                await Task.Delay(5000);
                message = "Транзакция подписана и отправлена!";
            }

            else
            {
                throw new Exception(failMessage);
            }

            return message;
        }

        private void SendTransaction(TransactionSending transactionSend, Transaction transaction) 
            => transactionSend?.Invoke(transaction);

        private string GenerateAddressForCheck(string publicKeyHex)
        {
            Wallet wallet = new Wallet();
            byte[] publicKeyBytes = HexToBytes(publicKeyHex);
            string addressForCheck = wallet.GenerateAddress(publicKeyBytes);

            return addressForCheck;
        }

        private byte[] HexToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex строка должна иметь четное количество символов.");
            }

            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private bool CheckAddress(string address)
        {
            bool status = false;

            blockchain_test_API.Data.Data data = new blockchain_test_API.Data.Data();
            string[] allKeys = data.GetAllPublicKeys();

            for (int i = 0; i < allKeys.Length; i++)
            {
                string addressForCheck = GenerateAddressForCheck(allKeys[i]);

                if (addressForCheck == address)
                {
                    status = true;
                    break;
                }
            }

            return status;
        }
    }
}