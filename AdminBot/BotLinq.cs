using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace AdminBot
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
        
        #endregion DB description

        private const string connectionString = @"Data Source = LocalHost; Initial Catalog = TelegramBot; Integrated Security = True";

        private DataContext data = null;

        internal BotLinq()
        {
            data = new DataContext(connectionString);
        }

        internal IEnumerable<KeyValuePair<int, string>> GetAsks(int first, int last)
        {
            IEnumerable<KeyValuePair<int,string>> shortAskList 
                = data.GetTable<Ask>().Where(x => first <= x.OrdNumb && x.OrdNumb <= last).OrderBy(x => x.OrdNumb)
                      .Select(x => new KeyValuePair<int, string>(x.OrdNumb, x.OrdNumb.ToString() + "\t" + 
                                                                 x.AskTxt.Substring(0, 200).PadRight(200) + "." + 
                                                                 Environment.NewLine));
            return shortAskList;
        }

        // ==========================================================================================================

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
    }
}
