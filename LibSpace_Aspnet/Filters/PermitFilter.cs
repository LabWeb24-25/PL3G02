using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using LibSpace_Aspnet.Data;
using Microsoft.EntityFrameworkCore;

public class PermitFilter : IAsyncActionFilter
{
    private readonly ApplicationDbContext _dbContext;

    public PermitFilter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        // Verifica se o usuário está autenticado
        if (!user.Identity.IsAuthenticated)
        {
            await next();

        }

        // Obtém o ID do usuário autenticado
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            await next();

        }

        // Verifica se o usuário está bloqueado no banco de dados
        var isBlocked = await _dbContext.Bloquears
            .AsNoTracking()
            .AnyAsync(b => b.IdUser == userId && b.EstadoBloqueio);

        if (isBlocked)
        {
            context.Result = new RedirectToActionResult("LockOut", "Acess", null);
            return;
        }

        await next();
    }
}