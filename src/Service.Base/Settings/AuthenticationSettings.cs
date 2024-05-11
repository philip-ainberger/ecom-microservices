using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Base.Settings;

public class AuthenticationSettings
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
}
