using FableProject.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.DataModel
{
    public class User
    {
        public string ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime DOB { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public DateTime Joined { get; set; }

        public string Website { get; set; }

        public string Author { get; set; }

        public string Admin { get; set; }

        public string Stories { get; set; }

        public string modAccountType { get; set; }

        public string modJoined { get; set; }

        public string modDOB { get; set; }

        public int modAge { get; set; }

    }

    public class UserSorted
    {
        public string Name { get; set; }
        public List<User> Users { get; set; }

    }

    public class UserDataSource
    {

        public List<UserSorted> Users { get; set; }
        public List<StoriesSorted> Stories { get; set; }

        public UserDataSource(string JSON, string username, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");

            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");
            users[0].modDOB = users[0].DOB.ToString("d MMMM");

            DateTime today = DateTime.Today;
            int age = today.Year - users[0].DOB.Year;
            if (users[0].DOB > today.AddYears(-age)) age--;

            users[0].modAge = age;

            if (users[0].Author == "1")
            {
                users[0].modAccountType = "Author";
            }
            else
            {
                users[0].modAccountType = "Reader";
            }

            if (users[0].Admin == "1")
            {
                users[0].modAccountType = "Admin";
            }

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string aDataKey = "authorDetails";
            string avDataKey = "avatarDetails";
            string idDataKey = "userIdDetails";
            string adDatakey = "adminDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, JSON);
                storage.SaveRoamingSettings(uDataKey, username);
                storage.SaveRoamingSettings(pDataKey, password);
                storage.SaveRoamingSettings(avDataKey, users[0].Avatar);
                storage.SaveRoamingSettings(aDataKey, users[0].Author);
                storage.SaveRoamingSettings(idDataKey, users[0].ID);
                storage.SaveRoamingSettings(adDatakey, users[0].Admin);
            }
            else if (roamingSetting == "false")
            {
                storage.SaveSettings(sDataKey, JSON);
                storage.SaveSettings(uDataKey, username);
                storage.SaveSettings(pDataKey, password);
                storage.SaveSettings(avDataKey, users[0].Avatar);
                storage.SaveSettings(aDataKey, users[0].Author);
                storage.SaveSettings(idDataKey, users[0].ID);
                storage.SaveSettings(adDatakey, users[0].Admin);
            }
            else if (roamingSetting == "Null")
            {
                storage.SaveSettings(sDataKey, JSON);
                storage.SaveSettings(uDataKey, username);
                storage.SaveSettings(pDataKey, password);
                storage.SaveSettings(avDataKey, users[0].Avatar);
                storage.SaveSettings(aDataKey, users[0].Author);
                storage.SaveSettings(idDataKey, users[0].ID);
                storage.SaveSettings(adDatakey, users[0].Admin);
            }

        }

        public UserDataSource(string JSON, string JSONStories, string username, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");

            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");
            users[0].modDOB = users[0].DOB.ToString("d MMMM");

            DateTime today = DateTime.Today;
            int age = today.Year - users[0].DOB.Year;
            if (users[0].DOB > today.AddYears(-age)) age--;

            users[0].modAge = age;

            if (users[0].Author == "1")
            {
                users[0].modAccountType = "Author";
            }
            else
            {
                users[0].modAccountType = "Reader";
            }

            if (users[0].Admin == "1")
            {
                users[0].modAccountType = "Admin";
            }

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();



            List<Stories> stories = JsonConvert.DeserializeObject<List<Stories>>(JSONStories);

            var storiesByTitle = stories.GroupBy(x => x.Title)
                                .Select(x => new StoriesSorted { Title = x.Key, Stories = x.ToList() });

            Stories = storiesByTitle.ToList();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string aDataKey = "authorDetails";
            string avDataKey = "avatarDetails";
            string idDataKey = "userIdDetails";
            string adDatakey = "adminDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, JSON);
                storage.SaveRoamingSettings(uDataKey, username);
                storage.SaveRoamingSettings(pDataKey, password);
                storage.SaveRoamingSettings(avDataKey, users[0].Avatar);
                storage.SaveRoamingSettings(aDataKey, users[0].Author);
                storage.SaveRoamingSettings(idDataKey, users[0].ID);
                storage.SaveRoamingSettings(adDatakey, users[0].Admin);
            }
            else if (roamingSetting == "false")
            {
                storage.SaveSettings(sDataKey, JSON);
                storage.SaveSettings(uDataKey, username);
                storage.SaveSettings(pDataKey, password);
                storage.SaveSettings(avDataKey, users[0].Avatar);
                storage.SaveSettings(aDataKey, users[0].Author);
                storage.SaveSettings(idDataKey, users[0].ID);
                storage.SaveSettings(adDatakey, users[0].Admin);
            }
            else if (roamingSetting == "Null")
            {
                storage.SaveSettings(sDataKey, JSON);
                storage.SaveSettings(uDataKey, username);
                storage.SaveSettings(pDataKey, password);
                storage.SaveSettings(avDataKey, users[0].Avatar);
                storage.SaveSettings(aDataKey, users[0].Author);
                storage.SaveSettings(idDataKey, users[0].ID);
                storage.SaveSettings(adDatakey, users[0].Admin);
            }

        }

    }
}
