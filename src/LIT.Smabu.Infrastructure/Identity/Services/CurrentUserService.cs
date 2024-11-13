using LIT.Smabu.Core;
using Microsoft.AspNetCore.Http;

namespace LIT.Smabu.Infrastructure.Identity.Services
{
    public class CurrentUserService : ICurrentUser
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor?.HttpContext?.User;
            //this.Id = user?.Claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? "?";
            this.Name = user?.Claims.FirstOrDefault(x => x.Type == "name")?.Value ?? "?";           
            this.Username = user?.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value ?? "?";
        }

        //public string Id { get; }
        public string Name { get; }
        public string Username { get; }
    }
}
