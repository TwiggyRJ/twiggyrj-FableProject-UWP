using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.DataModel
{
    public class Updates
    {

        public string ID { get; set; }

        public string Version { get; set; }

        public string Content { get; set; }

        public string Content_2 { get; set; }

        public string Content_3 { get; set; }

        public string Content_4 { get; set; }

        public string Content_5 { get; set; }

        public string Content_6 { get; set; }

        public string About { get; set; }

        public DateTime Date { get; set; }

        public string modDate { get; set; }

    }

    public class UpdatesSorted
    {
        public string Version { get; set; }
        public List<Updates> Updates { get; set; }

    }


    public class UpdatesDataSource
    {

        public List<UpdatesSorted> Updates { get; set; }

        public UpdatesDataSource(string JSON)
        {

            List<Updates> updates = JsonConvert.DeserializeObject<List<Updates>>(JSON);

            string bullet = "• ";

            updates[0].modDate = updates[0].Date.ToString("ddd d MMM yyy");
            updates[0].Content = bullet + updates[0].Content;
            updates[0].Content_2 = bullet + updates[0].Content_2;
            updates[0].Content_3 = bullet + updates[0].Content_3;
            updates[0].Content_4 = bullet + updates[0].Content_4;
            updates[0].Content_5 = bullet + updates[0].Content_5;
            updates[0].Content_6 = bullet + updates[0].Content_6;

            var updatesByVersion = updates.GroupBy(x => x.Version)
                                .Select(x => new UpdatesSorted { Version = x.Key, Updates = x.ToList() });

            Updates = updatesByVersion.ToList();


        }
    }
}
