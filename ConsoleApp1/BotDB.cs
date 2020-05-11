using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class BotDB
    {
        internal class Usr
        {
            internal int id;
            internal string nickName;
            internal string psw;
            internal string firstName;
            internal string secondName;
            internal string lastName;
            internal string mail;
            internal bool extendedVerification;
            internal bool useMailToReports;
        }

        private const string connectionString = @"Data Source = LocalHost; Initial Catalog = TelegramBot; Integrated Security = True";
        private SqlConnection connection = null;

        internal Usr usr;

        internal BotDB()
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
            if (connection.State != ConnectionState.Open)
            {
                throw new Exception("Не удалось подключиться к БД");
            }
        }

        internal void SetUser(Usr usr)
        {
            using (SqlCommand insertUser = new SqlCommand())
            {
                insertUser.Connection = connection;
                insertUser.CommandType = CommandType.Text;
                insertUser.CommandText =
                    "insert usr     (id,    nick_name,  psw,    first_name,     second_name, last_name, mail, extended_verification, use_mail_to_reports) " +
                    "       values  (@id,   @nick_name, @psw,   @first_name, @second_name, @last_name, @mail, @extended_verification, @use_mail_to_reports)";
                insertUser.Parameters.AddWithValue("id", usr.id);
                insertUser.Parameters.AddWithValue("nick_name", usr.id);
                insertUser.Parameters.AddWithValue("psw", usr.id);
                insertUser.Parameters.AddWithValue("first_name", usr.id);
                insertUser.Parameters.AddWithValue("second_name", usr.id);
                insertUser.Parameters.AddWithValue("last_name", usr.id);
                insertUser.Parameters.AddWithValue("mail", usr.id);
                insertUser.Parameters.AddWithValue("extended_verification", usr.id);
                insertUser.Parameters.AddWithValue("use_mail_to_reports", usr.id);
                insertUser.ExecuteNonQuery();
            }
        }

        internal Usr GetUserByLogin(string login)
        {
            using (SqlCommand selectUser = new SqlCommand())
            {
                string prmName = "nickName";

                selectUser.Connection = connection;
                selectUser.CommandType = CommandType.Text;
                selectUser.CommandText = 
                    "select id, psw, first_name, second_name, last_name, mail, extended_verification, use_mail_to_reports " +
                    "   from usr " +
                    "   where nick_name = @" + prmName + " ";
                selectUser.Parameters.Add(prmName, SqlDbType.VarChar, 50);
                selectUser.Parameters[prmName].Value = login;

                using (SqlDataReader reader = selectUser.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usr = new Usr();
                        usr.id = reader.GetInt32(0);
                        usr.nickName = login;
                        usr.psw = reader.GetString(1);
                        usr.firstName = reader.GetString(2);
                        usr.secondName = reader.GetString(3);
                        usr.lastName = reader.GetString(4);
                        usr.mail = reader.GetString(5);
                        usr.extendedVerification = string.Equals(reader.GetString(6), "Y", StringComparison.CurrentCultureIgnoreCase);
                        usr.useMailToReports = string.Equals(reader.GetString(7), "Y", StringComparison.CurrentCultureIgnoreCase);
                    }
                }
                return usr;
            }
        }
    }
}
