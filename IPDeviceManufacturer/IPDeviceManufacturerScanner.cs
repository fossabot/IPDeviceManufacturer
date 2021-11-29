using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Acolyte.Net.Manufacturer
{
    public class IPDeviceManufacturerScanner
    {
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);
        /// <summary>
        /// Set IP Address range by from.
        /// </summary>
        private IPAddress IPAddressFrom { get; set; }

        /// <summary>
        /// Set IP Address range by to.
        /// </summary>
        private IPAddress IPAddressTo { get; set; }

        /// <summary>
        /// Check Mac Address if is null.
        /// </summary>
        private readonly string EmptyMacAddress = "00-00-00-00-00-00";

        /// <summary>
        /// Max Postfix range ip address.
        /// </summary>
        private readonly Int32 MAX_RANGE_IP = 255;

        /// <summary>
        /// Get Current Device Path Database from assembly.
        /// </summary>
        private string DevicePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dist", "Devices.db");

        /// <summary>
        /// Create first instance class.
        /// </summary>
        /// <param name="ipAddressFrom"></param>
        /// <param name="ipAddressTo"></param>
        public IPDeviceManufacturerScanner(IPAddress ipAddressFrom, IPAddress ipAddressTo)
        {
            this.IPAddressFrom = ipAddressFrom;
            this.IPAddressTo = ipAddressTo;
        }

        /// <summary>
        /// Start Scan IP Address
        /// </summary>
        /// <exception cref="IPAddressOutOfRangeException"></exception>
        /// <exception cref="IPAddressInvalidRangeException"></exception>
        [Obsolete]
        public void StartScan()
        {
            var _ipAddrFrom = IPAddressFrom.ToString().Split('.');
            var _ipAddrTo = IPAddressTo.ToString().Split('.');
            var _ipAddrPrefixFrom = $"{_ipAddrFrom[0]}.{_ipAddrFrom[1]}.{_ipAddrFrom[2]}";
            var _ipAddrPrefixTo = $"{_ipAddrTo[0]}.{_ipAddrTo[1]}.{_ipAddrTo[2]}";
            var _ipAddrNumFrom = Convert.ToInt32(this.IPAddressFrom.ToString().Split('.')[3]);
            var _ipAddrNumTo = Convert.ToInt32(this.IPAddressTo.ToString().Split('.')[3]);

            if (_ipAddrNumTo > MAX_RANGE_IP)
                throw new IPAddressOutOfRangeException("IP Address postfix range must between 1 - 255.");

            if (_ipAddrPrefixFrom != _ipAddrPrefixTo)
                throw new IPAddressInvalidRangeException("IP Address range between From and To are not equal.");

            using (var db = new LiteDB.LiteDatabase(DevicePath))
            {
                var MacAddressDB = db.GetCollection<MacAddressCollection>("MacAddress");

                Parallel.For(_ipAddrNumFrom, _ipAddrNumTo, p =>
                {
                    try
                    {
                        var IpAddress = $"{_ipAddrPrefixFrom}.{p}";
                        IPAddress hostIPAddress = IPAddress.Parse(IpAddress);
                        byte[] ab = new byte[6];
                        int len = ab.Length, r = SendARP((int)hostIPAddress.Address, 0, ab, ref len);
                        var macIP = BitConverter.ToString(ab);

                        if (macIP != EmptyMacAddress)
                        {
                            var macStr = BitConverter.ToString(ab).Split('-');

                            var MacAddressPattern1 = $"{macStr[0]}:{macStr[1]}:{macStr[2]}";
                            var MacAddressPattern2 = $"{macStr[0]}:{macStr[1]}:{macStr[2]}:{macStr[3]}";
                            var MacAddressPattern3 = $"{macStr[0]}:{macStr[1]}:{macStr[2]}:{macStr[3]}:{macStr[4].Remove(macStr[4].Length - 1)}";

                            var mac = MacAddressDB.FindAll().Where(x => x.oui == MacAddressPattern1
                            || x.oui == MacAddressPattern2
                            || x.oui == MacAddressPattern3).FirstOrDefault();

                            OnRaiseScanReceived(new ScanRecievedEventArgs(IPAddress.Parse(IpAddress), macIP, mac.companyName, mac.companyAddress, mac.countryCode));
                        }
                    }
                    catch (Exception) { }
                });

                if (ScanComplete != null)
                    ScanComplete(this, null);
            }
        }

        /// <summary>
        /// Get host name from IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                if (entry != null)
                {
                    return entry.HostName;
                }
            }
            catch (System.Net.Sockets.SocketException) { }

            return null;
        }

        #region Events

        public delegate void ScanRecievedEventHandler(object sender, ScanRecievedEventArgs args);
        public event ScanRecievedEventHandler ScanRecieved;
        public event EventHandler ScanComplete;

        protected virtual void OnRaiseScanReceived(ScanRecievedEventArgs args)
        {
            if (ScanRecieved != null)
                ScanRecieved(this, args);
        }

        #endregion
    }
}
