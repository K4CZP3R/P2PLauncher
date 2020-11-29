using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class WindowsServiceListItem
    {
        public WindowsService WindowsService { get; set; }
        public bool Marked { get; set; }

        public WindowsServiceListItem(WindowsService windowsService)
        {
            Marked = false;
            WindowsService = windowsService;
        }

        public void FlipStatus()
        {
            Marked = !Marked;
        }

        public override string ToString()
        {
            string markStatus = Marked ? "Selected" : "Not selected";
            return $"{WindowsService} - {markStatus}";
        }
    }
}
