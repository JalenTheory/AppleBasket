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

namespace AppleBasket
{
    public partial class ListApplesPage : PhoneApplicationPage
    {
        AppleServiceClient client;
        public ListApplesPage()
        {
            InitializeComponent();
            client = new AppleServiceClient();
            client.GetApplesCompleted += new EventHandler<GetApplesCompletedEventArgs>(client_GetApplesCompleted);
        }

        void client_GetApplesCompleted(object sender, GetApplesCompletedEventArgs e)
        {
            this.DataContext = e.Result;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            client.GetApplesAsync();
        }
    }
}