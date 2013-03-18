using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppleService
{
    public class AppleSubscription : TableServiceEntity
    {
        public AppleSubscription() { }

        private const string _subscriptionsPartition = "subscriptions";
        // Every device has a channel through which it can receive our notifications
        private string _devicePushChannel = string.Empty;
        public string DevicePushChannel
        {
            get { return _devicePushChannel; }
            set { _devicePushChannel = value; }
        }

        public AppleSubscription(string devicePushChannel)
        {
            this.PartitionKey = _subscriptionsPartition;
            this.RowKey = Guid.NewGuid().ToString();
            // We can't use devicePushChannel as the row key because it contains backslashes
            this.DevicePushChannel = devicePushChannel;
        }

    }
}