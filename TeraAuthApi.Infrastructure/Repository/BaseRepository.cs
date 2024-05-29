using Microsoft.Extensions.Logging;
using TeraAuthApi.Infrastructure.DataContext;

namespace TeraAuthApi.Infrastructure.Repository;

public abstract class BaseRepository
{
    internal readonly TeraDbContext _context;
    internal readonly ILogger _logger;
    public BaseRepository(TeraDbContext context = null, ILogger logger = null)
    {
        _context = context;
        _logger = logger;
    }

}