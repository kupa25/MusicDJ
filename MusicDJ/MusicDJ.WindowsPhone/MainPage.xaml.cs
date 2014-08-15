using System;
using System.Globalization;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Xbox.Music.Platform.Client;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;

namespace MusicDJ
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string clientId;
        private string clientSecret;
        private Geolocator geolocator;
        IXboxMusicClient client;

        public MainPage()
        {
            clientId = "SimpleDJ_802";
            clientSecret = "/Cthq+SIvtTJk1P9x04JX7P3H2RVzxZXqht0Yq5LqMg=";
            client = XboxMusicClientFactory.CreateXboxMusicClient(clientId, clientSecret);

            geolocator = new Geolocator();

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Use null to get your current geography.
            // Specify a 2 letter country code (such as "US" or "DE") to force a specific country.
            string country = null;
            ResultList.Items.Clear();

            // Search for albums in your current geography
            ContentResponse searchResponse = await client.SearchAsync(Namespace.music, SearchText.Text, filter: SearchFilter.Albums);

            foreach (Album albumResult in searchResponse.Albums.Items)
            {
                ResultList.Items.Add(albumResult);
            }
        }

        private async void CreateGeofence()
        {
            GeofenceMonitor.Current.Geofences.Clear();

            BasicGeoposition basicGeoposition = new BasicGeoposition();
            Geoposition geoposition = await geolocator.GetGeopositionAsync();
            Geofence geofence;

            basicGeoposition.Latitude = geoposition.Coordinate.Latitude;
            basicGeoposition.Longitude = geoposition.Coordinate.Longitude;
            basicGeoposition.Altitude = (double) geoposition.Coordinate.Altitude;
            double radius = 10.0;

            Geocircle geocircle = new Geocircle(basicGeoposition, radius);

            // want to listen for enter geofence, exit geofence and remove geofence events
            // you can select a subset of these event states
            MonitoredGeofenceStates mask = 0;

            mask |= MonitoredGeofenceStates.Entered;
            mask |= MonitoredGeofenceStates.Exited;
            mask |= MonitoredGeofenceStates.Removed;

            // setting up how long you need to be in geofence for enter event to fire
            TimeSpan dwellTime = new TimeSpan(1, 0, 0);

            // setting up how long the geofence should be active
            TimeSpan duration = new TimeSpan(0,10,0);

            // setting up the start time of the geofence
            DateTimeOffset startTime = DateTimeOffset.Now;
            
            geofence = new Geofence("Test", geocircle, mask, true);

            GeofenceMonitor.Current.Geofences.Add(geofence);

        }

        private void Ellipse_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            CreateGeofence();
        }

        private void ResultList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var albumId = ResultList.SelectedItem == null ? string.Empty : ((Album) ResultList.SelectedItem).Id;

            if (!Frame.Navigate(typeof (AlbumDetail), albumId))
            {
                throw new Exception("Failed to navigate");
            }
        }
    }
}
