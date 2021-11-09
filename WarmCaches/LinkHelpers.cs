namespace EastCoast.BrokenLinkTests
{
    public static class LinkHelpers
    {
        public static string CleanVariablesFromUrl(this string url)
        {
            return url.Split('?')[0];
        }

        public static string CleanAnchorFromUrl(this string url)
        {
            return url.Split('#')[0];
        }

        public static bool IsInternalWebPageLink(this string url, string baseUrl)
        {
            return url.IsInternalLink(baseUrl) && !url.IsAssetLink();
        }

        private static bool IsAssetLink(this string url)
        {
            return url.Contains("/globalassets/") || url.Contains("/contentassets/");
        }

        private static bool IsInternalLink(this string url, string baseUrl)
        {
            return url.StartsWith(baseUrl);
        }
    }
}
