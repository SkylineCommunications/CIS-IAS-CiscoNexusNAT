namespace Skyline.Automation.CiscoNexusMulticastUnicastNat.View
{
    using System;
    using System.Text.RegularExpressions;
    using Skyline.Automation.CiscoNexusMulticastUnicastNat.Utils;
    using Skyline.DataMiner.Automation;
    using Skyline.DataMiner.Utils.InteractiveAutomationScript;

    public class View : Dialog
    {
        public View(IEngine engine) : base(engine)
        {
            Title = "Multcast To Unicast NAT";
            Width = 1500;
            AllowOverlappingWidgets = true;

            DestinationPhysicalInterface = new DropDown();
            DestinationUnicastIpAddress1 = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.37.10" };
            DestinationUnicastIpAddress2 = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.0.10" };
            SourceInterfaceName = new TextBox { Width = Utils.TextBoxWidth, Text = "loopback3" };
            LoopbackIpAddress = new TextBox { Width = Utils.TextBoxWidth , Text = "192.168.37.100/24" };
            RouterOspfId = new TextBox{ Width = Utils.TextBoxWidth , Text="1"};
            RouterOspfArea = new TextBox{ Width = Utils.TextBoxWidth, Text="0.0.0.0" };
            SourceMulticastIpAddress = new TextBox{ Width = Utils.TextBoxWidth, Text= "239.0.0.10" };
            SourceUnicastIpAddress = new TextBox { Width = Utils.TextBoxWidth, Text = "192.168.23.10" };
            SourceIpAddress = new TextBox{ Width = Utils.TextBoxWidth, Text= "192.168.23.10" };

            SendCommand = new Button("Create NAT") { Margin = new Margin(10, 10, 10, 10) };
            DeleteCommand = new Button("Delete NAT") { Margin = new Margin(10, 10, 10, 10) };
            ErrorLabel = new Label(String.Empty);
            InitializeView();
        }

        public DropDown DestinationPhysicalInterface { get; set; }

        public TextBox SourceInterfaceName { get; set; }

        public TextBox LoopbackIpAddress { get; set; }

        public TextBox RouterOspfId { get; set; }

        public TextBox RouterOspfArea { get; set; }

        public TextBox SourceMulticastIpAddress { get; set; }

        public TextBox SourceIpAddress { get; set; }

        public TextBox SourceUnicastIpAddress { get; set; }

        public TextBox DestinationUnicastIpAddress1{ get; set; }
       
        public TextBox DestinationUnicastIpAddress2 { get; set; }

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

            if (!Regex.IsMatch(SourceMulticastIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(SourceIpAddress.Text, ipAddressPattern))
            {
                return false;
            }

            if (!Regex.IsMatch(DestinationUnicastIpAddress1.Text, ipAddressPattern))
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
            AddWidget(new Label("Source Multicast IP Address") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(SourceMulticastIpAddress, RowCount, 1);
            AddWidget(new Label("Source Unicast IP Address") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(SourceUnicastIpAddress, RowCount, 3);
            AddWidget(new Label("Destination Unicast IP Address 1") { Width = Utils.LabelWidth }, RowCount + 1, 0);
            AddWidget(DestinationUnicastIpAddress1, RowCount, 1);
            AddWidget(new Label("Destination Unicast IP Address 2") { Width = Utils.LabelWidth }, RowCount, 2);
            AddWidget(DestinationUnicastIpAddress2, RowCount, 3);
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
