using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
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

        internal void SetUser(Usr user)
        {
            using (SqlCommand insertUser = new SqlCommand())
            {

            }
        }

        internal Usr GetUserByLogin(string login)
        {
            using (SqlCommand selectUser = new SqlCommand())
            {
                string prmName = "nickName";

                selectUser.CommandType = CommandType.Text;
                selectUser.CommandText = 
                    "select id, psw, first_name, second_name, last_name, mail, extended_verification, use_mail_to_reports " +
                    "   from usr " +
                    "   where nick_name = @" + prmName + " ";
                selectUser.Parameters.Add(prmName, SqlDbType.VarChar, 50);
                selectUser.Parameters[prmName].Value = login;

                SqlDataReader reader = selectUser.ExecuteReader();
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
                    usr.extendedVerification = string.Equals(reader.GetString(6),"Y", StringComparison.CurrentCultureIgnoreCase);
                    usr.useMailToReports = string.Equals(reader.GetString(7),"Y", StringComparison.CurrentCultureIgnoreCase);
                }
                return usr;
            }
        }
    }
}
