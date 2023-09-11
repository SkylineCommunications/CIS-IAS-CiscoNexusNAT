namespace Skyline.Automation.CiscoNexusMulticastUnicastNat.Presenter
{
	using System;
	using Skyline.Automation.CiscoNexusMulticastUnicastNat.Model;
	using Skyline.Automation.CiscoNexusMulticastUnicastNat.View;

    public class Presenter
    {
        private readonly View view;
        private readonly Model model;

        public Presenter(View view, Model model)
        {
            this.view = view ?? throw new ArgumentNullException("view");
            this.model = model ?? throw new ArgumentNullException("model");

            view.SendCommand.Pressed += OnSendCommandsPressed;
            view.DeleteCommand.Pressed += OnDeleteCommandPressed;
        }

        public void LoadFromModel()
        {
            view.DestinationPhysicalInterface.Options = model.PhysicalInterfaces;
        }

        private void OnSendCommandsPressed(object sender, EventArgs e)
        {
            if(!view.ValidateInputs())
            {
                view.ErrorLabel.Text = "Couldn't Send Command.";
                return;
            }

            model.PushConfigs(view);

            view.ErrorLabel.Text = "Command Sent";
        }

        private void OnDeleteCommandPressed(object sender, EventArgs e)
        {
            if (!view.ValidateInputs())
            {
                view.ErrorLabel.Text = "Couldn't Send Command.";
                return;
            }

            model.RemoveConfigs(view);

            view.ErrorLabel.Text = "Command Sent";
        }
    }
}