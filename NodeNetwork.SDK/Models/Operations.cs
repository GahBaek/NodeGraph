using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeNetwork.SDK.Models
{
    // operations
    public class Operations
    {
        public static double Add(double x, double y)
            => x + y;
        public static double Sub(double x, double y)
            => x - y;
        public static double Mul(double x, double y)
            => x * y;
        public static double Div(double x, double y)
            => x / y;
    }
}
