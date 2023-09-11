namespace Skyline.Automation.CiscoNexusUnicastMulticastNat.View
{
    using System;
    using System.Text.RegularExpressions;
    using Skyline.Automation.CiscoNexusUnicastMulticastNat.Utils;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class View : Dialog
    {
        public View(IEngine engine) : base(engine)
        {
            Title = "Unicast to Multicast NAT";
            Width = 1300;
            AllowOverlappingWidgets = true;

            DestinationPhysicalInterface = new DropDown();
            DestinationUnicastIpAddress = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.37.10" };
            SourceInterfaceName = new TextBox { Width = Utils.TextBoxWidth, Text = "loopback3" };
            LoopbackIpAddress = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.37.100/24" };
            RouterOspfId = new TextBox { Width = Utils.TextBoxWidth, Text = "1" };
            RouterOspfArea = new TextBox { Width = Utils.TextBoxWidth, Text = "0.0.0.0" };
            UnicastSourceIpAddress1 = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.23.10" };
            UnicastSourceIpAddress2 = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.28.10" };
            DestinationMulticastIpAddress = new TextBox { Width = Utils.TextBoxWidth, Text = "237.0.10.10" };

            SendCommand = new Button("Send Commands") { Margin = new Margin(10, 10, 10, 10) };
            DeleteCommand = new Button("Delete NAT") { Margin = new Margin(10, 10, 10, 10) };
            ErrorLabel = new Label(String.Empty);
            InitializeView();
        }

        public DropDown DestinationPhysicalInterface { get; set; }

        public TextBox SourceInterfaceName { get; set; }

        public TextBox LoopbackIpAddress { get; set; }

        public TextBox RouterOspfId { get; set; }

        public TextBox RouterOspfArea { get; set; }

        public TextBox UnicastSourceIpAddress1 { get; set; }

        public TextBox UnicastSourceIpAddress2 { get; set; }

        public TextBox DestinationMulticastIpAddress { get; set; }

        public TextBox DestinationUnicastIpAddress { get; set; }

        public Button SendCommand { get; set; }

        public Button DeleteCommand { get; set; }

        public Label ErrorLabel { get; set; }

        public bool ValidateInputs()
        {
            var ipAddressPattern = "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
            if (!Regex.IsMatch(LoopbackIpAddress.Text.Split('/')[0], ipAddressPattern))
            {
                LoopbackIpAddress.ValidationState = UIValidationState.Invalid;
                LoopbackIpAddress.ValidationText = "IP Address invalid";
                return false;
            }

            if (!Regex.IsMatch(UnicastSourceIpAddress1.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(UnicastSourceIpAddress2.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(DestinationMulticastIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(DestinationUnicastIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            return true;
        }

        private void InitializeView()
        {
            AddWidget(new Label("Source Interface Name") { Width = Utils.LabelWidth }, 0, 0);
            AddWidget(SourceInterfaceName, 0, 1);
            AddWidget(new Label("Loopback IP Address") { Width = Utils.LabelWidth }, 0, 2);
            AddWidget(LoopbackIpAddress, 0, 3);
            AddWidget(new Label("Source Unicast Address 1") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(UnicastSourceIpAddress1, RowCount, 1);
            AddWidget(new Label("Source Unicast Address 2") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(UnicastSourceIpAddress2, RowCount, 3);
            AddWidget(new Label("Destination Unicast IP Address") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(DestinationUnicastIpAddress, RowCount, 1);
            AddWidget(new Label("Destination Multicast IP Address") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(DestinationMulticastIpAddress, RowCount, 3);
            AddWidget(new Label("Destination Physical Interface") { Width = Utils.LabelWidth }, RowCount, 4);
            AddWidget(DestinationPhysicalInterface, RowCount, 5);
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
