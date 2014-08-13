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

        public MainPage()
        {
            clientId = "SimpleDJ_802";
            clientSecret = "/Cthq+SIvtTJk1P9x04JX7P3H2RVzxZXqht0Yq5LqMg=";

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
            IXboxMusicClient client = XboxMusicClientFactory.CreateXboxMusicClient(clientId, clientSecret);

            // Use null to get your current geography.
            // Specify a 2 letter country code (such as "US" or "DE") to force a specific country.
            string country = null;
            ResultList.Items.Clear();

            // Search for albums in your current geography
            ContentResponse searchResponse = await client.SearchAsync(Namespace.music, SearchText.Text, filter: SearchFilter.Albums);

            foreach (Album albumResult in searchResponse.Albums.Items)
            {
                ResultList.Items.Add(albumResult.Name);
            }
        }
    }
}
