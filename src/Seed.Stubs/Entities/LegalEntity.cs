namespace TSeedStubs.Entities
{
    public class LegalEntity
    {
        public int Id { get; set; }
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public bool DirectDebitEnabled { get; set; }
        public string CustomerType { get; set; }
    }
}