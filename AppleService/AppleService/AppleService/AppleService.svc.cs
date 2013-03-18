using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AppleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AppleService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AppleService.svc or AppleService.svc.cs at the Solution Explorer and start debugging.
    public class AppleService : IAppleService
    {
        private const string StorageConnectionString =
       "DefaultEndpointsProtocol=https;AccountName=applestorage;" +
       "AccountKey=EJx4IEKDLSws0fNa5Po9GqY5vFR7WvdJAbbwUXVo42RC0hsex4gPJgZRvxiRMhiCe+Fv/j/RsFfJGUc/J38jBg==";
        private const string AppleTableName = "apples";
        private const string SubscriptionTableName = "apples";

        public void DoWork()
        {
        }

        public bool AddApple(Apple apple)
        {
            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                (StorageConnectionString));



            // Create the table client
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist

            tableClient.CreateTableIfNotExist(AppleTableName);
            // Get the data service context
            var serviceContext = tableClient.GetDataServiceContext();

            // Add the new person to the people table
            serviceContext.AddObject(AppleTableName, apple);


            // Submit the operation to the table service
            serviceContext.SaveChangesWithRetries();

            SendToastMessage("New apples", apple.Applecount.ToString() +  " apples added");
            return true;
        }

        public bool ReserveApple(Apple apple)
        {
            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                (StorageConnectionString));



            // Create the table client
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist

            tableClient.CreateTableIfNotExist(AppleTableName);
            // Get the data service context
            var serviceContext = tableClient.GetDataServiceContext();

            var selectedApple = serviceContext.CreateQuery<Apple>(AppleTableName).Where(e => e.RowKey == apple.RowKey).First();
            if (selectedApple != null)
            {
                selectedApple.Reserved = true;
                SendToastMessage("Your apple was reserved", "Tap to check details");
            }
            // Add the new person to the people table
            //var alls = GetApples();
            //var theOne = alls.Where(e => e.RowKey == apple.RowKey).First();



            // Submit the operation to the table service
            serviceContext.SaveChangesWithRetries();

            SendToastMessage("New apples", apple.Applecount.ToString() + " apples added");
            return true;
        }



        public bool AddSubscription(string subscription)
        {
            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                (StorageConnectionString));



            // Create the table client
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist

            tableClient.CreateTableIfNotExist(SubscriptionTableName);
            // Get the data service context
            var serviceContext = tableClient.GetDataServiceContext();

            // Add the new person to the people table
            serviceContext.AddObject(AppleTableName, new AppleSubscription()
            {
                PartitionKey = "subs",
                RowKey = Guid.NewGuid().ToString(),
                DevicePushChannel = subscription.ToString()
            });

            // Submit the operation to the table service
            serviceContext.SaveChangesWithRetries();
            return true;
        }

        public List<Apple> GetApples()
        {
            List<Apple> apples = new List<Apple>();

            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                (StorageConnectionString));



            // Create the table client
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist

            tableClient.CreateTableIfNotExist(AppleTableName);
            // Get the data service context
            var serviceContext = tableClient.GetDataServiceContext();

            var partitionQuery = serviceContext.CreateQuery<Apple>(AppleTableName);

            // Loop through the results, displaying information about the entity
            return partitionQuery.ToList();

        }

        public List<AppleSubscription> GetSubscriptions()
        {
            List<AppleSubscription> apples = new List<AppleSubscription>();

            // Retrieve storage account from connection-string
            var storageAccount = CloudStorageAccount.Parse(
                (StorageConnectionString));



            // Create the table client
            var tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist

            tableClient.CreateTableIfNotExist(SubscriptionTableName);
            // Get the data service context
            var serviceContext = tableClient.GetDataServiceContext();

            var partitionQuery = serviceContext.CreateQuery<AppleSubscription>(SubscriptionTableName);

            // Loop through the results, displaying information about the entity
            return partitionQuery.ToList();

        }

        public bool SendToastMessage(string toastTitle, string toastText)
        {
            // Based on http://mobile.dzone.com/articles/notification-subscription
            // Toast messages are always formatted in this xml format

            string toastString = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
            "<wp:Notification xmlns:wp=\"WPNotification\">" +
            "<wp:Toast>" +
            "<wp:Text1>{0}</wp:Text1>" +
            "<wp:Text2>{1}</wp:Text2>" +
            "</wp:Toast>" +
            "</wp:Notification>";

            toastString = string.Format(toastString, toastTitle, toastText);
            byte[] messageBytes = Encoding.UTF8.GetBytes(toastString);

            // List all subscribers and send the message to all of them

            var subscriptionList = GetSubscriptions();

            foreach (AppleSubscription subscription in subscriptionList)
            {
                Uri channelUri = new
                Uri(subscription.DevicePushChannel);
                //Add headers to HTTP Post message.

                var myRequest = (HttpWebRequest)WebRequest.Create(channelUri); // The device's channelURI
                myRequest.Method = WebRequestMethods.Http.Post;
                myRequest.ContentType = "text/xml";
                myRequest.ContentLength = messageBytes.Length;

                myRequest.Headers.Add("X-MessageID", Guid.NewGuid().ToString()); // gives this message a unique ID

                myRequest.Headers["X-WindowsPhone-Target"] = "toast";
                // 2 = immediatly push toast
                // 12 = wait 450 seconds before push toast
                // 22 = wait 900 seconds before push toast

                myRequest.Headers.Add("X-NotificationClass", "2");
                //Merge headers with payload.

                using (Stream requestStream = myRequest.GetRequestStream())
                {

                    requestStream.Write(messageBytes, 0, messageBytes.Length);

                }
                //Send notification to the device
                try
                {
                    var response = (HttpWebResponse)myRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    //Log or handle exception
                }

            }
            return true;
        }


    }
}
