## Sharp-IP  Vendors Device Manufacturer
C# Find the vendor / manufacturer of a device description  by IP/MAC Address from you network adapter.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)

## Usage

```csharp
using System;
using System.Net;
using Acolyte.Net.Manufacturer;

[Obsolete]
void Main(string[] args)
{
    //Set an ip range from and to.
    IPDeviceManufacturerScanner manufacturerScanner = new IPDeviceManufacturerScanner(IPAddress.Parse("192.168.10.1"), IPAddress.Parse("192.168.10.255"));    
            manufacturerScanner.ScanRecieved += ManufacturerScanner_ScanRecieved;
            manufacturerScanner.ScanComplete += ManufacturerScanner_ScanComplete;
            manufacturerScanner.StartScan();
}

static void ManufacturerScanner_ScanRecieved(object sender, ScanRecievedEventArgs args)
{
	Console.WriteLine(
	$"IP Address : {args.IPAddress.ToString()} \n" +
	$"MAC Address : {args.MACAddress} \n" +
	$"Network Adapter Company : {args.CompanyName} \n" +
	$"Company Address : {args.CompanyAddress} \n" +
	$"Country Code : {args.CountryCode}\n" +
	$"Country Name : {args.CountryName}\n");
}

static void ManufacturerScanner_ScanComplete(object sender, EventArgs e)
{
	Console.WriteLine("Complete..!!");
	Console.ReadKey();
}

```
