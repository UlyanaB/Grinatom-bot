using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace TestBot
{
    internal class BotLinq
    {
        [Table(Name = "Ask")]
        internal class Ask
        {
            [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }

            [Column(Name = "ord_numb", DbType = "Int NOT NULL", CanBeNull = false)]
            public int OrdNumb { get; set; }

            [Column(Name = "ask_txt", DbType = "Varchar(1000) NOT NULL", CanBeNull = false)]
            public int AskTxt { get; set; }

        }

        [Table(Name = "Ans")]
        internal class Ans
        {
            [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }

            [Column(Name = "id_ask", DbType = "Int NOT NULL", CanBeNull = false)]
            public int IdAsk { get; set; }

            [Column(Name = "ind", DbType = "char(1) NOT NULL", CanBeNull = false)]
            public char Ind { get; set; }

            [Column(Name = "true_ind", DbType = "char(1) NOT NULL", CanBeNull = false)]
            public int TrueInd { get; set; }

            [Column(Name = "ans_txt", DbType = "varchar(500) NOT NULL", CanBeNull = false)]
            public int AnsTxt { get; set; }

        }

        [Table(Name = "usr")]
        internal class Usr
        {
            [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }

            [Column(Name = "nick_name", DbType = "Varchar(50) NOT NULL", CanBeNull = false)]
            public string NickName { get; set; }

            [Column(DbType = "Varchar(50) NOT NULL", CanBeNull = false)]
            public string Psw { get; set; }
        }

        private const string connectionString = @"Data Source = LocalHost; Initial Catalog = TelegramBot; Integrated Security = True";

        private DataContext data = null;

        internal BotLinq()
        {
            data = new DataContext(connectionString);
            //Table<Usr> users = data.GetTable<Usr>();
            //users.InsertOnSubmit(new Usr() { NickName = "Vbn", Psw = "159" });
            //data.SubmitChanges();
        }

        internal Usr GetUserByLogin(string login)
        {
            Usr usr = data.GetTable<Usr>().FirstOrDefault(x => x.NickName == login);
            return usr;
        }

        internal Ask GetAskByOrd(int ordNumb)
        {
            Ask ask = data.GetTable<Ask>().FirstOrDefault(x => x.OrdNumb == ordNumb);
            return ask;
        }

        internal Dictionary<char,Ans> GetAnsByAsk(Ask ask)
        {
            Dictionary<char, Ans> ans = data.GetTable<Ans>().Where(x => x.IdAsk == ask.Id).ToDictionary(x => x.Ind, x => x);
            return ans;
        }
    }
}
