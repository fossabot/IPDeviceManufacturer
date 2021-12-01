using System;
using System.Windows.Forms;
using System.Net;
using Acolyte.Net.Manufacturer;

namespace WindowsForms.Test
{
    public partial class FormDemo : Form
    {
        private IPDeviceManufacturerScanner manufacturerScanner = null;
        public FormDemo()
        {
            InitializeComponent();
        }
        private void FormDemo_Load(object sender, EventArgs e)
        {
            if (manufacturerScanner == null)
            {
                manufacturerScanner = new IPDeviceManufacturerScanner();

                manufacturerScanner.ScanRecieved += ManufacturerScanner_ScanRecieved;
                manufacturerScanner.ScanComplete += ManufacturerScanner_ScanComplete;
            }
        }

        private void ManufacturerScanner_ScanComplete(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    tbIPAddrFrom.Enabled = tbIPAddrTo.Enabled = btnScan.Enabled = true;
                    lblSc.Text = "Scan Complete";
                    MessageBox.Show("Scan Complete", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            else
            {
                tbIPAddrFrom.Enabled = tbIPAddrTo.Enabled = btnScan.Enabled = true;
                lblSc.Text = "Scan Complete";
                MessageBox.Show("Scan Complete", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ManufacturerScanner_ScanRecieved(object sender, ScanRecievedEventArgs args)
        {
            // Use cross thread operation for GUI mode.

            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    DgvIPScanner.Rows.Add(
                        $"{args.IPAddress}",
                        $"{args.MACAddress}",
                        $"{args.HostName}",
                        $"{args.CompanyName}",
                        $"{args.CompanyAddress}",
                        $"{args.CountryCode}",
                        $"{args.CountryName}"
                        );
                }));
            }
            else
            {
                DgvIPScanner.Rows.Add(
                        $"{args.IPAddress}",
                        $"{args.MACAddress}",
                        $"{args.HostName}",
                        $"{args.CompanyName}",
                        $"{args.CompanyAddress}",
                        $"{args.CountryCode}",
                        $"{args.CountryName}"
                        );
            }
        }

        [Obsolete]
        private void btnScan_Click(object sender, EventArgs e)
        {
            //clear data before start (for rows existing).
            DgvIPScanner.Rows.Clear();

            manufacturerScanner.SetIPAddressRange(IPAddress.Parse(tbIPAddrFrom.Text), IPAddress.Parse(tbIPAddrTo.Text));
            manufacturerScanner.StartScan();

            tbIPAddrFrom.Enabled = tbIPAddrTo.Enabled = btnScan.Enabled = false;
            lblSc.Text = "Scaning...";
        }
    }
}
