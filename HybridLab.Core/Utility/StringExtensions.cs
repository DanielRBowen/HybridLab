namespace HybridLab.Core.Utility
{
    public static class StringExtensions
    {
        public static string GetSubDomain(this string host)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(host))
                {
                    if (host.Contains("https://"))
                    {
                        host = host.Replace("https://", "");
                    }

                    return host.Split('.')[0].Trim().ToLower();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
