using System.Net.Http.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using TextStream.Api.Contracts.Requests;
using TextStream.Api.Contracts.Response;
using TextStream.Api.Contracts.Types;

namespace Commentators.Handler;

internal class Handler
{
    private readonly HubConnection _hubConnection;
    private DateTime _matchTime;
    private string _apiUrl;

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
        string StatusUrl = $"{_apiUrl}ShowStatus?id={matchId}";

        using (HttpClient client = new HttpClient())
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
            }

            HttpResponseMessage response = await client.GetAsync(StatusUrl, new CancellationToken());

            if (response.IsSuccessStatusCode)
            {
                StatusType statusType = await response.Content.ReadFromJsonAsync<StatusType>();

                if (statusType == StatusType.InLive)
                {
                    Console.Clear();
                    await _hubConnection.StartAsync();

                    await _hubConnection.SendAsync("JoinMatch", matchId.ToString());

                    Console.WriteLine(
                        $"Подключен к матчу {matchId}. Нажмите 'q' чтобы покинуть матч или 'm' чтобы отправить сообщение.");
                    _hubConnection.On<string>("CommentatorMessage", (message) => { Console.WriteLine(message); });

                    while (true)
                    {
                        var key = Console.ReadKey().Key;
                        if (key == ConsoleKey.Q)
                        {
                            Console.Clear();
                            await _hubConnection.SendAsync("LeaveMatch");
                            _hubConnection.Remove("CommentatorMessage");
                            break;
                        }

                        if (key == ConsoleKey.M)
                        {
                            Console.WriteLine("\nВведите сообщение");
                            var message = Console.ReadLine();
                            var formatedMessage = await FormatMessage(message, matchId);
                            await _hubConnection.SendAsync("SendCommentatorMessage", formatedMessage, matchId.ToString());
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

    public async Task CreateBroadcast(BroadcastRequest model)
    {
        string CreateUrl = $"{_apiUrl}broadcast";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(CreateUrl, model, new CancellationToken());
            long id = await response.Content.ReadFromJsonAsync<long>();
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Трансляция номер {id} зарегистрированна");
            }
            else
            {
                Console.WriteLine("Ошибка при регистрации трансляции");
            }
        }
    }

    public async Task StartBroadcast()
    {
        Console.WriteLine("Введите номер трансляции");
        string id = Console.ReadLine();
        string StartUrl = $"{_apiUrl}Start?id={id}";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(StartUrl, null, new CancellationToken());
            if (response.IsSuccessStatusCode)
            {
                
                Console.WriteLine("Трансляция запущена");
            }
            else
            {
                Console.WriteLine("Ошибка при запуске трансляции");
            }
        }
    }
    
    public async Task StopBroadcast()
    {
        Console.WriteLine("Введите номер трансляции");
        string id = Console.ReadLine();
        string StopUrl = $"{_apiUrl}Stop?id={id}";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.PostAsync(StopUrl, null, new CancellationToken());
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Трансляция завершена");
            }
            else
            {
                Console.WriteLine("Ошибка при завершении трансляции");
            }
        }
    }

    public Task<BroadcastRequest?> CreateModel()
    {
        var model = new BroadcastRequest();
        Console.WriteLine("Введите название команды в гостях");
        string GueastCommand = Console.ReadLine();
        if (GueastCommand == "")
        {
            Console.WriteLine("Название комманды не может быть пустым");
            return Task.FromResult<BroadcastRequest?>(null);
        }
        model.GuestCommandName = GueastCommand;
        Console.WriteLine("Введите название команды, которая играет дома");
        string HomeCommand = Console.ReadLine();
        if (HomeCommand == "" || HomeCommand == GueastCommand)
        {
            Console.WriteLine("Имя команды не может быть пустым или совпадать с командой соперников");
            return Task.FromResult<BroadcastRequest?>(null);
        }
        model.HomeCommandName = HomeCommand;
        Console.WriteLine("Введите дату начала трансляции в формате YYYY-MM-DDTHH:MM");
        string date = Console.ReadLine();
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            model.DateStart = parsedDate;
        }
        else
        {
            Console.WriteLine("Некорректный формат даты. Пожалуйста, введите дату в правильном формате.");
            return Task.FromResult<BroadcastRequest?>(null);
        }

        return Task.FromResult(model);
    }
    
    private async Task<string> FormatMessage(string message, long matchId)
    {
        Console.WriteLine("Выберите тип сообщения:\nГол - 0\nVAR - 1\nЗамена - 2\nЖелтая карточка - 3\nКрасная карточка - 4\nНажмите enter, чтобы продолжить без указания типа события");
        string eventType = Console.ReadLine();
        string formatedMessage = "";
        string FindUrl = $"{_apiUrl}id?id={matchId}";
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(FindUrl, new CancellationToken());
            BroadcastResponse model = await response.Content.ReadFromJsonAsync<BroadcastResponse>();
            DateTime startTime = model.DateStart;
            if (response.IsSuccessStatusCode)
            {
                int currentTime = DateTime.Now.Minute + DateTime.Now.Hour * 60 - startTime.Minute - startTime.Hour * 60;
                formatedMessage = $"{currentTime} минута; ";
            }
            else
            {
                Console.WriteLine("Ошибка при регистрации трансляции");
            }
        }

        switch (eventType)
        {
            case "0":
                formatedMessage += "Гол; ";
                break;
            case "1":
                formatedMessage += "VAR; ";
                break;
            case "2":
                formatedMessage += "Замена; ";
                break;
            case "3":
                formatedMessage += "Желтая карточка; ";
                break;
            case "4":
                formatedMessage += "Красная карточка; ";
                break;
        }

        formatedMessage += message;

        return formatedMessage;
    }
} 