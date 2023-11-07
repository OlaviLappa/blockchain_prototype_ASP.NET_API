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

        public decimal GetBalance(string address)
        {
            Wallet wallet = new Wallet();
            wallet.Address = address;

            BalanceHandler balanceHandler = new BalanceHandler();

            return balanceHandler.GetCurrentBalance(wallet);
        }

        public async Task<Wallet> RecoveryWalletAsync<T>(T type)
        {
            WalletStatus status = WalletStatus.Awaiting;

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
                    status = WalletStatus.Restored;
                    break;
                }

                else
                {
                    status = WalletStatus.DoesNotExist;
                }
            }

            if (status == WalletStatus.Restored)
            {
                return wallet;
            }

            else
            {
                return null;
            }
        }

        public async Task<string> TrySignTransaction(Wallet from, string to, decimal amount, TransactionSending transactionSend)
        {
            string message = string.Empty;
            string failMessage = "Fail!";

            Wallet senderWallet = from;
            Wallet recepientWallet = new Wallet();
            Random rand = new Random();

            bool fromAddress = CheckAddress(from.Address);
            bool toAddress = CheckAddress(to);

            if (from.Address == to)
            {
                message = "Перевод на один и тот же адрес невозможен";
                await Task.Delay(5000);

                return message;
            }

            if (fromAddress && toAddress)
            {
                recepientWallet.Address = to;

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