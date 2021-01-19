using P2PLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Utils
{
    public static class AddressHelper
    {
        public static AddressType GetAddressType(string input)
        {
            if (input.Contains("."))
                return AddressType.IPV4;
            return AddressType.UNKNOWN;
        }
    }
}
