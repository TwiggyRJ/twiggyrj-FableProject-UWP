using FableProject.Data;
using FableProject.Functions;
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

        public string modAgeDisplay { get; set; }

        public string modStories { get; set; }

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

        public UserDataSource(string JSON)
        {

            string rDatakey = "roamingDetails";
            string dfDatakey = "dateFormatDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string dateSetting = "";

            if (roamingSetting == "true")
            {
                dateSetting = storage.LoadRoamingSettings(dfDatakey);
            }
            else
            {
                dateSetting = storage.LoadSettings(dfDatakey);
            }

            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");

            if (dateSetting == "0")
            {
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "1")
            {
                users[0].modJoined = users[0].Joined.ToString("dddd d MMMM yyyy");
            }
            else if (dateSetting == "2")
            {
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "3")
            {
                users[0].modJoined = users[0].Joined.ToString("dd/MM/yyyy");
            }
            else if (dateSetting == "4")
            {
                users[0].modJoined = users[0].Joined.ToString("M/d/yyyy");
            }
            else
            {
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();

        }

        public UserDataSource(string JSON, string username, string password)
        {

            string sDataKey = "userDetails";
            string nDataKey = "nameDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string aDataKey = "authorDetails";
            string avDataKey = "avatarDetails";
            string idDataKey = "userIdDetails";
            string adDatakey = "adminDetails";
            string dfDatakey = "dateFormatDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string dateSetting = "";

            if (roamingSetting == "true")
            {
                dateSetting = storage.LoadRoamingSettings(dfDatakey);
            }
            else
            {
                dateSetting = storage.LoadSettings(dfDatakey);
            }

            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");

            if (dateSetting == "0")
            {
                users[0].modDOB = users[0].DOB.ToString("d MMMM yy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "1")
            {
                users[0].modDOB = users[0].DOB.ToString("dddd d MMMM yyyy");
                users[0].modJoined = users[0].Joined.ToString("dddd d MMMM yyyy");
            }
            else if (dateSetting == "2")
            {
                users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "3")
            {
                users[0].modDOB = users[0].DOB.ToString("dd/MM/yyyy");
                users[0].modJoined = users[0].Joined.ToString("dd/MM/yyyy");
            }
            else if (dateSetting == "4")
            {
                users[0].modDOB = users[0].DOB.ToString("M/d/yyyy");
                users[0].modJoined = users[0].Joined.ToString("M/d/yyyy");
            }
            else
            {
                users[0].modDOB = users[0].DOB.ToString("d MMMM yy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }

            users[0].modDOB = "Birth Date: " + users[0].modDOB;

            users[0].modStories = "Stories Written: " + users[0].Stories;


            DateTime today = DateTime.Today;
            int age = today.Year - users[0].DOB.Year;
            if (users[0].DOB > today.AddYears(-age)) age--;

            users[0].modAge = age;
            users[0].modAgeDisplay = "Age: " + age;

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

            users[0].modAccountType = "Account Type: " + users[0].modAccountType;

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, JSON);
                storage.SaveRoamingSettings(uDataKey, username);
                storage.SaveRoamingSettings(pDataKey, password);
                storage.SaveRoamingSettings(nDataKey, users[0].Name);
                storage.SaveRoamingSettings(avDataKey, users[0].Avatar);
                storage.SaveRoamingSettings(aDataKey, users[0].Author);
                storage.SaveRoamingSettings(idDataKey, users[0].ID);
                storage.SaveRoamingSettings(adDatakey, users[0].Admin);
            }
            else
            {
                storage.SaveSettings(sDataKey, JSON);
                storage.SaveSettings(uDataKey, username);
                storage.SaveSettings(pDataKey, password);
                storage.SaveSettings(nDataKey, users[0].Name);
                storage.SaveSettings(avDataKey, users[0].Avatar);
                storage.SaveSettings(aDataKey, users[0].Author);
                storage.SaveSettings(idDataKey, users[0].ID);
                storage.SaveSettings(adDatakey, users[0].Admin);
            }

        }

        public UserDataSource(string JSON, string JSONStories, string username, string password)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(JSON);

            //users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");

            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");
            //users[0].modDOB = users[0].DOB.ToString("d MMMM");

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string aDataKey = "authorDetails";
            string avDataKey = "avatarDetails";
            string idDataKey = "userIdDetails";
            string adDatakey = "adminDetails";
            string dfDatakey = "dateFormatDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string dateSetting = "";

            if (roamingSetting == "true")
            {
                dateSetting = storage.LoadRoamingSettings(dfDatakey);
            }
            else if (roamingSetting == "false")
            {
                dateSetting = storage.LoadSettings(dfDatakey);
            }
            else if (roamingSetting == "Null")
            {
                dateSetting = storage.LoadSettings(dfDatakey);
            }



            //users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");

            if (dateSetting == "0")
            {
                users[0].modDOB = users[0].DOB.ToString("d MMMM yy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "1")
            {
                users[0].modDOB = users[0].DOB.ToString("dddd d MMMM yyyy");
                users[0].modJoined = users[0].Joined.ToString("dddd d MMMM yyyy");
            }
            else if (dateSetting == "2")
            {
                users[0].modDOB = users[0].DOB.ToString("ddd d MMM yyy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }
            else if (dateSetting == "3")
            {
                users[0].modDOB = users[0].DOB.ToString("dd/MM/yyyy");
                users[0].modJoined = users[0].Joined.ToString("dd/MM/yyyy");
            }
            else if (dateSetting == "4")
            {
                users[0].modDOB = users[0].DOB.ToString("M/d/yyyy");
                users[0].modJoined = users[0].Joined.ToString("M/d/yyyy");
            }
            else
            {
                users[0].modDOB = users[0].DOB.ToString("d MMMM yy");
                users[0].modJoined = users[0].Joined.ToString("ddd d MMM yyy");
            }

            users[0].modDOB = "Birth Date: " + users[0].modDOB;

            users[0].modStories = "Stories Written: " + users[0].Stories;


            DateTime today = DateTime.Today;
            int age = today.Year - users[0].DOB.Year;
            if (users[0].DOB > today.AddYears(-age)) age--;

            users[0].modAge = age;
            users[0].modAgeDisplay = "Age: " + age;

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

            users[0].modAccountType = "Account Type: " + users[0].modAccountType;

            var usersByName = users.GroupBy(x => x.Username)
                                .Select(x => new UserSorted { Name = x.Key, Users = x.ToList() });

            Users = usersByName.ToList();



            List<Stories> stories = JsonConvert.DeserializeObject<List<Stories>>(JSONStories);

            var storiesByTitle = stories.GroupBy(x => x.Title)
                                .Select(x => new StoriesSorted { Title = x.Key, Stories = x.ToList() });

            Stories = storiesByTitle.ToList();

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
            else
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
