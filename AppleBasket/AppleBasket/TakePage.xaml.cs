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
using Microsoft.Phone.Tasks;

namespace AppleBasket
{
    public partial class TakePage : PhoneApplicationPage
    {
        public TakePage()
        {
            InitializeComponent();
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            PhoneCallTask call = new PhoneCallTask();
            call.PhoneNumber = txtblk_tel.Text;
            call.DisplayName = txtblk_name.Text;
            call.Show();
        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            BingMapsDirectionsTask bingMapsDirectionsTask = new BingMapsDirectionsTask();
            LabeledMapLocation destination = new LabeledMapLocation();
            bingMapsDirectionsTask.End = destination;
            bingMapsDirectionsTask.Show();
        }
    }
}