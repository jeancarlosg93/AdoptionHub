using Microsoft.EntityFrameworkCore;

namespace AdoptionHub.Services;

using AdoptionHub.Models;
using AdoptionHub.Contexts;

public class LogInLogService
{
    private readonly ApplicationDbContext _context;

    public LogInLogService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateLogRegistry(string msg)
    {
        var logEntry = new LoginLog
        {
            Date = DateTime.Now,
            Message = msg
        };

        await _context.LoginLogs.AddAsync(logEntry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<LoginLog>> GetLogs()
    {
        var logs = await _context.LoginLogs.ToListAsync();
        return logs;
    }
}
