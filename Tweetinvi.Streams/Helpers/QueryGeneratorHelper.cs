﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Tweetinvi.Streams.Helpers
{
    public class QueryGeneratorHelper
    {
        public static string GenerateFilterTrackRequest(List<string> tracks)
        {
            if (tracks.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("track=");
            for (int i = 0; i < tracks.Count - 1; ++i)
            {
                queryBuilder.Append(Uri.EscapeDataString(string.Format("{0},", tracks.ElementAt(i))));
            }

            queryBuilder.Append(Uri.EscapeDataString(tracks.ElementAt(tracks.Count - 1)));

            return queryBuilder.ToString();
        }

        public static string GenerateFilterFollowRequest(List<long?> followUserIds)
        {
            if (followUserIds.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("follow=");
            for (int i = 0; i < followUserIds.Count - 1; ++i)
            {
                queryBuilder.Append(Uri.EscapeDataString(string.Format("{0},", followUserIds.ElementAt(i))));
            }

            queryBuilder.Append(followUserIds.ElementAt(followUserIds.Count - 1));

            return queryBuilder.ToString();
        }

		private static string GenerateLocationParameters(ILocation location, bool isLastLocation)
		{
			double maxLongitude = Math.Max(location.Coordinate1.Longitude, location.Coordinate2.Longitude);
			double maxLatitude = Math.Max(location.Coordinate1.Latitude, location.Coordinate2.Latitude);

			double minLongitude = Math.Min(location.Coordinate1.Longitude, location.Coordinate2.Longitude);
			double minLatitude = Math.Min(location.Coordinate1.Latitude, location.Coordinate2.Latitude);

			//Twitter API wants longitude, latitude pairs, whereas we use the latitude, longitude convention in TweetInvi.
			return string.Format("{0},{1},{2},{3}{4}", minLongitude.ToString(CultureInfo.InvariantCulture),
				minLatitude.ToString(CultureInfo.InvariantCulture), maxLongitude.ToString(CultureInfo.InvariantCulture), maxLatitude.ToString(CultureInfo.InvariantCulture),
				isLastLocation ? "" : ",");
		}


		public static string GenerateFilterLocationRequest(List<ILocation> locations)
        {
            if (locations.IsNullOrEmpty())
            {
                return string.Empty;
            }

            StringBuilder queryBuilder = new StringBuilder();
            // queryBuilder.Append("locations=");
            for (int i = 0; i < locations.Count - 1; ++i)
            {
                queryBuilder.Append(GenerateLocationParameters(locations[i], false));
            }

            queryBuilder.Append(GenerateLocationParameters(locations[locations.Count - 1], true));

            return string.Format("locations={0}", StringFormater.UrlEncode(queryBuilder.ToString()));
        }
    }
}