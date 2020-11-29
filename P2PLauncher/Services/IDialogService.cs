using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        string FilePath { get; set; }
        bool OpenFileDialog();
    }
}
