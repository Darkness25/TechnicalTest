namespace AccountTransactions
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public decimal InitialBalance { get; set; }
        public string State { get; set; }
        public int ClientId { get; set; }
    }
}
