using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using AzureMicroHelpers;

namespace GadgeteerCallingWebApi
{
    public partial class Program
    {
        string _url = "http://TemperatureAndHumidityApi.azurewebsites.net/api/sensor/";
        GT.Timer _timer;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            // UX to show in a waiting state
            _led.TurnColor(GT.Color.Yellow);

            // set up the timer
            _timer = new GT.Timer(1000);
            _timer.Tick += OnTimerTick;

            // set up the network
            if (!_ethernet.Interface.IsOpen)
                _ethernet.Interface.Open();

            // set up the network per the conference specifications
            NetworkConfigurator.SetupNetwork("192.168.100.93");
            
            // handle measurements obtained
            _temp.MeasurementComplete += OnMeausurementComplete;

            // start the timer
            _timer.Start();

            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");
        }

        void OnMeausurementComplete(GTM.Seeed.TemperatureHumidity sender,
            double temperature,
            double relativeHumidity)
        {
            Debug.Print("Temperature " + temperature + ", Humidity " + relativeHumidity);

            // read the temp and huidity
            var t = temperature.ToString().Substring(0, temperature.ToString().IndexOf('.'));
            var h = relativeHumidity.ToString().Substring(0, relativeHumidity.ToString().IndexOf('.'));

            // build the API URL
            var url = _url + t + "/" + h;
            Debug.Print("Calling API at " + url);

            _timer.Stop();

            _led.TurnColor(GT.Color.Yellow);

            // create a Web Request we'll call in a moment
            var request = WebClient.GetFromWeb(url);

            // whenever the request responds, handle it
            request.ResponseReceived += OnResponseReceived;

            // call the API
            request.SendRequest();
        }

        /// <summary>
        /// Fires when the Web Request obtains a response from the server
        /// </summary>
        void OnResponseReceived(HttpRequest sender, HttpResponse response)
        {
            _led.TurnColor(GT.Color.Green);
            Debug.Print("Response: " + response.Text);
            _timer.Start();
        }

        /// <summary>
        /// Fires whenever the timer ticks, 
        /// during which we'll get the sensor reading.
        /// </summary>
        void OnTimerTick(GT.Timer timer)
        {
            Debug.Print("Getting Measurements");

            // get a measurement
            _temp.RequestMeasurement();
        }
    }
}
