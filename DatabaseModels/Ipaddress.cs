using System;
using System.Collections.Generic;

namespace netCoreApi.Models
{
    public partial class Ipaddress
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Ip { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
