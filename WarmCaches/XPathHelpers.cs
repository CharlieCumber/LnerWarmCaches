namespace EastCoast.BrokenLinkTests
{
    public static class XPathHelpers
    {
        public static string ElementWithValue(string element, string value) => $"//{element}[@value='{value}']";
    }
}
