using DesafioTaskrow.Application.Interfaces;
using DesafioTaskrow.Domain;
using DesafioTaskrow.Domain.Enums;
using DesafioTaskrow.Domain.Models;

namespace DesafioTaskrow.Application.Servicos;

public class LogService : ILogService
{
    private readonly Contexto _context;
    
    public LogService(Contexto contexto)
    {
        _context = contexto;
    }
    public async Task LogInfo(string local, string detalhes)
    {
        var log = new Log()
        {
            Local = local,
            Detalhes = detalhes,
            LogLevel = EnumLogLevel.Info
        };
        
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task LogWarning(string local, string detalhes)
    {
        var log = new Log()
        {
            Local = local,
            Detalhes = detalhes,
            LogLevel = EnumLogLevel.Warning
        };
        
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task LogError(string local, string detalhes)
    {
        var log = new Log()
        {
            Local = local,
            Detalhes = detalhes,
            LogLevel = EnumLogLevel.Error
        };
        
        await _context.Logs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}