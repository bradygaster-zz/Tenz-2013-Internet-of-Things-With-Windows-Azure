using AzureMicroHelpers;
using AzureMicroHelpers.Table;
using System.Threading;
using TemperatureRecording;

namespace AzureMicroClients.NetduinoPlus
{
    public class Program
    {
        private const string AccountName = "techednzstorage";
        private const string AccountKey = "SL4SUQIQ42KFl7IgonzIjvG0T2Nd59WaladAdHnIJ00fWkl1XcgIX3wrYbCay64lvMszJXdKmpUjob/6V4k63A==";

        public static void Main()
        {
            NetworkConfigurator.SetupNetwork("192.168.100.94");

            var scenario = new Scenario(
                new ScenarioParameters
                {
                    AccountKey = AccountKey,
                    AccountName = AccountName
                });

            while (true)
            {
                Thread.Sleep(2000);

                scenario.SaveTemperature(42, "NZ", "Netduino");
            }
        }
    }
}
