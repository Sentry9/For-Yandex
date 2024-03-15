namespace TextStream.Host.Hub;
using Microsoft.AspNetCore.SignalR;

public class BroadcastHub : Hub
{
    private static Dictionary<string, List<string>> matchConnections = new Dictionary<string, List<string>>();
    private static Dictionary<string, string> userMatches = new Dictionary<string, string>();

    public async Task JoinMatch(string matchId)
    {
        string connectionId = Context.ConnectionId;

        if (!matchConnections.ContainsKey(matchId))
        {
            matchConnections[matchId] = new List<string>();
        }

        matchConnections[matchId].Add(connectionId);
        userMatches[connectionId] = matchId;

        await Groups.AddToGroupAsync(connectionId, matchId);
        await Clients.Group(matchId).SendAsync("CommentatorMessage", $"{Context.ConnectionId} присоединился к трансляции.");

    }

    public async Task LeaveMatch()
    {
        if (userMatches.TryGetValue(Context.ConnectionId, out string matchId))
        {
            matchConnections[matchId].Remove(Context.ConnectionId);
            userMatches.Remove(Context.ConnectionId);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId);
            await Clients.Group(matchId).SendAsync("CommentatorMessage", $"{Context.ConnectionId} покинул трансляцию.");

            if (matchConnections[matchId].Count == 0)
            {
                matchConnections.Remove(matchId);
            }
        }
    }

    public async Task SendCommentatorMessage(string message, string matchId)
    { 
        await Clients.Group(matchId).SendAsync("CommentatorMessage", $"Commentator: {message}");
    }
}