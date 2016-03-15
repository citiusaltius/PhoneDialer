using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Defines
{
    public class Global
    {
        public static Amazon.RegionEndpoint DynamoDbRegion = Amazon.RegionEndpoint.USWest1;
        public static Amazon.RegionEndpoint CognitoIdRegion = Amazon.RegionEndpoint.USEast1;

        public static Amazon.RegionEndpoint SecurityTokenRegion = Amazon.RegionEndpoint.USEast1;
        public static int SecurityTokenDuration = 3600;

        public static Amazon.RegionEndpoint SqsRegion = Amazon.RegionEndpoint.USWest1;

        public static Amazon.RegionEndpoint S3Region = Amazon.RegionEndpoint.USWest1;
        public const string S3ServiceUrl = "https://s3-us-west-1.amazonaws.com";
        public static Amazon.S3.AmazonS3Config S3Config = new Amazon.S3.AmazonS3Config()
        {
            ServiceURL = S3ServiceUrl,
            AuthenticationRegion = S3Region.SystemName,
            RegionEndpoint = S3Region
        };

        public const string DateTimeFormatAmz = "yyyyMMddTHHmmssZ";

        public const int ServerDaemonEntityId = 343;

        public static TimeSpan ExpirationTimespanDefault = new TimeSpan(30, 0, 0, 0, 0);

        public static string UtcNowDateTimeFormatAmz()
        {
            return DateTime.UtcNow.ToString(DateTimeFormatAmz);
        }

        public static string UtcNowDateTimePlusTimeFormatAmz(TimeSpan AddedTime)
        {
            return DateTime.UtcNow.Add(AddedTime).ToString(DateTimeFormatAmz);
        }

        public static DateTime ParseAmzDateTimeFormat(string AmzDateTime)
        {
            DateTime retval;
            retval = DateTime.ParseExact(AmzDateTime, DateTimeFormatAmz, System.Globalization.CultureInfo.InvariantCulture,
                                       DateTimeStyles.AssumeUniversal |
                                       DateTimeStyles.AdjustToUniversal);
            return retval;
        }

        public const int DataVersionCurrent = 1;
    }
}
