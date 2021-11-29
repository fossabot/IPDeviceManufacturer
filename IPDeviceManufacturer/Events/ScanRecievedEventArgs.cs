using Bia.Countries.Iso3166;
public class ScanRecievedEventArgs
{
    public System.Net.IPAddress IPAddress { get; set; }
    public string MACAddress { get; set; }
    public string HostName { get; set; }
    public string CompanyName { get; set; }
    public string CompanyAddress { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public ScanRecievedEventArgs(System.Net.IPAddress ipAddress,
        string MacAddress,
        string hostName,
        string companyName,
        string companyAddress,
        string countryCode)
    {
        this.IPAddress = ipAddress;
        this.MACAddress = MacAddress.Trim();
        this.HostName = hostName;
        this.CompanyName = companyName.Trim();
        this.CompanyAddress = companyAddress.Trim();
        this.CountryCode = countryCode.Trim();
        this.CountryName = Countries.GetCountryByAlpha2(CountryCode).ShortName;
    }
}