using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tracker.Shared.Cors;

internal sealed class CorsOptions
{
    public bool AllowCredentials { get; set; }
    public IEnumerable<string>? AllowedOrigins { get; set; }
    public IEnumerable<string>? AllowedMethods { get; set; }
    public IEnumerable<string>? AllowedHeaders { get; set; }
}