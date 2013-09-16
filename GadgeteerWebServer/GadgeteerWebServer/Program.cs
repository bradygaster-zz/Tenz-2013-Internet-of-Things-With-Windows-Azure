using AzureMicroHelpers;
using Gadgeteer.Networking;
using Microsoft.SPOT;
using GT = Gadgeteer;

namespace GadgeteerWebServer
{
    public partial class Program
    {
        private WebEvent _greenEvent;
        private WebEvent _redEvent;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            _led.TurnColor(GT.Color.Yellow);

            NetworkConfigurator.SetupNetwork("192.168.100.94");

            StartWebServer();
        }

        void StartWebServer()
        {
            string ip = _ethernet.NetworkSettings.IPAddress;

            WebServer.StartLocalServer(ip, 80);

            _greenEvent = WebServer.SetupWebEvent("green");
            _greenEvent.WebEventReceived += OnGreenEventReceived;

            _redEvent = WebServer.SetupWebEvent("red");
            _redEvent.WebEventReceived += OnRedEventReceived;
        }

        void OnRedEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            _led.TurnRed();
            responder.Respond("Turned the LED Green");
        }

        void OnGreenEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            _led.TurnGreen();
            responder.Respond("Turned the LED Red");
        }
    }
}
