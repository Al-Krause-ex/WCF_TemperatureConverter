using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MyConverterServerClient
{
    [ServiceContract]
    public interface IMyConverter
    {
        [OperationContract]
        bool TestConnection();

        //Объявление пути для http запроса
        [WebInvoke(Method = "GET", UriTemplate = "/FtoC?f={tempF}", RequestFormat = WebMessageFormat.Json, 
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        double FtoC(double tempF);
        
        [WebInvoke(Method = "GET", UriTemplate = "/CtoF?c={tempC}", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        double CtoF(double tempC); 
    }

    public class MyConverter : IMyConverter
    {
        public bool TestConnection() //Проверка функционирования сервиса
        {
            return true;
        }

        public double FtoC(double tempF) //Фаренгейт в Цельсий
        {
            return (tempF - 32) * 5 / 9;
        }

        public double CtoF(double tempC) //Цельсий в Фаренгейт
        {
            return (tempC * 9 / 5) + 32;
        }
    }
    class Server
    {
        static void Main(string[] args)
        {
            // Инициализируем службу, указываем адрес, по которому она будет доступна
            var host = new WebServiceHost(typeof(MyConverter), new Uri("http://localhost:8000/MyConverter"));

            // Добавляем конечную точку службы с заданным интерфейсом, привязкой (создаём новую) и адресом конечной точки
            host.AddServiceEndpoint(typeof(IMyConverter), new WebHttpBinding(), "");

            //Два варианта, webHttpBinding - в браузере, BasicHttpBinding - в консольном приложении
            host.AddServiceEndpoint(typeof(IMyConverter), new BasicHttpBinding(), "basic");

            // Запускаем службу
            host.Open();

            Console.WriteLine("Сервер запущен");
            Console.ReadLine();

            // Закрываем службу
            host.Close();
        }
    }
}
