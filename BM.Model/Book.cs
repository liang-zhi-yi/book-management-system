namespace BookManagement.Model
{
    public class Book
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string ISBN { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int TotalCount { get; set; }
        public int AvailableCount { get; set; }
        public decimal Price { get; set; }
    }
}