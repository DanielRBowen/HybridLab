namespace HybridLab.Core.Utility
{
    public static class NullObjectWaiter
    {
        /// <summary>
        /// From Bing Chat with this query:
        /// Create a c# class with a async function which takes an object and an action and optional timespan period as parameters. The function must chect if the object parameter is null, if it is null then it must start a timer for which runs every 200 millisecond period and checks if the object is null, if the object isn't null then it should run the action. The timer times out if the time is equal or greater than 1 second.
        /// 
        /// Example: Task.Run(async () => NullObjectWaiter.FireWhenNotNull(_itemOptions, async () => await _itemOptions.Open(menuItem.MenuItemId)));
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static async Task FireWhenNotNull(object obj, Action action, TimeSpan? period = null)
        {
            var timeout = TimeSpan.FromSeconds(1);
            var interval = period ?? TimeSpan.FromMilliseconds(200);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            while (obj == null && stopwatch.Elapsed < timeout)
            {
                await Task.Delay(interval);
            }

            if (obj != null)
                action();
        }
    }
}
