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
        #region DB description

        [Table(Name = "Ask")]
        internal class Ask
        {
            [Column(DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }
            
            [Column(Name = "ord_numb", DbType = "Int NOT NULL", CanBeNull = false)]
            public int OrdNumb { get; set; }
            
            [Column(Name = "ask_txt", DbType = "Varchar(1000)", CanBeNull = true)]
            public string AskTxt { get; set; }
            
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
            public char TrueInd { get; set; }

            [Column(Name = "ans_txt", DbType = "varchar(500) NOT NULL", CanBeNull = false)]
            public string AnsTxt { get; set; }

        }
        
        [Table(Name = "BotUsers")]
        internal class BotUsers
        {
            [Column(Name = "Id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }

            [Column(Name = "TlgUserId", DbType = "Int NOT NULL", CanBeNull = false)]
            public int TlgUserId { get; set; }

            [Column(Name = "TlgUserName", DbType = "Varchar(100) NOT NULL", CanBeNull = false)]
            public string TlgUserName { get; set; }

            [Column(Name = "FirstEnter", DbType = "DateTime NOT NULL", CanBeNull = false)]
            public DateTime FirstEnter { get; set; }

            [Column(Name = "LastEnter", DbType = "DateTime NOT NULL", CanBeNull = false)]
            public DateTime LastEnter { get; set; }

            [Column(Name = "BestResult", DbType = "Int NOT NULL", CanBeNull = false)]
            public int BestResult { get; set; }

        }

        [Table(Name = "UsersLog")]
        internal class UsersLog
        {
            [Column(Name = "Id", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
            public int Id { get; set; }

            [Column(Name = "dt", DbType = "DateTime", IsDbGenerated = true)]
            public DateTime Dt { get; set; }

            [Column(Name = "BotUsersId", DbType = "Int NOT NULL", CanBeNull = false)]
            public int BotUserId { get; set; }

            [Column(Name = "TrueAns", DbType = "char(1) NOT NULL", CanBeNull = false)]
            public char TrueAns { get; set; }

            [Column(Name = "AskQuantity", DbType = "Int NOT NULL", CanBeNull = false)]
            public int AskQuantity { get; set; }

            [Column(Name = "TrueAnsQuantity", DbType = "Int NOT NULL", CanBeNull = false)]
            public int TrueAnsQuantity { get; set; }

            [Column(Name = "AskTxt", DbType = "varchar(1000) NOT NULL", CanBeNull = false)]
            public string AskTxt { get; set; }

            [Column(Name = "AnsTxt", DbType = "varchar(500) NOT NULL", CanBeNull = false)]
            public string AnsTxt { get; set; }

        }

        #endregion DB description

        private const string connectionString = @"Data Source = LocalHost; Initial Catalog = TelegramBot; Integrated Security = True";

        private DataContext data = null;

        internal BotLinq()
        {
            data = new DataContext(connectionString);
        }

        /// <summary>
        /// вернуть вопрос по номеру
        /// </summary>
        /// <param name="ordNumb">номер вопроса</param>
        /// <returns>вопрос по номеру</returns>
        internal Ask GetAskByOrd(int ordNumb)
        {
            Ask ask = data.GetTable<Ask>().FirstOrDefault(x => x.OrdNumb == ordNumb);
            return ask;
        }

        /// <summary>
        /// вернуть ответы по вопросу
        /// </summary>
        /// <param name="ask">вопрос</param>
        /// <returns>ответы</returns>
        internal IEnumerable<Ans> GetAnsByAsk(Ask ask)
        {
            IEnumerable<Ans> ans = data.GetTable<Ans>().Where(x => x.IdAsk == ask.Id).OrderBy(x => x.Ind);
            return ans;
        }

        /// <summary>
        /// дать список номеров вопросов
        /// </summary>
        /// <returns>список номеров вопросов</returns>
        internal IList<int> GetAskList()
        {
            List<int> lst = data.GetTable<Ask>().Select(x => x.OrdNumb).ToList();
            return lst;
        }

        /// <summary>
        /// добавить или обновить пользователя бота
        /// </summary>
        /// <param name="botUsers"></param>
        /// <returns></returns>
        internal BotUsers AddorUpdateBotUsers(BotUsers botUsers)
        {
            Predicate<BotUsers> predicate = x => x.TlgUserId == botUsers.TlgUserId;
            BotUsers usr = data.GetTable<BotUsers>().FirstOrDefault(x => predicate(x));
            if (usr == null)
            {
                data.GetTable<BotUsers>().InsertOnSubmit(botUsers);
                data.SubmitChanges();
                usr = data.GetTable<BotUsers>().FirstOrDefault(x => predicate(x));
            }
            else
            {
                usr = data.GetTable<BotUsers>().Where(x => predicate(x)).ToList()[0];
                usr.LastEnter = DateTime.Now;
                data.SubmitChanges();
            }
            return usr;
        }

        /// <summary>
        /// добавить строку в протокол
        /// </summary>
        /// <param name="tlgUserId"></param>
        /// <param name="tlgUserName"></param>
        /// <returns></returns>
        internal BotUsers AddorUpdateBotUsers(int tlgUserId, string tlgUserName)
        {
            BotUsers botUsers = new BotUsers() { BestResult = 0, FirstEnter = DateTime.Now, LastEnter = DateTime.Now, TlgUserId = tlgUserId, TlgUserName = tlgUserName };
            //Predicate<BotUsers> predicate = x => x.TlgUserId == botUsers.TlgUserId;
            BotUsers usr = data.GetTable<BotUsers>().FirstOrDefault(x => x.TlgUserId == botUsers.TlgUserId);
            if (usr == null)
            {
                data.GetTable<BotUsers>().InsertOnSubmit(botUsers);
                data.SubmitChanges();
                usr = data.GetTable<BotUsers>().FirstOrDefault(x => x.TlgUserId == botUsers.TlgUserId);
            }
            else
            {
                usr = data.GetTable<BotUsers>().Where(x => x.TlgUserId == botUsers.TlgUserId).ToList()[0];
                usr.LastEnter = DateTime.Now;
                data.SubmitChanges();
            }
            return usr;
        }

        /// <summary>
        /// добавить строку в протокол
        /// </summary>
        /// <param name="usersLog"></param>
        internal void AddToUsersLog(UsersLog usersLog)
        {
            data.GetTable<UsersLog>().InsertOnSubmit(usersLog);
            data.SubmitChanges();
        }

        /// <summary>
        /// добавить строку в протокол
        /// </summary>
        /// <param name="botUserId"></param>
        /// <param name="trueAns"></param>
        /// <param name="askQuantity"></param>
        /// <param name="trueAnsQuantity"></param>
        /// <param name="askTxt"></param>
        /// <param name="ansTxt"></param>
        internal void AddToUsersLog(int botUserId, char trueAns, int askQuantity, int trueAnsQuantity, string askTxt, string ansTxt)
        {
            UsersLog usersLog = new UsersLog() {    BotUserId = botUserId, TrueAns = trueAns, AskQuantity = askQuantity,
                                                    TrueAnsQuantity = trueAnsQuantity, AskTxt = askTxt, AnsTxt = ansTxt    };
            data.GetTable<UsersLog>().InsertOnSubmit(usersLog);
            try
            {
                data.SubmitChanges();
            }
            catch(Exception ex)
            {

            }
        }

    }
}
