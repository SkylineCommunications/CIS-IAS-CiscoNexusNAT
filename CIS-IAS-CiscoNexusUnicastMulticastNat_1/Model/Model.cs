namespace Skyline.Automation.CiscoNexusUnicastMulticastNat.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using Skyline.Automation.CiscoNexusUnicastMulticastNat.View;
    using Skyline.Automation.CiscoNexusUnicastMulticastNat.Utils;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Core.DataMinerSystem.Automation;
    using Skyline.DataMiner.Core.DataMinerSystem.Common;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class Model
    {
        public Model(Engine engine)
        {
            Engine = engine;
            Dms = engine.GetDms() ?? throw new NullReferenceException("dms");
            CiscoNexusElement = LoadElement();
            LoadInterfaces();
        }

        public IDmsElement CiscoNexusElement { get; set; }

        public List<string> PhysicalInterfaces { get; set; }

        public List<string> LoopbackInterfaces { get; set; }

        private Engine Engine { get; }

        private IDms Dms { get; }

        internal void PushConfigs(View view)
        {
            Engine.GenerateInformation("Pushing Configs...");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"interface {view.SourceInterfaceName.Text};");
            stringBuilder.Append($"ip address {view.LoopbackIpAddress.Text};");
            stringBuilder.Append($"ip address {view.UnicastSourceIpAddress2.Text}/24 secondary;");
            stringBuilder.Append($"ip router ospf {view.RouterOspfId.Text} area {view.RouterOspfArea.Text};");
            stringBuilder.Append($"ip pim sparse-mode");

            CiscoNexusElement.GetStandaloneParameter<string>((int)Utils.Pids.AddCommandWritePid).SetValue(stringBuilder.ToString());
            Engine.GenerateInformation("Comamnds Sent: " + stringBuilder);
            stringBuilder.Clear();

            stringBuilder.Append($"ip service-reflect source-interface {view.SourceInterfaceName.Text};");
            stringBuilder.Append($"ip service-reflect destination {view.UnicastSourceIpAddress2.Text} to {view.DestinationMulticastIpAddress.Text} mask-len 32 source {view.UnicastSourceIpAddress1.Text} to {view.DestinationUnicastIpAddress.Text} mask-len 32;");
            stringBuilder.Append($"multicast service-reflect dest-prefix 0.0.0.0/0 map interface {view.DestinationPhysicalInterface.Selected} max-replication 3");

            CiscoNexusElement.GetStandaloneParameter<string>((int)Utils.Pids.AddCommandWritePid).SetValue(stringBuilder.ToString());

            Engine.GenerateInformation("Comamnds Sent: " + stringBuilder);
        }

        internal void RemoveConfigs(View view)
        {
            Engine.GenerateInformation("Deleting Configs...");

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"no multicast service-reflect dest-prefix 0.0.0.0/0 map interface {view.DestinationPhysicalInterface.Selected} max-replication 3;");
            stringBuilder.Append($"no ip service-reflect destination {view.UnicastSourceIpAddress2.Text} to {view.DestinationMulticastIpAddress.Text} mask-len 32 source {view.UnicastSourceIpAddress1.Text} to {view.DestinationUnicastIpAddress.Text} mask-len 32;");
            stringBuilder.Append($"no ip service-reflect source-interface {view.SourceInterfaceName.Text}");
            CiscoNexusElement.GetStandaloneParameter<string>((int)Utils.Pids.AddCommandWritePid).SetValue(stringBuilder.ToString());
            stringBuilder.Clear();

            CiscoNexusElement.GetStandaloneParameter<string>((int)Utils.Pids.AddCommandWritePid).SetValue($"no interface {view.SourceInterfaceName.Text}");
        }

        /// <summary>
        /// Retrieves a Network Address of an IP Address. WARNING IT IS ONLY WORKING FOR /24.
        /// </summary>
        /// <param name="sourceIpAddress">The IP Address.</param>
        /// <param name="subnetMask">The Subnet Mask CIDR, e.g. 24.</param>
        /// <returns>The Network Address in the format [NetowrkAddress]/[CIDR Subnet mask].</returns>
        private static string GetNetworkAddress(string sourceIpAddress, string subnetMask)
        {
            var ipAddressOctets = sourceIpAddress.Split('.');
            ipAddressOctets[ipAddressOctets.Length - 1] = "0";
            return String.Join(".", ipAddressOctets) + "/" + subnetMask;
        }

        private IDmsElement LoadElement()
        {
            var dummy = Engine.GetDummy(Utils.DummyId);
            var element = Dms.GetElement(new DmsElementId(dummy.DmaId, dummy.ElementId));

            if (element.State != ElementState.Active)
            {
                Engine.ExitFail($"{element.Name} element is not active!");
            }

            if (element.GetStandaloneParameter<int?>((int)Utils.Pids.SshLoginStatusPid).GetValue() != (int)Utils.SshState.Connected)
            {
                Engine.ExitFail($"{element.Name} is not SSH authenticated!");
            }

            if (element.GetStandaloneParameter<int?>((int)Utils.Pids.CLICommunicationType).GetValue() != (int)Utils.CliCommType.NX_API)
            {
                Engine.ExitFail($"{element.Name} is not working in NX API mode.");
            }

            return element;
        }

        private void LoadInterfaces()
        {
            var interfaceTable = CiscoNexusElement.GetTable((int)Utils.Pids.DetailedInterfaceTablePid);
            var interfaceTableData = interfaceTable.GetData();

            PhysicalInterfaces = interfaceTableData/*.Where(kvp => (kvp.Value[(int)Utils.Idx.DetailedInterfaceInterfaceTypeIdx] as string) == Utils.PhysicalInterfaceType)*/
                                     .Select(kvp => kvp.Value[(int)Utils.Idx.DetailedInterfaceInterfaceNameIdx] as string)
                                     .ToList();

            //LoopbackInterfaces = interfaceTableData.Where(kvp => (kvp.Value[(int)Utils.Idx.DetailedInterfaceInterfaceTypeIdx] as string) == "24")
            //                         .Select(kvp => kvp.Value[(int)Utils.Idx.DetailedInterfaceInterfaceNameIdx] as string)
            //                         .ToList();
        }
    }
}
