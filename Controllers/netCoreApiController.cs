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
    public class netCoreApiController : ControllerBase
    {

        private readonly ILogger<netCoreApiController> _logger;
        private readonly IMemoryCache _memoryCache;


        private IDatabase _database { get; }
        public netCoreApiController(ILogger<netCoreApiController> logger, IDatabase database, IMemoryCache memoryCache)
        {
            _logger = logger;
            _database = database;
            _memoryCache = memoryCache;
        }
        [HttpGet(Name = "GetCountries")]
        public async Task<GetModel> GetCountries(string ip)
        {
            GetModel returnGet = new GetModel();
            IEnumerable<Country> countries;
            string ipCache;

            bool exist = _memoryCache.TryGetValue("countries", out countries);
            _memoryCache.TryGetValue("ip", out ipCache);

            if (!exist || ipCache != ip)
            {


                countries = await _database.FindCountryByIp(ip);


            }
            else
            {
                _memoryCache.TryGetValue("countries", out countries);

            }
            returnGet.Name = countries.FirstOrDefault()?.Name;
            returnGet.TwoLetterCode = countries.FirstOrDefault()?.TwoLetterCode;
            returnGet.ThreeLetterCode = countries.FirstOrDefault()?.ThreeLetterCode;

            return returnGet;
        }
        [HttpGet(Name = "Report")]
        public async Task<List<GetReport>> Report()
        {
            List<GetReport> report = new List<GetReport>();
            var countries = await _database.GetCountries();
            var ips = await _database.GetIps();
            foreach (var country in countries)
            {
                var currentIp = ips.ToList().Where(i => i.CountryId == country.Id).OrderByDescending(i => i.UpdatedAt).FirstOrDefault();
                var count = ips.ToList().Where(i => i.CountryId == country.Id).Count();
                GetReport reportItem = new GetReport();
                reportItem.CountryName = country.Name;
                reportItem.AddresCount = count;
                reportItem.LstAddressUpdated = currentIp.UpdatedAt;

                report.Add(reportItem);

            }
            return report;
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<bool> CreateUser(Users user)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, 5)
                                                    .Select(s => s[random.Next(s.Length)]).ToArray());
            User userDb = new User();
            userDb.Email = user.Email;
            userDb.FirstName = user.FirstName;
            userDb.Password = user.Password;
            userDb.LastName = user.LastName;
            userDb.Status = false;
            userDb.Token = randomString;
            bool create = await _database.CreateUser(userDb);
            if (create)
            {
                Email email = new Email();
                email.SendEmail(user);
                var dbUser =_database.FindUserRegister(userDb);
                var urlScheme = Request.Scheme;
                string urlHost = Request.Host.Host;
                int? urlPort = Request.Host.Port;
                email.SendConfirmEmail(dbUser.Token, dbUser.Email, urlHost, urlPort);
            }
            return create;
        }
    }
}