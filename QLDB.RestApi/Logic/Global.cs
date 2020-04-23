
using Amazon;

namespace AmazonQLDB.ServiceCore
{
    public static class Global
    {
        public static string LedgerName { get; set; }

        public static string AccessKey { get; set; }

        public static string SecretKey { get; set; }

        public static RegionEndpoint Region { get; set; } = RegionEndpoint.USEast1;
    }
}