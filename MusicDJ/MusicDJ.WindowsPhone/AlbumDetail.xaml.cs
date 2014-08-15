using System;
using System.Linq;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using Microsoft.Xbox.Music.Platform.Client;
using Microsoft.Xbox.Music.Platform.Contract.DataModel;

namespace MusicDJ
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AlbumDetail : Page
    {

        private string clientId;
        private string clientSecret;
        IXboxMusicClient client;


        public AlbumDetail()
        {
            clientId = "SimpleDJ_802";
            clientSecret = "/Cthq+SIvtTJk1P9x04JX7P3H2RVzxZXqht0Yq5LqMg=";
            client = XboxMusicClientFactory.CreateXboxMusicClient(clientId, clientSecret);

            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ContentResponse lookupResponse = await client.LookupAsync(e.Parameter.ToString(), extras: ExtraDetails.Tracks);

            // Display information about the album
            Album album = lookupResponse.Albums.Items[0];
            AlbumImage.Source = new BitmapImage(new Uri(album.GetImageUrl(800, 800)));

            //AlbumDetailView.Text += string.Format("Album: {0} (link: {1})", album.Name, album.GetLink(ContentExtensions.LinkAction.Play));

            Artist.Text = String.Join(",", album.Artists.Select(x => x.Artist.Name));

            //foreach (Contributor contributor in album.Artists)
            //{
            //    Artist artist = contributor.Artist;
            //    Artist.Text += string.Format("Artist: {0} (link: {1})", artist.Name, artist.GetLink());
            //}
            TrackDetail.Items.Clear();

            foreach (Track track in album.Tracks.Items)
            {
                TrackDetail.Items.Add(string.Format("Track: {0} - {1}", track.TrackNumber, track.Name));
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }
    }
}
