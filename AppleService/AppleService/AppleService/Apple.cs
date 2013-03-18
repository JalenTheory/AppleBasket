using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AppleService
{
    public sealed class Apple : TableServiceEntity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public Apple() { }
        public Apple(int applecount, double latitude, double longitude, bool reserved)
        {
            Applecount = applecount;
            Latitude = latitude;
            Longitude = longitude;
            Reserved = reserved;
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = "apples";
        }

        private int _appleCount;
        public int Applecount
        {
            get { return _appleCount; }
            set { _appleCount = value; NotifyPropertyChanged("Applecount"); }
        }

        private double _latitude;
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; NotifyPropertyChanged("Latitude"); }
        }

        private double _longitude;
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; NotifyPropertyChanged("Longitude"); }
        }

        private bool _reserved;
        public bool Reserved
        {
            get { return _reserved; }
            set { _reserved = value; NotifyPropertyChanged("Reserved"); }
        }

        private int _applecount;
        public int AppleCount
        {
            get { return _applecount; }
            set { _applecount = value; NotifyPropertyChanged("AppleCount"); }
        }
    }
}