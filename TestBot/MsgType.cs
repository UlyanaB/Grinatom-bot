using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    internal enum MsgType
    {
        None,   // неопределенное состояние
        Info,   // некоторая информация
        Enter,  // вход пользователя
        Exit    // выход пользователя
    }
}
