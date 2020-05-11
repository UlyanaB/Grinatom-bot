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

        public BotLinq()
        {
            data = new DataContext(connectionString);
            Table<Usr> users = data.GetTable<Usr>();
            users.InsertOnSubmit(new Usr() { NickName = "Vbn", Psw = "159" });
            data.SubmitChanges();
        }
        

    }
}
