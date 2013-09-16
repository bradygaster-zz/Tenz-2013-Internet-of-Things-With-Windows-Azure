using AzureMicroHelpers;
using Microsoft.SPOT;
using TemperatureRecording;
using GT = Gadgeteer;

namespace AzureMicroClients.GadgeteerSpider
{
    public partial class Program
    {
        private const string AccountName = "techednzstorage";
        private const string AccountKey = "SL4SUQIQ42KFl7IgonzIjvG0T2Nd59WaladAdHnIJ00fWkl1XcgIX3wrYbCay64lvMszJXdKmpUjob/6V4k63A==";
        GT.Timer _timer;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            NetworkConfigurator.SetupNetwork("192.168.100.93");

            var scenario = new Scenario(
                new ScenarioParameters
                {
                    AccountKey = AccountKey,
                    AccountName = AccountName
                });

            _timer = new GT.Timer(5000);

            _timer.Tick += (t) =>
            {
                _timer.Stop();

                scenario.SaveTemperature(42, "NZ", "Gadgeteer");

                _timer.Start();
            };
            _timer.Start();
        }
    }
}
