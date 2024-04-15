namespace AccountTransactions
{
    public class Transactions
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }
        public decimal Value { get; set; }
        public decimal Balance { get; set; }
        public int ClientId { get; set; }
    }
}
