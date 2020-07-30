using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoilerPlate_API.Helpers
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeSpan { get; set; }
        public TimeSpan RefreshTokenLifeSpan { get; set; }
    }
}
