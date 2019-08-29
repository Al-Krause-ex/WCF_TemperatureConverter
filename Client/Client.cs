using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Client
{
    [ServiceContract]
    public interface IMyConverter
    {
        [OperationContract]
        bool TestConnection();

        [OperationContract]
        double FtoC(double tempF);

        [OperationContract]
        double CtoF(double tempC);
    }
    class Client
    {
        static void Main(string[] args)
        {
            //Задаём адрес нашей службы
            var tcpUri = new Uri("http://localhost:8000/MyConverter/basic");

            //Создаём сетевой адрес, с которым клиент будет взаимодействовать 
            var address = new EndpointAddress(tcpUri);
            var binding = new BasicHttpBinding();

            //Данный класс используется клиентами для отправки сообщений
            var factory = new ChannelFactory<IMyConverter>(binding, address);

            //Открываем канал для общения клиента со службой
            var service = factory.CreateChannel();

            bool test;
            Exception expection = null;

            try
            {
                test = service.TestConnection();
            } catch (Exception e)
            {
                test = false;
                expection = e;
            }

            Console.WriteLine("Соединение с сервером: " + (test ? "есть" : $"отсутствует \n\n({expection.Message})"));
            if (test)
            {
                bool shouldExit = false;
                while (!shouldExit)
                {
                    Console.WriteLine("\nВыберите цифру: \n1.Цельсий => Фаренгейт \n2.Фаренгейт => Цельсий \n3.Выйти ");

                    var answer = ReadIntSafe();

                    switch (answer)
                    {
                        case 1:
                            Console.Write("Введите температуру в градусах Цельсия: ");
                            double tempC = ReadIntSafe();
                            double tempF = service.CtoF(tempC);

                            Console.WriteLine($"Фаренгейт: {Math.Round(tempF, 3)}");
                            break;

                        case 2:
                            Console.Write("Введите температуру в градусах Фаренгейт: ");
                            double tempF1 = ReadIntSafe();
                            double tempC1 = service.FtoC(tempF1);

                            Console.WriteLine($"Цельсий: {Math.Round(tempC1, 3)}");
                            break;

                        case 3:
                            Console.WriteLine("Всего доброго! :) \nНажмите <Any Key> для выхода");
                            shouldExit = true;
                            break;

                        default:
                            Console.WriteLine("Вы ввели некорректный символ");
                            break;
                    } 
                }
            }

            Console.ReadKey();
        }

        static int ReadIntSafe()
        {
            var rawInput = Console.ReadLine();
            int result;
            while (!int.TryParse(rawInput, out result))
            {
                Console.WriteLine($"Вы ввели не число: {rawInput}. \nВведите число");
                rawInput = Console.ReadLine();
            }
            return result;
        }
    }
}