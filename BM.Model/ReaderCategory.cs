namespace BookManagement.Model
{
    public class ReaderCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MaxBorrowCount { get; set; }  // 该类别读者最大借书量
        public int BorrowDays { get; set; }      // 最大借阅天数
        public decimal LateFeePerDay { get; set; } // 逾期每日罚款
    }
}
