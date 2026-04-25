using System;

namespace BookManagement.Model
{
    public class BorrowRecord
    {
        public int BorrowId { get; set; }
        public int ReaderId { get; set; }
        public int BookId { get; set; }
        public string ReaderName { get; set; }
        public string BookName { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int Status { get; set; }
    }
}