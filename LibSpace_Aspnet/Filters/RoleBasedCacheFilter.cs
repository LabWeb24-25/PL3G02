using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// Esta classe é o que vai aprovar se guarda em cache ou não
public class RoleBasedCacheFilter : ActionFilterAttribute
{
    private readonly string _role;

    public RoleBasedCacheFilter(string role)
    {
        _role = role;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var user = context.HttpContext.User;
        if (!user.IsInRole(_role))
        {
            context.Result = new ForbidResult(); // Bloqueia o acesso se a role não for correspondente
        }

        base.OnActionExecuting(context);
    }
}
