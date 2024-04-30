namespace EventService.Client.Models
{
    public class EventSubscriptionOverview(IEnumerable<EventSubscription> eventSubscriptions)
    {
        public IEnumerable<EventSubscription> EventSubscriptions = eventSubscriptions;
        public int RegistrationCount => EventSubscriptions.Count();
        public int TotalAmontVisitors => EventSubscriptions.Sum(x => x.Companions) + RegistrationCount;
    }
}
