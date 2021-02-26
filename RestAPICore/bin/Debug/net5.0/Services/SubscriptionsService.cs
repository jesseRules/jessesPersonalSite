using MongoDB.Driver;
using RestAPICore.Models;
using System.Linq;

namespace RestAPICore.Services
{
    public class SubscriptionsService
    {
        private readonly IMongoCollection<SubscriptionModel> _subscriptions;

        public SubscriptionsService(ISubscriptionDBSettingsModel settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _subscriptions = database.GetCollection<SubscriptionModel>(settings.SubscriptionsCollectionName);
        }

        public SubscriptionModel Get(string id) =>
        _subscriptions.Find<SubscriptionModel>(subscription => subscription.Id == id).FirstOrDefault();

        public SubscriptionModel Create(SubscriptionModel subscription)
        {
            _subscriptions.InsertOne(subscription);
            return subscription;
        }
    }
}