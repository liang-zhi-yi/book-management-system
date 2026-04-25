using BookManagement.DAL;
using BookManagement.Model;

namespace BookManagement.BLL
{
    public class UserManager
    {
        private UserService userService = new UserService();

        /// <summary>
        /// 用户登录业务逻辑
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功的用户对象</returns>
        public User Login(string userName, string password)
        {
            // 1. 数据验证
            if (string.IsNullOrEmpty(userName))
            {
                throw new System.Exception("用户名不能为空！");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new System.Exception("密码不能为空！");
            }

            // 2. 调用DAL层方法进行登录验证
            User user = userService.UserLogin(userName, password);

            // 3. 检查登录结果
            if (user == null)
            {
                throw new System.Exception("用户名或密码错误！");
            }

            return user;
        }
    }
}
