using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.DataModel
{
    public class Policy
    {
        public string ID { get; set; }

        public string Title { get; set; }

        public DateTime Updated { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public string modDate { get; set; }
    }
    public class PoliciesSorted
    {
        public string Type { get; set; }
        public List<Policy> Policies { get; set; }

    }


    public class PoliciesDataSource
    {

        public List<PoliciesSorted> Policies { get; set; }

        public PoliciesDataSource(string JSON)
        {

            List<Policy> policies = JsonConvert.DeserializeObject<List<Policy>>(JSON);

            var modDate = policies[0].Updated.ToString("ddd d MMM yyy");

            policies[0].modDate = "Published: " + modDate;

            var policiesByType = policies.GroupBy(x => x.Type)
                                .Select(x => new PoliciesSorted { Type = x.Key, Policies = x.ToList() });

            Policies = policiesByType.ToList();


        }
    }
}
