using AzureMicroHelpers.NTP;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Net.NetworkInformation;

namespace AzureMicroHelpers
{
    /// <summary>
    /// This class was used on stage at TechEd New Zealand. 
    /// 
    /// It provides network settings specific to the conference, as provided by the 
    /// awesome & super-wise networking gods who supported the event.
    /// </summary>
    public static class NetworkConfigurator
    {
        public static void SetupNetwork(string ipAddress)
        {
            NetworkInterface[] _netIF = NetworkInterface.GetAllNetworkInterfaces();

            _netIF[0].EnableStaticIP(ipAddress, "255.255.254.0", "192.168.101.254");
            _netIF[0].EnableStaticDns(new string[] { "8.8.8.8" });

            var ip = _netIF[0].IPAddress;

            // set the network time
            var networkTime = NtpClient.GetNetworkTime();
            Utility.SetLocalTime(networkTime);
        }
    }
}
