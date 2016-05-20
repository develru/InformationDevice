using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InformationDevice
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private  DispatcherTimer timer;
        public MainPage()
        {
            timer = new DispatcherTimer();
            timer.Tick += WeatherUpdateTimeout;

            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await UpdateUIWithWeatherData();
            timer.Interval = TimeSpan.FromHours(1);
            timer.Start();
        }

        private async Task UpdateUIWithWeatherData()
        {
            try
            {
                RootObject myWeather = await OpenWeatherMapProxy.GetWeather();
                string icon = string.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);
                ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
                TempTextBlock.Text = (int)myWeather.main.temp + " °C"; //((int)myWeather.main.temp).ToString();
                DescriptionTextBlock.Text = myWeather.weather[0].description;
                LocationTextBlock.Text = myWeather.name;

                // Forecast
                ForecastRootobject myForecast = await OpenWeatherForecastProxy.GetForecast();
                string forecastIconDay1 = string.Format("ms-appx:///Assets/Weather/{0}.png", myForecast.list[0].weather[0].icon);
                Day1Icon.Source = new BitmapImage(new Uri(forecastIconDay1, UriKind.Absolute));
                Day1TempTextBlock.Text = (int)myForecast.list[0].temp.max + "°C / " + (int)myForecast.list[0].temp.min + "°C";
                Day1DescriptionTextBlock.Text = myForecast.list[0].weather[0].description;
                var day1Time = FromUnixTime(myForecast.list[0].dt);
                Day1TimeTextBlock.Text = day1Time.ToLocalTime().ToString();

                string forecastIconDay2 = string.Format("ms-appx:///Assets/Weather/{0}.png", myForecast.list[1].weather[0].icon);
                Day2Icon.Source = new BitmapImage(new Uri(forecastIconDay2, UriKind.Absolute));
                Day2TempTextBlock.Text = (int)myForecast.list[1].temp.max + "°C / " + (int)myForecast.list[1].temp.min + "°C";
                Day2DescriptionTextBlock.Text = myForecast.list[1].weather[0].description;
                var day2Time = FromUnixTime(myForecast.list[1].dt);
                Day2TimeTextBlock.Text = day2Time.ToLocalTime().ToString();

                string forecastIconDay3 = string.Format("ms-appx:///Assets/Weather/{0}.png", myForecast.list[2].weather[0].icon);
                Day3Icon.Source = new BitmapImage(new Uri(forecastIconDay3, UriKind.Absolute));
                Day3TempTextBlock.Text = (int)myForecast.list[2].temp.max + "°C / " + (int)myForecast.list[2].temp.min + "°C";
                Day3DescriptionTextBlock.Text = myForecast.list[2].weather[0].description;
                var day3Time = FromUnixTime(myForecast.list[2].dt);
                Day3TimeTextBlock.Text = day3Time.ToLocalTime().ToString(); 
            }
            catch
            {
                LocationTextBlock.Text = "Unable to get the weather data!";
            }
        }

        private DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        private async void WeatherUpdateTimeout(object sender, object e)
        {
            await UpdateUIWithWeatherData();
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
    }
}
