using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading; //пространство имен содержит в себе классы ,поддерживающие многопоточное программирование

namespace Lab4
{
    class Token //класс токен
    {
        public String data;
        public int recipient;

        public Token(string data, int recipient)
        {
            this.data = data;
            this.recipient = recipient;
        }
    }

    class Program
    {
        static object locker = new object();

        static void send(int num, Token token, int count)
        {

            lock (locker) //блокировка потоков. т.е пока первый поток выполняет ТУ ЧАСТЬ КОДА КОТОРАЯ НАПИСАНА ДЛЯ  lock (locker) - другой поток не сможет сюда влезть
            {
                if (num + 1 == token.recipient) //если получатель
                {
                    Console.WriteLine("Recipient " + (num + 1) + " получил token: data= " + token.data + ", recipient= " + token.recipient);
                }
                else if (num+1 < token.recipient) // передаем другому
                {
                    Console.WriteLine("Поток " + (num + 1) + " посылает token дальше");
                }
                
            }
        }

        static void Main(string[] args)
        {
            Token token = new Token("token", 5);
            int count = 10;
            if (count < token.recipient)
                count = token.recipient;
            for (int i = 0; i < count; i++)
            {
                Thread thread = new Thread(() => send(i, token, count)); //поток = Thread...в цикле создаем кол-во потоков ( но еще не запустили - только экземпляры). new Thread( )  - всегда принимает функцию БЕЗ параметров - в данном случае мы сказали что у нас тут есть функция  () => - она безымянная и ее тело это вызов другой функции  send(i, token, count)) - в которой ЕСТЬ параметры - номер потока, токен, КОЛ-ВО потоков.
                Console.WriteLine("Запуск потока " + (i + 1));
                thread.Start(); //thread.Start(); - стандартная функия ЗАПУСКА потока
                Thread.Sleep(100);//Thread.Sleep(100); - усыпление потока - чтобы все успели проработать нормально
            }
            Console.ReadLine();
        }
    }
}
