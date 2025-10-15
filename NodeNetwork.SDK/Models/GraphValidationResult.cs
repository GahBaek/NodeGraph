using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetworkSDK.Models
{
    public class GraphValidationResult
    {
        public bool IsValid { get; }
        public string? Error { get; }
        public IReadOnlyList<Guid>? Order { get; }

        private GraphValidationResult(bool ok, string? err, IReadOnlyList<Guid>? order)
        { IsValid = ok; Error = err; Order = order; }

        public static GraphValidationResult Ok(IReadOnlyList<Guid> order) => new(true, null, order);
        public static GraphValidationResult Ok() => new(true, null, null);
        public static GraphValidationResult Fail(string err) => new(false, err, null);
    }
}
