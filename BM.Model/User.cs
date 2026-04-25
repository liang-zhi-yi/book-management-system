using System;

namespace BookManagement.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserType { get; set; } // 0:管理员, 1:读者
        public string RealName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}