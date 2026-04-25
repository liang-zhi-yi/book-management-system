using BookManagement.DAL.Utility;
using BookManagement.Model;
using System;
using System.Data.SqlClient;

namespace BookManagement.DAL
{
    public class UserService
    {
        /// <summary>
        /// 根据用户名和密码验证用户登录
        /// </summary>
        public User UserLogin(string userName, string password)
        {
            string sql = $"SELECT * FROM Users WHERE UserName='{userName}' AND Password='{password}'";

            SqlDataReader reader = SQLHelper.ExecuteReader(sql);
            if (reader != null && reader.Read())
            {
                User user = new User();
                user.UserId = (int)reader["UserId"];
                user.UserName = reader["UserName"].ToString();
                user.Password = reader["Password"].ToString();
                user.UserType = (int)reader["UserType"];
                user.RealName = reader["RealName"].ToString();
                user.CreateTime = (DateTime)reader["CreateTime"];

                reader.Close();
                return user;
            }
            if (reader != null)
                reader.Close();
            return null;
        }

        /// <summary>
        /// 检查用户名是否存在
        /// </summary>
        public bool CheckUserName(string userName)
        {
            string sql = $"SELECT COUNT(*) FROM Users WHERE UserName='{userName}'";
            object result = SQLHelper.ExecuteScalar(sql);
            if (result != null)
            {
                return (int)result > 0;
            }
            return false;
        }
    }
}