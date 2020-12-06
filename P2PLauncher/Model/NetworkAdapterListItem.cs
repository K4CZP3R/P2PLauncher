using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace P2PLauncher.Model
{
    /// <summary>
    /// Wrapper for NetworkAdapter to be showable in ListBox.
    /// Contains NetworkAdapter and the status (Marked).
    /// </summary>
    public class NetworkAdapterListItem
    {
        /// <summary>
        /// NetworkAdapter
        /// </summary>
        public NetworkAdapter NetworkAdapter { get; set; }
        /// <summary>
        /// Status if this NetworkAdapter should be disabled on FreeLan start.
        /// </summary>
        public bool Marked { get; set; }

        public NetworkAdapterListItem(NetworkAdapter networkAdapter)
        {
            Marked = false;
            NetworkAdapter = networkAdapter;
        }

        public NetworkAdapterListItem(NetworkAdapter networkAdapter, bool state)
        {
            Marked = state;
            NetworkAdapter = networkAdapter;
        }

        /// <summary>
        /// Flips the Marked status
        /// true -> false,
        /// false -> true
        /// </summary>
        public void FlipStatus()
        {
            Marked = !Marked;
        }

        /// <summary>
        /// Returns readable version of NetworkAdapter and if it's selected or not.
        /// </summary>
        /// <returns>String containing NetworkAdapter and the selection status.</returns>
        public override string ToString()
        {
            string markStatus = Marked ? "Selected" : "Not selected";
            return $"{NetworkAdapter} - {markStatus}";
        }
    }
}
