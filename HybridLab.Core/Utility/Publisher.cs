namespace HybridLab.Core.Utility
{
    /// <summary>
    /// To call functions from other services without creating circular dependencies.
    /// Code from Bing chat AI.
    /// </summary>
    public class Publisher
    {
        private readonly Dictionary<string, Action> subscribers = new();

        public void On(string eventName, Action subscriber)
        {
            if (!subscribers.ContainsKey(eventName))
            {
                subscribers.Add(eventName, subscriber);
            }
            else
            {
                subscribers[eventName] += subscriber;
            }
        }

        public void Call(string eventName)
        {
            if (subscribers.TryGetValue(eventName, out var value))
            {
                value.Invoke();
            }
        }
    }
}
