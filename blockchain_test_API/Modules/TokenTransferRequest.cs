namespace blockchain_test_API.Modules
{
    public class TokenTransferRequest
    {
        public string FromAddress { get; set; }
        public string SenderPrivateKey { get; set; }
        public string SenderPublicKey { get; set; }
        public decimal SenderBalance { get; set; }
        public string ToAddress { get; set; }
        public decimal Amount { get; set; }
    }
}