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

namespace ImageCapture.MonitoringDevice
{
    public partial class Program
    {
        GT.Timer _timer;
        string _url = "http://imagecapture.azurewebsites.net/capture";

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            // set up an event handler for when the camera snaps a shot
            _camera.PictureCaptured += OnPictureCaptured;

            // set up the network
            if (!_ethernet.Interface.IsOpen)
                _ethernet.Interface.Open();

            // configure the network with the conference settings
            NetworkConfigurator.SetupNetwork("192.168.100.93");

            // start the timer
            _timer = new GT.Timer(5000);
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }

        /// <summary>
        /// When the timer ticks, take a picture. 
        /// </summary>
        void OnTimerTick(GT.Timer timer)
        {
            _timer.Stop();

            Debug.Print("Taking Picture");

            _camera.TakePicture();
        }

        /// <summary>
        /// Once the camera has finished taking a picture, 
        /// upload it TO THE CLOUD. (I couldn't resist)
        /// </summary>
        void OnPictureCaptured(GTM.GHIElectronics.Camera sender, 
            GT.Picture picture)
        {
            Debug.Print("Picture Taken");

            var data = picture.PictureData;

            // we want to UPLOAD DATA so make a Post Request with the byte array
            var content = POSTContent.CreateBinaryBasedContent(data);
            var request = HttpHelper.CreateHttpPostRequest(_url, content, "image/jpg");
            request.AddHeaderField("filename", DateTime.Now.Ticks.ToString() + ".jpg");
            request.ResponseReceived += OnResponseReceived;

            // send the request
            request.SendRequest();
        }

        /// <summary>
        /// This runs whenever the response is obtained from the cloud. 
        /// </summary>
        void OnResponseReceived(HttpRequest sender, HttpResponse response)
        {
            Debug.Print("Response received. Status Code: " + response.StatusCode + ", Response: " + response.Text);

            _timer.Start();
        }
    }
}
