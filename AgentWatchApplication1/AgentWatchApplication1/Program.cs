using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using System.Net;
using System.IO;
using System.Text;

namespace AgentWatchApplication1
{
    public class Program
    {
        static Bitmap _display;
        static Font fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);
        
        public static void Main()
        {
            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // sample "hello world" code
            DisplayMessage("Hello World");

            var button = new InterruptPort(HardwareProvider.HwProvider.GetButtonPins(Button.VK_SELECT),
                false,
                Port.ResistorMode.PullDown,
                Port.InterruptMode.InterruptEdgeBoth);
            button.OnInterrupt += OnButtonPressed;

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        private static void DisplayMessage(string message)
        {
            _display.Clear();
            _display.DrawText(message, fontNinaB, Color.White, 10, 10);
            _display.Flush();
        }

        static void OnButtonPressed(uint data1, uint status, DateTime time)
        {
            if (status == 1)
            {
                DisplayMessage("Getting Server Time");

                byte[] data;

                using (var request = WebRequest.Create("http://AGENTWatchBackEnd.azurewebsites.net/api/servertime"))
                {
                    var response = request.GetResponse();
                    var stm = response.GetResponseStream();
                    data = new byte[stm.Length];
                    stm.Read(data, 0, data.Length);
                }

                using (var ms = new MemoryStream(data))
                {
                    ms.Position = 0;
                    var output = new StreamReader(ms).ReadToEnd();
                    DisplayMessage(output);
                }
            }
        }
    }
}
