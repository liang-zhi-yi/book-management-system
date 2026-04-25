using System;

namespace BookManagement.Model
{
    public class Reader
    {
        public int ReaderId { get; set; }
        public string ReaderName { get; set; }
        public string Gender { get; set; }  // 性别
        public int ReaderCategoryId { get; set; }  // 读者类别ID
        public string ReaderCategoryName { get; set; }  // 读者类别名称（用于显示）
        public string Phone { get; set; }  // 联系电话
        public string Email { get; set; }  // 邮箱
        public string Address { get; set; }  // 地址
        public DateTime RegistrationDate { get; set; }  // 注册日期
        public DateTime ExpiryDate { get; set; }  // 有效期至
        public int Status { get; set; }  // 状态：0-正常，1-挂失，2-注销
        public string Remark { get; set; }  // 备注
    }
}
