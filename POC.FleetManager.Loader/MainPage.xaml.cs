using POC.FleetManager.Common;

namespace POC.FleetManager.Loader
{
    public partial class MainPage : ContentPage
    {
        int counter = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            counter++;

            if (counter == 1)
                CounterBtn.Text = $"Clicked {counter} time";
            else
                CounterBtn.Text = $"Clicked {counter} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
