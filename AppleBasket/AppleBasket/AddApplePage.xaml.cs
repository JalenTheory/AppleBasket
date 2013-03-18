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
using System.Device.Location;
using Microsoft.Phone.Shell;

namespace AppleBasket
{
    public partial class AddApplePage : PhoneApplicationPage
    {
        GeoCoordinateWatcher geoWatcher;
        AppleServiceClient client;
        public AddApplePage()
        {
            InitializeComponent();
            geoWatcher = new GeoCoordinateWatcher();
            geoWatcher.MovementThreshold = 10;
            geoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatcher_PositionChanged);
            geoWatcher.Start();
            client = new AppleServiceClient();
            client.AddAppleCompleted += new EventHandler<AddAppleCompletedEventArgs>(client_AddAppleCompleted);
        }

        void client_AddAppleCompleted(object sender, AddAppleCompletedEventArgs e)
        {
            MessageBox.Show("Apple added!");
        }

        void geoWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //if (sub == null) sub = ApplicationBar.MenuItems[0] as ApplicationBarIconButton;
            //sub.IsEnabled = true;
            map.Center = e.Position.Location;
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            client.AddAppleAsync(new Apple()
            {
                PartitionKey = "apples",
                RowKey = Guid.NewGuid().ToString(),
                Latitude = map.Center.Latitude,
                Longitude = map.Center.Longitude,
                AppleCount = (int)amount.Value,
                Reserved = false,
                Timestamp = DateTime.Now
            });
        }
    }
}