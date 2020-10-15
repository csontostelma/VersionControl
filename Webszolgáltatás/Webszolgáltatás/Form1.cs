using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Webszolgáltatás.Entities;
using Webszolgáltatás.MnbServiceReference;

namespace Webszolgáltatás
{
    public partial class Form1 : Form
    {
        BindingList<RateDate> Rates;

        public Form1()
        {
            InitializeComponent();

            GetExchangeRates();

            dataGridView1.DataSource = Rates;
        }

        private void GetExchangeRates()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate= "2020-01-01",
                endDate="2020-06-30",

            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;



            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateDate();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("curr");

                var unit=decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit!=0)
                {
                    rate.Value = value / unit;
                }




            }
        }
    }
}
