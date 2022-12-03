using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using netCoreApi.ApiModels;
using netCoreApi.DatabaseModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace netCoreApi.Database
{
    public class Database :IDatabase
    {
        public Database(masterContext context, IMemoryCache memoryCache)
        {
            _context= context;
            _memoryCache= memoryCache;
        }
        public masterContext _context;
        private readonly IMemoryCache _memoryCache;

        public async Task<IEnumerable<Country>> FindCountryByIp( string ip)
        {
            Ipaddress ipaddress = new Ipaddress();
            List<Country> countries = new List<Country>();
            
            ipaddress = _context.Ipaddresses.Where(x => x.Ip == ip).FirstOrDefault();
            countries = await _context.Countries.Where(x => x.Id == ipaddress.CountryId).ToListAsync();

            var cacheOptions = new MemoryCacheEntryOptions().
                                  SetSlidingExpiration(TimeSpan.FromHours(100));
            _memoryCache.Set("countries", countries, cacheOptions);
            _memoryCache.Set("ip", ip, cacheOptions);

            return countries;
        }
        public async  Task<IEnumerable<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }
        public async Task<IEnumerable<Ipaddress>> GetIps()
        {
//            select count(*) CountryId, max(UpdatedAt) as max_LogDate, c.name
//from[master].[dbo].[IPAddresses] i
//inner join[master].[dbo].Countries c on(c.Id = i.CountryId)
//group by CountryId, c.name
//order by max_LogDate desc
                
            return await _context.Ipaddresses.OrderByDescending(x=> x.UpdatedAt).ToListAsync();
        }

        public async Task<bool> CreateUser(User user)
        {
            bool success =false;

            try
            {
                var exist = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();
                if (exist == null)
                {
                    var success1 = _context.Users.Add(user);
                    _context.SaveChanges();
                    success = true;
                }
                else
                    success = false;
            }
            catch(Exception ex)
            {
                success = false;
            }
            

            return success;
        }
        public  User FindUserRegister(User user)
        {
            bool success = false;
            User userAccount = new User();
            try
            {
                userAccount = _context.Users.Where(x => x.Token == user.Token).FirstOrDefault();
                //_context.SaveChanges();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }


            return userAccount;
        }
        public bool ChangeStatus(string token)
        {
            bool success = false;

            try
            {
                var user = _context.Users.Where(x => x.Token == token).FirstOrDefault();
                user.Status = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }


            return success;
        }
    }
}
