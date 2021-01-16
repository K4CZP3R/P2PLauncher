using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    /// <summary>
    /// Enforce every window to have a simple set of functions.
    /// To maintain code readability.
    /// </summary>
    public interface IWindow
    {
        void UpdateWindow();
    }
}
