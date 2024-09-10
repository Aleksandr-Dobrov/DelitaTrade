using System.Net.NetworkInformation;

namespace DelitaTrade.Models.DataProviders
{
    public class InternetProvider
    {
        
        public InternetProvider()
        {
            NetworkStatusChange += () => { };
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(AvailabilityChangeCallback);
        }
        public bool CheckForInternetConnection()
        {
            try
            {
                Ping myPing = new Ping();
                string host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions options = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, options);
                return (reply.Status == IPStatus.Success);
            }
            catch
            {
                return false;
            }
        }

        public event Action NetworkStatusChange;

        private void AvailabilityChangeCallback(object? sender, NetworkAvailabilityEventArgs e)
        {              
            NetworkStatusChange.Invoke();
        }
    }
}
