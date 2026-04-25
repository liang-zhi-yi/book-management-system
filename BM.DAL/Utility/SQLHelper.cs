using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BookManagement.DAL.Utility
{
    public class SQLHelper
    {
        public static string ConnectionString
        {
            get { return connString; }
        }
        // 从配置文件中读取数据库连接字符串
        private static string connString = ConfigurationManager.ConnectionStrings["BookDB"].ConnectionString;

        /// <summary>
        /// 执行增、删、改操作
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public static string GetConnectionString()
        {
            return connString;
        }
        public static int ExecuteNonQuery(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库操作错误：" + ex.Message, "系统提示");
                return -1;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("数据库查询错误：" + ex.Message, "系统提示");
                return null;
            }
        }

        /// <summary>
        /// 执行查询，返回单个值（第一行第一列）
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>object对象</returns>
        public static object ExecuteScalar(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库查询错误：" + ex.Message, "系统提示");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>DataSet对象</returns>
        public static DataSet ExecuteDataSet(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库查询错误：" + ex.Message, "系统提示");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <returns>连接成功返回true，失败返回false</returns>
        public static bool TestConnection()
        {
            SqlConnection conn = new SqlConnection(connString);
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库连接失败：" + ex.Message, "系统提示");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}