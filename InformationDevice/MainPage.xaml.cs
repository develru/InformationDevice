using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<List> myForecastDataList { get; set; }

        public MainPage()
        {
            timer = new DispatcherTimer();
            timer.Tick += WeatherUpdateTimeout;

            this.InitializeComponent();

            myForecastDataList = new ObservableCollection<List>();
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
                await OpenWeatherForecastProxy.GetForecast(myForecastDataList);
                var now = DateTime.Now;
                StatusTextBlock.Text = string.Format("Last update: {0}:{1}", now.Hour, now.Minute);
            }
            catch
            {
                LocationTextBlock.Text = "Unable to get the weather data!";
            }
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
