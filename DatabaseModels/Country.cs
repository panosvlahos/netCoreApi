using System;
using System.Collections.Generic;

namespace netCoreApi.DatabaseModels
{
    public partial class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TwoLetterCode { get; set; } = null!;
        public string ThreeLetterCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
