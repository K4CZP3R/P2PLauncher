using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class NetworkAdapterListItem
    {
        public NetworkAdapter NetworkAdapter { get; set; }
        public bool Marked { get; set; }

        public NetworkAdapterListItem(NetworkAdapter networkAdapter)
        {
            Marked = false;
            NetworkAdapter = networkAdapter;
        }

        public void FlipStatus()
        {
            Marked = !Marked;
        }

        public override string ToString()
        {
            string markStatus = Marked ? "Selected" : "Not selected";
            return $"{NetworkAdapter} - {markStatus}";
        }
    }
}
