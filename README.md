## Sharp-IP  Vendors Device Manufacturer
C# Find the vendor / manufacturer of a device description  by IP/MAC Address from network adapter.

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/KravitzMC/IPDeviceManufacturer/blob/main/LICENSE)

## Nuget

[![Nuget](https://img.shields.io/nuget/dt/IPDeviceManufacturer?style=for-the-badge)](https://www.nuget.org/packages/IPDeviceManufacturer) 

##  Why must be this library ?

✔️ Get more deivce information from IP/MAC by general can't do it.

✔️ Vendors / Manufacturer from the IEEE Standards Association have over than 40000+ devices .

✔️ You can scan range IP address by without pinging cause some client not response anytime. 
    but we use Send ARP instead.

## Help or Feedback

Need help or have feedback?  Please file an issue here!

## Usage

```csharp
using System;
using System.Net;
using Acolyte.Net.Manufacturer;

[Obsolete]
void Main(string[] args)
{
    //Set IP Address range from and to.
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

```
## Example Output
![Output](https://github.com/KravitzMC/IPDeviceManufacturer/blob/main/outputexample.png)


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.
