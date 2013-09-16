using System;
using Microsoft.SPOT;
using AzureMicroHelpers.Table;
using Microsoft.SPOT.Net.NetworkInformation;
using Microsoft.SPOT.Hardware;
using AzureMicroHelpers.NTP;

namespace TemperatureRecording
{
    public class ScenarioParameters
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
    }

    public class Scenario
    {
        private TableClient _tableClient;
        private ScenarioParameters _parameters;

        public Scenario(ScenarioParameters parameters)
        {
            _parameters = parameters;
         
            CreateTable();
        }

        private void CreateTable()
        {
            _tableClient = new TableClient(_parameters.AccountName, _parameters.AccountKey);
            _tableClient.CreateTable("netmfdata");
            Debug.Print("Table exists or was created successfully");
        }

        public void SaveTemperature(int temperature, string country, string deviceIdentifier)
        {
            _tableClient.AddTableEntityForTemperature("netmfdata",
                    deviceIdentifier,
                    (DateTime.Now.Ticks).ToString(),
                    DateTime.Now,
                    temperature,
                    country);
        }
    }
}
