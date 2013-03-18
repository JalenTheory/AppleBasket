using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using AppleBasket.AppleService;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Shell;

namespace AppleBasket
{
    public partial class Pan : PhoneApplicationPage
    {
        AppleServiceClient client;
        private HttpNotificationChannel _devicePushChannel;
        public Pan()
        {
            InitializeComponent();
            SubscribeToNotifications();
            client = new AppleServiceClient();
            client.AddSubscriptionCompleted += new EventHandler<AddSubscriptionCompletedEventArgs>(client_AddSubscriptionCompleted);

            ShellTile PrimaryTile = ShellTile.ActiveTiles.First();

            if (PrimaryTile != null)
            {
                StandardTileData tile = new StandardTileData();

                tile.BackgroundImage = new Uri("/icons/tile.jpg", UriKind.Relative);
                tile.Count = 4;
                tile.Title = "Fenton's Apples";
                PrimaryTile.Update(tile);
            }
            initializeAppleList();
        }
        private void initializeAppleList()
        {
            AppleServiceClient client2;
            client2 = new AppleServiceClient();
            client2.GetApplesCompleted += new EventHandler<GetApplesCompletedEventArgs>(client2_GetApplesCompleted);
            client2.GetApplesAsync();
        }

        void client2_GetApplesCompleted(object sender, GetApplesCompletedEventArgs e)
        {
            //listbox_appleList.DataContext = e.Result;
            listbox_appleList.ItemsSource = e.Result;

        }
       

        void client_AddSubscriptionCompleted(object sender, AddSubscriptionCompletedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Add.xaml", UriKind.Relative));
        }

        private void SubscribeToNotifications()
        {

            _devicePushChannel = HttpNotificationChannel.Find("DevicePushChannel");


            if (_devicePushChannel == null)
            {
                _devicePushChannel = new
                HttpNotificationChannel("DevicePushChannel");
                _devicePushChannel.Open();
            }

            _devicePushChannel.ShellToastNotificationReceived += ToastReceived;


            // Wait until the channel is open

            _devicePushChannel.ChannelUriUpdated += new
            EventHandler<NotificationChannelUriEventArgs>(devicePushChannel_ChannelUriUpdated);

        }


        void devicePushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        { 
            //toastClient.SubscribeToToastsAsync(e.ChannelUri.ToString());
            client.AddSubscriptionAsync(e.ChannelUri.ToString());

            // Allows the application to react to toast notifications

            _devicePushChannel.BindToShellToast(); 
        }


        private void ToastReceived(object sender, NotificationEventArgs e)
        { 
            // If the toast is received when the application is in foreground, show it in a message box

            Dispatcher.BeginInvoke(() =>
            { 
                MessageBox.Show(e.Collection["wp:Text2"], e.Collection["wp:Text1"], MessageBoxButton.OK);

            });

        }
    }
}
