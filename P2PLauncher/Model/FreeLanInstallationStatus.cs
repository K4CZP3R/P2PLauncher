using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public enum FreeLanInstallationStatus
    {
        [Description("FreeLan path is unknown.")]
        CONFIG_NOT_SET,
        [Description("Current FreeLan path is invalid.")]
        INVALID_PATH,
        [Description("Current FreeLan executable is invalid.")]
        INVALID_EXECUTABLE,
        [Description("FreeLan is located and valid!")]
        OK
    }
}
