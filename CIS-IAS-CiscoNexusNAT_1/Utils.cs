using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyline.Automation.CiscoNexusNat.Utils
{
    public static class Utils
    {
        public static readonly int DummyId = 10;
        public static readonly string PhysicalInterfaceType = "Ethernet CSMACD";
        public static readonly string LoopbackInterfaceType = "Software Loop Back";
        public static readonly int LabelWidth = 150;
        public static readonly int TextBoxWidth = 250;

        public enum Pids
        {
            DetailedInterfaceTablePid = 2800,
            DetailedInterfaceInterfaceNamePid = 2802,
            SshLoginStatusPid = 9632,
            AddCommandWritePid = 9670,
            CommandHistoryTablePid = 9700,
            CommandHistoryTableCommandPid = 9702,
            CommandHistoryTableCommandOutputPid = 9703,
            CLICommunicationType = 9623,
        }

        public enum Idx
        {
            DetailedInterfaceInterfaceNameIdx = 1,
            CommandHistoryTableCommandIdx = 1,
            DetailedInterfaceInterfaceTypeIdx = 2,
            CommandHistoryTableCommandOutputIdx = 2,
        }

        public enum SshState
        {
            Connected = 1,
            Disconnected = 0,
        }

        public enum CliCommType
        {
            SSH = 1,
            NX_API = 2,
        }
    }
}
