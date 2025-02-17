namespace DesafioTaskrow.Application.Interfaces;

public interface ILogService
{
    Task LogInfo(string local, string detalhes);
    Task LogWarning(string local, string detalhes);
    Task LogError(string local, string detalhes);
}