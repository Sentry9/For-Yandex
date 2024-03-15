using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Viewers.Handler;

class Program
{
    static async Task Main(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        string apiUrl = configuration["DefaultConnection"];
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiUrl}broadcast").Build();
        
        var handler = new Handler(hubConnection);

        while (true)
        {
            Console.WriteLine("Введите номер трансляции для подключения или s для просмотра списка трансляций, для выхода нажмите q");
            string matchId = Console.ReadLine();

            if (matchId == "q")
            {
                break;
            }

            if (matchId == "s")
            {
                Console.WriteLine("Введите дату трансляции в формате YYYY-MM-DD");
                var date = Console.ReadLine();
                await handler.ShowMatchesOnDateAsync(date);
                continue;
            }

            if (long.TryParse(matchId, out long longValue))
            {
                await handler.ConnectToMatchAsync(longValue);
            }
            else
            {
                Console.WriteLine("Введены неверные данные");
            }
        }
    }
}