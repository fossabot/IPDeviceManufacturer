using System;
using System.Net;
using Acolyte.Net.Manufacturer;

namespace WindowsConsole.Test
{
    internal class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            //-------------------------------------------------------------------------------------

            ///!!! Warning : Before run it please set your range ip address from network adapter.

            //-------------------------------------------------------------------------------------

            IPDeviceManufacturerScanner manufacturerScanner = new IPDeviceManufacturerScanner(IPAddress.Parse("192.168.43.1"), IPAddress.Parse("192.168.43.255"));

            manufacturerScanner.ScanRecieved += ManufacturerScanner_ScanRecieved;
            manufacturerScanner.ScanComplete += ManufacturerScanner_ScanComplete;
            manufacturerScanner.StartScan();
        }

        static void ManufacturerScanner_ScanRecieved(object sender, ScanRecievedEventArgs args)
        {
            Console.WriteLine(
                $"IP Address : {args.IPAddress.ToString()} \n" +
                $"MAC Address : {args.MACAddress} \n" +
                $"Host Name : {args.HostName}\n" +
                $"Network Adapter Company : {args.CompanyName} \n" +
                $"Company Address : {args.CompanyAddress} \n" +
                $"Country Code : {args.CountryCode}\n" +
                $"Country Name : {args.CountryName}\n");

            Console.WriteLine("-----------------------------------------------------------");
        }

        static void ManufacturerScanner_ScanComplete(object sender, EventArgs e)
        {
            Console.WriteLine("Complete..!!");
            Console.ReadKey();
        }
    }
}
