using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading.Tasks;

namespace Webszolgáltatás.Entities
{
    class RateDate
    {
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public decimal Value { get; set; }

    }
}
