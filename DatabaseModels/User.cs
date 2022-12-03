using System;
using System.Collections.Generic;

namespace netCoreApi.DatabaseModels
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool Status { get; set; }
        public string Token { get; set; } = null!;
    }
}
