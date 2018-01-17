using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SAPKantar
{
    public interface kant
    {
        string weight { get;}
    }

    [ComVisible(true), Guid("9CB760E8-417E-4251-B3A6-5A721C85D3CA")]
    public partial class Kantar : UserControl
    {
        private const string COM_PORT = "COM1";
        private const int BAUD_RATE = 9600;
        private const int DATA_BITS = 7;
        private const Parity PARITY = Parity.Even;
        private const StopBits STOP_BITS = StopBits.One;
        private SerialPort port;

        public Kantar()
        {
            InitializeComponent();
            port = new SerialPort(COM_PORT, BAUD_RATE, PARITY, DATA_BITS, STOP_BITS);
            port.ReadTimeout = 10000;
            port.RtsEnable = false;
            setLed(1);
        }

        private void setLed(int ImageIndex)
        {
            picStat.Image = ilMain.Images[ImageIndex];
            picStat.Visible = true;
            Application.DoEvents();
        }

        public string weight
        {
            get
            {
                try
                {
                    // Portu açalým
                    //if (port.IsOpen) port.Close();
                    setLed(3);
                    System.Threading.Thread.Sleep(3000);
                    port.Open();
                    port.DiscardInBuffer();
                    port.DiscardOutBuffer();

                    // P deðerini gönderelim
                    port.Write("P");
                    System.Threading.Thread.Sleep(3000);

                    // Aðýrlýðý alalým
                    Byte[] buf = new Byte[4096];
                    int len = port.Read(buf, 0, 4096);
                    port.Close();

                    if (len > 0)
                    {
                        setLed(2);
                        string x = System.Text.Encoding.ASCII.GetString(buf, 0, len);
                        return formatWeight(x);
                    }
                    else
                    {
                        setLed(0);
                        return "ERROR";
                    }
                }
                catch (Exception ex)
                {
                    setLed(0);
                    MessageBox.Show(ex.ToString());
                    return "ERROR";
                }
            }
        }

        private string formatWeight(string Raw)
        {
            string ret = "";
            bool append = false;

            // $$$   91,5 KG $$$$ -> 91,5 KG
            for (int n = 0; n < Raw.Length; n++)
            {
                string c = Raw.Substring(n, 1);
                if (c == "0" ||
                    c == "1" ||
                    c == "2" ||
                    c == "3" ||
                    c == "4" ||
                    c == "5" ||
                    c == "6" ||
                    c == "7" ||
                    c == "8" ||
                    c == "9") append = true;

                if (append) ret += c;

                if (c == "g" || c == "G")
                {
                    append = false;
                    return ret;
                }

            }

            return ret;
        }

    }
}
