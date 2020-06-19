using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    [ServiceContract]
    public interface ITestBotService
    {
        [OperationContract]
        bool GetUser(int position, long tlgUserId, string tlgUserName, string lastEnter, int bestResult);
    }
}
