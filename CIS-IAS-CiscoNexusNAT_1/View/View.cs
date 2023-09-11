namespace Skyline.Automation.CiscoNexusNat.View
{
    using System;
    using System.Text.RegularExpressions;
    using Skyline.Automation.CiscoNexusNat.Utils;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class View : Dialog
    {
        public View(IEngine engine) : base(engine)
        {
            Title = "Egress NAT";
            Width = 900;
            AllowOverlappingWidgets = true;

            DestinationPhysicalInterface = new DropDown();
            SourcePhysicalInterface = new DropDown();
            DestinationIpAddress = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.37.10" };
            SourceInterfaceName = new TextBox { Width = Utils.TextBoxWidth, Text = "loopback3" };
            LoopbackIpAddress = new TextBox { Width = Utils.TextBoxWidth , Text = "192.168.37.100/24" };
            RouterOspfId = new TextBox{ Width = Utils.TextBoxWidth , Text="1"};
            RouterOspfArea = new TextBox{ Width = Utils.TextBoxWidth, Text="0.0.0.0" };
            DestinationMulticastIpAddress = new TextBox{ Width = Utils.TextBoxWidth, Text= "237.0.10.10" };
            SourceMulticastIpAddress = new TextBox{ Width = Utils.TextBoxWidth, Text= "239.0.0.10" };
            SourceIpAddress = new TextBox{ Width = Utils.TextBoxWidth, Text= "192.168.23.10" };

            SendCommand = new Button("Create NAT") { Margin = new Margin(10,10,10,10)};
            DeleteCommand = new Button("Delete NAT") { Margin = new Margin(10, 10, 10, 10) };
            ErrorLabel = new Label(String.Empty);
            InitializeView();
        }

        public DropDown DestinationPhysicalInterface { get; set; }

        public DropDown SourcePhysicalInterface { get; set; }

        public TextBox SourceInterfaceName { get; set; }

        public TextBox LoopbackIpAddress { get; set; }

        public TextBox RouterOspfId { get; set; }

        public TextBox RouterOspfArea { get; set; }

        public TextBox DestinationMulticastIpAddress { get; set; }

        public TextBox SourceMulticastIpAddress { get; set; }

        public TextBox SourceIpAddress { get; set; }

        public TextBox DestinationIpAddress { get; set; }

        public Button SendCommand { get; set; }

        public Label ErrorLabel { get; set; }

        public Button DeleteCommand { get; set; }

        public bool ValidateInputs()
        {
            var ipAddressPattern = "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (!Regex.IsMatch(LoopbackIpAddress.Text.Split('/')[0], ipAddressPattern))
            {
                LoopbackIpAddress.ValidationState = UIValidationState.Invalid;
                LoopbackIpAddress.ValidationText = "IP Address invalid";
                return false;
            }

            if (!Regex.IsMatch(DestinationMulticastIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(SourceMulticastIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(SourceIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(DestinationIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            return true;
        }

        private void InitializeView()
        {
            AddWidget(new Label("Source Interface Name") { Width = Utils.LabelWidth },0,0);
            AddWidget(SourceInterfaceName, 0, 1);
            AddWidget(new Label("Loopback IP Address") { Width = Utils.LabelWidth }, 0, 2);
            AddWidget(LoopbackIpAddress, 0, 3);
            AddWidget(new Label("Source Physical Interface") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(SourcePhysicalInterface, RowCount, 1);
            AddWidget(new Label("Destination Physical Interface") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(DestinationPhysicalInterface, RowCount, 3);
            AddWidget(new Label("Source Multicast IP Address") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(SourceMulticastIpAddress, RowCount, 1);
            AddWidget(new Label("Destination Multicast IP Address") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(DestinationMulticastIpAddress, RowCount, 3);
            AddWidget(new Label("Source Unicast IP Address") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(SourceIpAddress, RowCount, 1);
            AddWidget(new Label("Destination Unicast IP Address") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(DestinationIpAddress, RowCount, 3);
            AddWidget(new Label("Router OSPF ID") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(RouterOspfId, RowCount, 1);
            AddWidget(new Label("Router OSPF Area") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(RouterOspfArea, RowCount, 1);
            AddWidget(SendCommand, RowCount + 1, 0);
            AddWidget(DeleteCommand, RowCount, 1);
            AddWidget(ErrorLabel, RowCount, 2);
        }
    }
}
