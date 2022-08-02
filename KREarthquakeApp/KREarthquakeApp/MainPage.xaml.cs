using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

// You need to download the package for these two.
// To do so, just open the NuGet Package Manager under Tools and select "Manage NuGet packages for solution..."
// then download the latest NewtonSoft.Json package.
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace KREarthquakeApp
{
    public partial class MainPage : ContentPage
    {
        //Create the three variables that will be entered by the user
        Random random = new Random(); //Will be used to chose a random earthquake
        string startDate;
        string endDate;
        int magnitude;

        public MainPage()
        {
            InitializeComponent();
        }

        private void LoadEarthquakeData()
        {
            // Format the the DatePicker date to send to the API
            startDate = DatePickerStartDate.Date.ToString("yyyy-MM-dd");
            endDate = DatePickerEndDate.Date.ToString("yyyy-MM-dd");
            magnitude = int.Parse(EntMag.Text);

            //should connect to the api and save responses
            using (WebClient wc = new WebClient())
            {

                //this encodes special charecters in c# code
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    //The {} allows us to update those variables in the URL
                    string jsonData = wc.DownloadString($"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={startDate}&endtime={endDate}&minmagnitude={magnitude}");

                    JObject jo = JObject.Parse(jsonData);

                    // use the created class to deserialize the JSON data into C# properties and classes
                    AllEarthquakeDetails allEarthquake = JsonConvert.DeserializeObject<AllEarthquakeDetails>(jsonData);


                    //assigns variable randomNumber a number between 0 the count value in AllEarthquakeDetails
                    int randomNumber = random.Next(0, int.Parse(jo["metadata"]["count"].ToString()));
                

                    LblResults.Text = $"There were {allEarthquake.metadata.count} earthquakes during this time.\n\n" +
                    $"Details of one of them:\nPlace: {allEarthquake.features[randomNumber].properties.place}\nMagnitude: {allEarthquake.features[randomNumber].properties.mag}.";
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", ex.Message, "Close");
                }


            }
        }

        private void BtnFind_Clicked(object sender, EventArgs e)
        {
            if (true)
            {
                LoadEarthquakeData();
            }
            else
            {

            }
        }

    }
}
