using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    /// <summary>
    /// State of FreeLan installation.
    /// UNK: Status is unknown, needs to be determined.
    /// CONFIG_NOT_SET: There is no entry in the config file.
    /// INVALID_PATH: Path in the config file does no (longer) exist.
    /// OK: Path in the config file does exist.
    /// </summary>
    public enum FreeLanInstallationStatus
    {
        [Description("Status is unknown.")]
        UNK,
        [Description("FreeLan path is unknown.")]
        CONFIG_NOT_SET,
        [Description("Current FreeLan path is invalid.")]
        INVALID_PATH,
        [Description("FreeLan is located and valid!")]
        OK
    }
}
