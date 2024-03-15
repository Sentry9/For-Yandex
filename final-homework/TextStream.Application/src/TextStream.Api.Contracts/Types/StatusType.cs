namespace TextStream.Api.Contracts.Types;
/// <summary>
/// Статусы трансляции
/// </summary>
public enum StatusType : short
{
    Soon = 0,
    InLive = 1, /// В ожидании
    Finished = 2, /// Одобрено
}