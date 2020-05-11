using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBot
{
    enum States
    {
        none,               // пустое состояние
        start,              // бот запущен
        login,              // успешно вошли в игру (т.е. логин и пароль правильные)
        enter_name,         // введено имя пользователя
        enter_pas,          // введен пароль
    }
}
