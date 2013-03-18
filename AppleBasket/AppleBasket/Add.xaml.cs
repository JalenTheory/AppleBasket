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
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Windows.Media.Imaging;
using System.Collections;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.UserData;
using AppleBasket.AppleService;


namespace AppleBasket
{
    public partial class Add : PhoneApplicationPage
    {
        AppleServiceClient client;
       Apple apple;

        public Add()
        {
            InitializeComponent();
            client = new AppleServiceClient();
            client.AddSubscriptionCompleted += new EventHandler<AddSubscriptionCompletedEventArgs>(client_AddSubscriptionCompleted);
            
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
           
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 20;
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            //watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            watcher.Start();
            apple = new Apple();
            
        
        }

        void client_AddSubscriptionCompleted(object sender, AddSubscriptionCompletedEventArgs e)
        {
            MessageBox.Show("Announcement Added");
        }

        void photoCaptureOrSelectionCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            { 
                //MessageBox.Show(e.ChosenPhoto.Length.ToString());  //Code to display the photo on the page in an image control named myImage. 
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                _image.Source = bmp;                           
            } 
        } 

        void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show(" The Location Service is working, but it cannot get location data.");
                    break;
            }
        }       

        private void Rectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(photoCaptureOrSelectionCompleted);
            cameraCaptureTask.Show();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            client.AddSubscriptionAsync(txtblok_title.Text);
        }
    }
}