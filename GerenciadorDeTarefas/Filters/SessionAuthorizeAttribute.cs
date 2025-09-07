using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GerenciadorDeTarefas.Filters
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            if (userId == null) 
            {
                context.Result = new RedirectToActionResult("Login", "Usuario", null);
            }

            base.OnActionExecuting(context);
        }
    }
}
