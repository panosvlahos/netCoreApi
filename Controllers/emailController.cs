using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using netCoreApi.ApiModels;
using netCoreApi.Database;
using netCoreApi.DatabaseModels;
using netCoreApi.Service;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace netCoreApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class emailController : ControllerBase
    {

        private readonly ILogger<netCoreApiController> _logger;
        private readonly IMemoryCache _memoryCache;


        private IDatabase _database { get; }
        public emailController(ILogger<netCoreApiController> logger, IDatabase database, IMemoryCache memoryCache)
        {
            _logger = logger;
            _database = database;
            _memoryCache = memoryCache;
        }
        [HttpGet(Name = "SendInvitationEmail")]
        public Task<bool> SendInvitationEmail()
        {
            bool success=false;
            Email email = new Email();
            try
            {
                email.SendInvitationEmail();
                success = true;
            }
            catch(Exception ex)
            {
                success = false;
            }

            return Task.FromResult(success);
        }
        [HttpGet(Name = "ConfirmEmail")]
        public Task<bool> ConfirmEmail(string token)
        {
            bool success = false;
            Email email = new Email();
            try
            {
                
                _database.ChangeStatus(token);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return Task.FromResult(success);
        }
    }
}