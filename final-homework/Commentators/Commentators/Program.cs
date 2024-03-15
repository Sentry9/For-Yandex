using Commentators.Handler;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

public static class Program
{
    private static HubConnection _hubConnection;

    public static async Task Main(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        string apiUrl = configuration["DefaultConnection"];
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{apiUrl}broadcast").Build();

        var handler = new Handler(_hubConnection);
        bool IsRunning = true;
        while (IsRunning)
        {
            long id;
            Console.WriteLine("Введите номер трансляции для подключения, с для регистрации трансляции, start для запуска трансляции, stop для остановки трансляции или введите q для выхода ");
            string matchId = Console.ReadLine();
            switch (matchId)
            {
                case "q":
                    IsRunning = false;
                    break;

                case "c":
                    var model = await handler.CreateModel();
                    if (model == null)
                    {
                        break;
                    }
                    await handler.CreateBroadcast(model);
                    break;
                
                case "start":
                    await handler.StartBroadcast();
                    break;
                
                case "stop":
                    await handler.StopBroadcast();
                    break;

                default:
                    if (long.TryParse(matchId, out long longValue))
                    {
                        id = longValue;
                        await handler.ConnectToMatchAsync(id);
                    }
                    else
                    {
                        Console.WriteLine("Введены неверные данные");
                    }
                    break;
            }
        }
    }
}