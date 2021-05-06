using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogeCoinPriceAlert
{
    public partial class Form1 : Form
    {
        private const int Stunde = 1000 * 60 * 60;

        private static Uri ApiUrl { get; set; } = new Uri("https://sochain.com//api/v2/get_price/DOGE/USD");
        private static string PriceFile { get; set; }  = ".\\Preis.prs";
        private static bool CancelToken { get; set; } = false;
        private static double Price { get; set; } = 0.00;
        public Form1()
        {
            InitializeComponent();

            if (System.IO.File.Exists(PriceFile))
                Price = Convert.ToDouble(System.IO.File.ReadAllText(PriceFile));

            _ = ComparePrice();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CancelTask();
            notifyIcon1.Dispose();
            Close();
        }

        private void CancelTask()
        {
            CancelToken = true;
            System.IO.File.WriteAllText(PriceFile, Price.ToString());

        }

        private static async Task ComparePrice()
        {            
            while (!CancelToken)
            {
                try
                {
                    DogeCoinObj dogeCoin = await GetDogeCoinData();
                    if (dogeCoin == null)
                    { 
                        throw new Exception("DOGECOIN KONNTE NICHT ABGERUFEN WERDEN");

                    }

                    double currentPrice = dogeCoin.data.prices[0].price;
                    if (currentPrice < Price)
                    {
                        new ToastContentBuilder()
                            .AddArgument("action", "viewConversation")
                            .AddText("DogeCoin Price Alert")
                            .AddText($"Preis liegt bei {currentPrice}, vorheriger Preis {Price}")
                            .Show();  
                    }
                    Price = currentPrice;
                    await Task.Delay(Stunde);
                }
                catch (Exception ex)
                {
                    new ToastContentBuilder()
                        .AddArgument("action", "viewConversation")
                        .AddText("FEHLER")
                        .AddText(ex.Message)
                        .Show(); // 
                } 
            }

        }


        private static async Task<DogeCoinObj> GetDogeCoinData()
        {
            DogeCoinObj dogeCoin = null;
            await Task.Run(() =>
            {
                string Json = GetJson();
                dogeCoin = Newtonsoft.Json.JsonConvert.DeserializeObject<DogeCoinObj>(Json);
            });
            return dogeCoin;
        }


        private static string GetJson()
        {
            WebClient client = new WebClient();
            return client.DownloadString(ApiUrl);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            notifyIcon1.Dispose();
            CancelTask();
        }
    }
}
