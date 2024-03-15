using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;

namespace Viewers.Handler;

internal class Handler
{
    private readonly HubConnection _hubConnection;
    private readonly string _apiUrl;

    public Handler(HubConnection hubConnection)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        _apiUrl = configuration["DefaultConnection"];
        _hubConnection = hubConnection;
    }

    public async Task ConnectToMatchAsync(long matchId)
    {
        if (_hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync();
        }
        string statusUrl = $"{_apiUrl}ShowStatus?id={matchId}";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(statusUrl, new CancellationToken());

            if (response.IsSuccessStatusCode)
            {
                StatusType statusType = await response.Content.ReadFromJsonAsync<StatusType>();

                if (statusType == StatusType.InLive)
                {
                    Console.Clear();
                    
                    await _hubConnection.StartAsync();

                    await _hubConnection.SendAsync("JoinMatch", matchId.ToString());

                    Console.WriteLine($"Подключен к матчу {matchId}. Нажмите 'q' чтобы покинуть матч.");

                    _hubConnection.On<string>("CommentatorMessage", (message) => { Console.WriteLine(message); });

                    while (true)
                    {
                        if (Console.ReadKey().Key == ConsoleKey.Q)
                        {
                            Console.Clear();
                            await _hubConnection.SendAsync("LeaveMatch");
                            _hubConnection.Remove("CommentatorMessage");
                            break;
                        }
                    }

                    Console.WriteLine("Отключен от матча.");
                }
                else
                {
                    Console.WriteLine("Матч не в сети");
                }
            }
            else
            {
                Console.WriteLine($"Матч не найден");
            }
        }
    }

    public async Task ShowMatchesOnDateAsync(string date)
    {
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            string broadcastsUrl = $"{_apiUrl}ShowGames?date={date}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage gamesResponse = await client.GetAsync(broadcastsUrl, new CancellationToken());
                var games = await gamesResponse.Content.ReadFromJsonAsync<List<BroadcastResponse>>();
                if (games != null && games.Count > 0)
                {
                    PrintGamesToConsole(games);
                }
                else
                {
                    Console.WriteLine("Матчи на указанную дату не найдены");
                }
            }
        }
        else
        {
            Console.WriteLine("Введена неверная дата");
        }
    }

    public void PrintGamesToConsole(List<BroadcastResponse> games)
    {
        foreach (var game in games)
        {
            if (game.StatusType == StatusType.InLive)
            {
                int currentTime = DateTime.Now.Hour * 60 + DateTime.Now.Minute - game.DateStart.Hour * 60 -
                                  game.DateStart.Minute;
                int half;
                if (currentTime > 45)
                {
                    half = 2;
                }
                else
                {
                    half = 1;
                }

                Console.WriteLine(
                    $"Номер трансляции: {game.id}, Команды: {game.HomeCommandName} против {game.GuestCommandName}, Статус: {game.StatusType}, Тайм: {half}, Время: {currentTime}");
            }
            else
            {
                Console.WriteLine(
                    $"Номер трансляции: {game.id}, Команды: {game.HomeCommandName} против {game.GuestCommandName}, Статус: {game.StatusType}");
            }
        }
    }
}