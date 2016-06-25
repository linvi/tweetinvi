using System;

namespace Tweetinvi.Core.Attributes
{
    // ReSharper disable UnusedMember.Global
    // ReSharper disable InconsistentNaming

    public class WindowsTimeZoneAttribute : Attribute
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public string WindowsId { get; private set; }
        public string TZinfo { get; private set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        public WindowsTimeZoneAttribute(string windowsId, string tzinfo)
        {
            WindowsId = windowsId;
            TZinfo = tzinfo;
        }
    }

    public enum WindowsTimeZone
    {
        /// <summary>
        /// (UTC-12:00) International Date Line West
        /// </summary>
        [WindowsTimeZone("Dateline Standard Time", "Pacific/Midway")]
        International_Date_Line_West,

        /// <summary>
        /// (UTC-11:00) Coordinated Universal Time-11
        /// </summary>
        [WindowsTimeZone("UTC-11", null)]
        Coordinated_Universal_Time_11,

        /// <summary>
        /// (UTC-10:00) Hawaii
        /// </summary>
        [WindowsTimeZone("Hawaiian Standard Time", "Pacific/Honolulu")]
        Hawaii,

        /// <summary>
        /// (UTC-09:00) Alaska
        /// </summary>
        [WindowsTimeZone("Alaskan Standard Time", "America/Juneau")]
        Alaska,

        /// <summary>
        /// (UTC-08:00) Baja California
        /// </summary>
        [WindowsTimeZone("Pacific Standard Time (Mexico)", null)]
        Baja_California,

        /// <summary>
        /// (UTC-08:00) Pacific Time (US & Canada)
        /// </summary>
        [WindowsTimeZone("Pacific Standard Time", "America/Los_Angeles")]
        Pacific_Time_US_Canada,

        /// <summary>
        /// (UTC-07:00) Arizona
        /// </summary>
        [WindowsTimeZone("US Mountain Standard Time", "America/Phoenix")]
        Arizona,

        /// <summary>
        /// (UTC-07:00) Chihuahua, La Paz, Mazatlan
        /// </summary>
        [WindowsTimeZone("Mountain Standard Time (Mexico)", "America/Chihuahua")]
        Chihuahua_La_Paz_Mazatlan,

        /// <summary>
        /// (UTC-07:00) Mountain Time (US & Canada)
        /// </summary>
        [WindowsTimeZone("Mountain Standard Time", null)]
        Mountain_Time_US_Canada,

        /// <summary>
        /// (UTC-06:00) Central America
        /// </summary>
        [WindowsTimeZone("Central America Standard Time", null)]
        Central_America,

        /// <summary>
        /// (UTC-06:00) Central Time (US & Canada)
        /// </summary>
        [WindowsTimeZone("Central Standard Time", null)]
        Central_Time_US_Canada,

        /// <summary>
        /// (UTC-06:00) Guadalajara, Mexico City, Monterrey
        /// </summary>
        [WindowsTimeZone("Central Standard Time (Mexico)", null)]
        Guadalajara_Mexico_City_Monterrey,

        /// <summary>
        /// (UTC-06:00) Saskatchewan
        /// </summary>
        [WindowsTimeZone("Canada Central Standard Time", null)]
        Saskatchewan,

        /// <summary>
        /// (UTC-05:00) Bogota, Lima, Quito, Rio Branco
        /// </summary>
        [WindowsTimeZone("SA Pacific Standard Time", null)]
        Bogota_Lima_Quito_Rio_Branco,

        /// <summary>
        /// (UTC-05:00) Eastern Time (US & Canada)
        /// </summary>
        [WindowsTimeZone("Eastern Standard Time", null)]
        Eastern_Time_US_Canada,

        /// <summary>
        /// (UTC-05:00) Indiana (East)
        /// </summary>
        [WindowsTimeZone("US Eastern Standard Time", null)]
        Indiana_East,

        /// <summary>
        /// (UTC-04:30) Caracas
        /// </summary>
        [WindowsTimeZone("Venezuela Standard Time", null)]
        Caracas,

        /// <summary>
        /// (UTC-04:00) Asuncion
        /// </summary>
        [WindowsTimeZone("Paraguay Standard Time", null)]
        Asuncion,

        /// <summary>
        /// (UTC-04:00) Atlantic Time (Canada)
        /// </summary>
        [WindowsTimeZone("Atlantic Standard Time", null)]
        Atlantic_Time_Canada,

        /// <summary>
        /// (UTC-04:00) Cuiaba
        /// </summary>
        [WindowsTimeZone("Central Brazilian Standard Time", null)]
        Cuiaba,

        /// <summary>
        /// (UTC-04:00) Georgetown, La Paz, Manaus, San Juan
        /// </summary>
        [WindowsTimeZone("SA Western Standard Time", "America/La_Paz")]
        Georgetown_La_Paz_Manaus_San_Juan,

        /// <summary>
        /// (UTC-04:00) Santiago
        /// </summary>
        [WindowsTimeZone("Pacific SA Standard Time", null)]
        Santiago,

        /// <summary>
        /// (UTC-03:30) Newfoundland
        /// </summary>
        [WindowsTimeZone("Newfoundland Standard Time", null)]
        Newfoundland,

        /// <summary>
        /// (UTC-03:00) Brasilia
        /// </summary>
        [WindowsTimeZone("E. South America Standard Time", null)]
        Brasilia,

        /// <summary>
        /// (UTC-03:00) Buenos Aires
        /// </summary>
        [WindowsTimeZone("Argentina Standard Time", null)]
        Buenos_Aires,

        /// <summary>
        /// (UTC-03:00) Cayenne, Fortaleza
        /// </summary>
        [WindowsTimeZone("SA Eastern Standard Time", null)]
        Cayenne_Fortaleza,

        /// <summary>
        /// (UTC-03:00) Greenland
        /// </summary>
        [WindowsTimeZone("Greenland Standard Time", null)]
        Greenland,

        /// <summary>
        /// (UTC-03:00) Montevideo
        /// </summary>
        [WindowsTimeZone("Montevideo Standard Time", null)]
        Montevideo,

        /// <summary>
        /// (UTC-03:00) Salvador
        /// </summary>
        [WindowsTimeZone("Bahia Standard Time", null)]
        Salvador,

        /// <summary>
        /// (UTC-02:00) Coordinated Universal Time-02
        /// </summary>
        [WindowsTimeZone("UTC-02", null)]
        Coordinated_Universal_Time_02,

        /// <summary>
        /// (UTC-02:00) Mid-Atlantic - Old
        /// </summary>
        [WindowsTimeZone("Mid-Atlantic Standard Time", null)]
        Mid_Atlantic_Old,

        /// <summary>
        /// (UTC-01:00) Azores
        /// </summary>
        [WindowsTimeZone("Azores Standard Time", null)]
        Azores,

        /// <summary>
        /// (UTC-01:00) Cape Verde Is.
        /// </summary>
        [WindowsTimeZone("Cape Verde Standard Time", null)]
        Cape_Verde_Is_,

        /// <summary>
        /// (UTC) Casablanca
        /// </summary>
        [WindowsTimeZone("Morocco Standard Time", null)]
        Casablanca,

        /// <summary>
        /// (UTC) Coordinated Universal Time
        /// </summary>
        [WindowsTimeZone("UTC", null)]
        Coordinated_Universal_Time,

        /// <summary>
        /// (UTC) Dublin, Edinburgh, Lisbon, London
        /// </summary>
        [WindowsTimeZone("GMT Standard Time", null)]
        Dublin_Edinburgh_Lisbon_London,

        /// <summary>
        /// (UTC) Monrovia, Reykjavik
        /// </summary>
        [WindowsTimeZone("Greenwich Standard Time", null)]
        Monrovia_Reykjavik,

        /// <summary>
        /// (UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna
        /// </summary>
        [WindowsTimeZone("W. Europe Standard Time", null)]
        Amsterdam_Berlin_Bern_Rome_Stockholm_Vienna,

        /// <summary>
        /// (UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague
        /// </summary>
        [WindowsTimeZone("Central Europe Standard Time", null)]
        Belgrade_Bratislava_Budapest_Ljubljana_Prague,

        /// <summary>
        /// (UTC+01:00) Brussels, Copenhagen, Madrid, Paris
        /// </summary>
        [WindowsTimeZone("Romance Standard Time", null)]
        Brussels_Copenhagen_Madrid_Paris,

        /// <summary>
        /// (UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb
        /// </summary>
        [WindowsTimeZone("Central European Standard Time", null)]
        Sarajevo_Skopje_Warsaw_Zagreb,

        /// <summary>
        /// (UTC+01:00) West Central Africa
        /// </summary>
        [WindowsTimeZone("W. Central Africa Standard Time", null)]
        West_Central_Africa,

        /// <summary>
        /// (UTC+01:00) Windhoek
        /// </summary>
        [WindowsTimeZone("Namibia Standard Time", null)]
        Windhoek,

        /// <summary>
        /// (UTC+02:00) Amman
        /// </summary>
        [WindowsTimeZone("Jordan Standard Time", null)]
        Amman,

        /// <summary>
        /// (UTC+02:00) Athens, Bucharest
        /// </summary>
        [WindowsTimeZone("GTB Standard Time", null)]
        Athens_Bucharest,

        /// <summary>
        /// (UTC+02:00) Beirut
        /// </summary>
        [WindowsTimeZone("Middle East Standard Time", null)]
        Beirut,

        /// <summary>
        /// (UTC+02:00) Cairo
        /// </summary>
        [WindowsTimeZone("Egypt Standard Time", null)]
        Cairo,

        /// <summary>
        /// (UTC+02:00) Damascus
        /// </summary>
        [WindowsTimeZone("Syria Standard Time", null)]
        Damascus,

        /// <summary>
        /// (UTC+02:00) E. Europe
        /// </summary>
        [WindowsTimeZone("E. Europe Standard Time", null)]
        E__Europe,

        /// <summary>
        /// (UTC+02:00) Harare, Pretoria
        /// </summary>
        [WindowsTimeZone("South Africa Standard Time", null)]
        Harare_Pretoria,

        /// <summary>
        /// (UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius
        /// </summary>
        [WindowsTimeZone("FLE Standard Time", null)]
        Helsinki_Kyiv_Riga_Sofia_Tallinn_Vilnius,

        /// <summary>
        /// (UTC+02:00) Istanbul
        /// </summary>
        [WindowsTimeZone("Turkey Standard Time", null)]
        Istanbul,

        /// <summary>
        /// (UTC+02:00) Jerusalem
        /// </summary>
        [WindowsTimeZone("Israel Standard Time", null)]
        Jerusalem,

        /// <summary>
        /// (UTC+02:00) Kaliningrad (RTZ 1)
        /// </summary>
        [WindowsTimeZone("Kaliningrad Standard Time", null)]
        Kaliningrad_RTZ_1,

        /// <summary>
        /// (UTC+02:00) Tripoli
        /// </summary>
        [WindowsTimeZone("Libya Standard Time", null)]
        Tripoli,

        /// <summary>
        /// (UTC+03:00) Baghdad
        /// </summary>
        [WindowsTimeZone("Arabic Standard Time", null)]
        Baghdad,

        /// <summary>
        /// (UTC+03:00) Kuwait, Riyadh
        /// </summary>
        [WindowsTimeZone("Arab Standard Time", null)]
        Kuwait_Riyadh,

        /// <summary>
        /// (UTC+03:00) Minsk
        /// </summary>
        [WindowsTimeZone("Belarus Standard Time", null)]
        Minsk,

        /// <summary>
        /// (UTC+03:00) Moscow, St. Petersburg, Volgograd (RTZ 2)
        /// </summary>
        [WindowsTimeZone("Russian Standard Time", null)]
        Moscow_St__Petersburg_Volgograd_RTZ_2,

        /// <summary>
        /// (UTC+03:00) Nairobi
        /// </summary>
        [WindowsTimeZone("E. Africa Standard Time", null)]
        Nairobi,

        /// <summary>
        /// (UTC+03:30) Tehran
        /// </summary>
        [WindowsTimeZone("Iran Standard Time", null)]
        Tehran,

        /// <summary>
        /// (UTC+04:00) Abu Dhabi, Muscat
        /// </summary>
        [WindowsTimeZone("Arabian Standard Time", null)]
        Abu_Dhabi_Muscat,

        /// <summary>
        /// (UTC+04:00) Baku
        /// </summary>
        [WindowsTimeZone("Azerbaijan Standard Time", null)]
        Baku,

        /// <summary>
        /// (UTC+04:00) Izhevsk, Samara (RTZ 3)
        /// </summary>
        [WindowsTimeZone("Russia Time Zone 3", null)]
        Izhevsk_Samara_RTZ_3,

        /// <summary>
        /// (UTC+04:00) Port Louis
        /// </summary>
        [WindowsTimeZone("Mauritius Standard Time", null)]
        Port_Louis,

        /// <summary>
        /// (UTC+04:00) Tbilisi
        /// </summary>
        [WindowsTimeZone("Georgian Standard Time", null)]
        Tbilisi,

        /// <summary>
        /// (UTC+04:00) Yerevan
        /// </summary>
        [WindowsTimeZone("Caucasus Standard Time", null)]
        Yerevan,

        /// <summary>
        /// (UTC+04:30) Kabul
        /// </summary>
        [WindowsTimeZone("Afghanistan Standard Time", null)]
        Kabul,

        /// <summary>
        /// (UTC+05:00) Ashgabat, Tashkent
        /// </summary>
        [WindowsTimeZone("West Asia Standard Time", null)]
        Ashgabat_Tashkent,

        /// <summary>
        /// (UTC+05:00) Ekaterinburg (RTZ 4)
        /// </summary>
        [WindowsTimeZone("Ekaterinburg Standard Time", null)]
        Ekaterinburg_RTZ_4,

        /// <summary>
        /// (UTC+05:00) Islamabad, Karachi
        /// </summary>
        [WindowsTimeZone("Pakistan Standard Time", null)]
        Islamabad_Karachi,

        /// <summary>
        /// (UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi
        /// </summary>
        [WindowsTimeZone("India Standard Time", null)]
        Chennai_Kolkata_Mumbai_New_Delhi,

        /// <summary>
        /// (UTC+05:30) Sri Jayawardenepura
        /// </summary>
        [WindowsTimeZone("Sri Lanka Standard Time", null)]
        Sri_Jayawardenepura,

        /// <summary>
        /// (UTC+05:45) Kathmandu
        /// </summary>
        [WindowsTimeZone("Nepal Standard Time", null)]
        Kathmandu,

        /// <summary>
        /// (UTC+06:00) Astana
        /// </summary>
        [WindowsTimeZone("Central Asia Standard Time", null)]
        Astana,

        /// <summary>
        /// (UTC+06:00) Dhaka
        /// </summary>
        [WindowsTimeZone("Bangladesh Standard Time", null)]
        Dhaka,

        /// <summary>
        /// (UTC+06:00) Novosibirsk (RTZ 5)
        /// </summary>
        [WindowsTimeZone("N. Central Asia Standard Time", null)]
        Novosibirsk_RTZ_5,

        /// <summary>
        /// (UTC+06:30) Yangon (Rangoon)
        /// </summary>
        [WindowsTimeZone("Myanmar Standard Time", null)]
        Yangon_Rangoon,

        /// <summary>
        /// (UTC+07:00) Bangkok, Hanoi, Jakarta
        /// </summary>
        [WindowsTimeZone("SE Asia Standard Time", null)]
        Bangkok_Hanoi_Jakarta,

        /// <summary>
        /// (UTC+07:00) Krasnoyarsk (RTZ 6)
        /// </summary>
        [WindowsTimeZone("North Asia Standard Time", null)]
        Krasnoyarsk_RTZ_6,

        /// <summary>
        /// (UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi
        /// </summary>
        [WindowsTimeZone("China Standard Time", null)]
        Beijing_Chongqing_Hong_Kong_Urumqi,

        /// <summary>
        /// (UTC+08:00) Irkutsk (RTZ 7)
        /// </summary>
        [WindowsTimeZone("North Asia East Standard Time", null)]
        Irkutsk_RTZ_7,

        /// <summary>
        /// (UTC+08:00) Kuala Lumpur, Singapore
        /// </summary>
        [WindowsTimeZone("Singapore Standard Time", null)]
        Kuala_Lumpur_Singapore,

        /// <summary>
        /// (UTC+08:00) Perth
        /// </summary>
        [WindowsTimeZone("W. Australia Standard Time", null)]
        Perth,

        /// <summary>
        /// (UTC+08:00) Taipei
        /// </summary>
        [WindowsTimeZone("Taipei Standard Time", null)]
        Taipei,

        /// <summary>
        /// (UTC+08:00) Ulaanbaatar
        /// </summary>
        [WindowsTimeZone("Ulaanbaatar Standard Time", null)]
        Ulaanbaatar,

        /// <summary>
        /// (UTC+09:00) Osaka, Sapporo, Tokyo
        /// </summary>
        [WindowsTimeZone("Tokyo Standard Time", null)]
        Osaka_Sapporo_Tokyo,

        /// <summary>
        /// (UTC+09:00) Seoul
        /// </summary>
        [WindowsTimeZone("Korea Standard Time", null)]
        Seoul,

        /// <summary>
        /// (UTC+09:00) Yakutsk (RTZ 8)
        /// </summary>
        [WindowsTimeZone("Yakutsk Standard Time", null)]
        Yakutsk_RTZ_8,

        /// <summary>
        /// (UTC+09:30) Adelaide
        /// </summary>
        [WindowsTimeZone("Cen. Australia Standard Time", null)]
        Adelaide,

        /// <summary>
        /// (UTC+09:30) Darwin
        /// </summary>
        [WindowsTimeZone("AUS Central Standard Time", null)]
        Darwin,

        /// <summary>
        /// (UTC+10:00) Brisbane
        /// </summary>
        [WindowsTimeZone("E. Australia Standard Time", null)]
        Brisbane,

        /// <summary>
        /// (UTC+10:00) Canberra, Melbourne, Sydney
        /// </summary>
        [WindowsTimeZone("AUS Eastern Standard Time", null)]
        Canberra_Melbourne_Sydney,

        /// <summary>
        /// (UTC+10:00) Guam, Port Moresby
        /// </summary>
        [WindowsTimeZone("West Pacific Standard Time", null)]
        Guam_Port_Moresby,

        /// <summary>
        /// (UTC+10:00) Hobart
        /// </summary>
        [WindowsTimeZone("Tasmania Standard Time", null)]
        Hobart,

        /// <summary>
        /// (UTC+10:00) Magadan
        /// </summary>
        [WindowsTimeZone("Magadan Standard Time", null)]
        Magadan,

        /// <summary>
        /// (UTC+10:00) Vladivostok, Magadan (RTZ 9)
        /// </summary>
        [WindowsTimeZone("Vladivostok Standard Time", null)]
        Vladivostok_Magadan_RTZ_9,

        /// <summary>
        /// (UTC+11:00) Chokurdakh (RTZ 10)
        /// </summary>
        [WindowsTimeZone("Russia Time Zone 10", null)]
        Chokurdakh_RTZ_10,

        /// <summary>
        /// (UTC+11:00) Solomon Is., New Caledonia
        /// </summary>
        [WindowsTimeZone("Central Pacific Standard Time", null)]
        Solomon_Is__New_Caledonia,

        /// <summary>
        /// (UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky (RTZ 11)
        /// </summary>
        [WindowsTimeZone("Russia Time Zone 11", null)]
        Anadyr_Petropavlovsk_Kamchatsky_RTZ_11,

        /// <summary>
        /// (UTC+12:00) Auckland, Wellington
        /// </summary>
        [WindowsTimeZone("New Zealand Standard Time", null)]
        Auckland_Wellington,

        /// <summary>
        /// (UTC+12:00) Coordinated Universal Time+12
        /// </summary>
        [WindowsTimeZone("UTC+12", null)]
        Coordinated_Universal_Time_12,

        /// <summary>
        /// (UTC+12:00) Fiji
        /// </summary>
        [WindowsTimeZone("Fiji Standard Time", null)]
        Fiji,

        /// <summary>
        /// (UTC+12:00) Petropavlovsk-Kamchatsky - Old
        /// </summary>
        [WindowsTimeZone("Kamchatka Standard Time", null)]
        Petropavlovsk_Kamchatsky_Old,

        /// <summary>
        /// (UTC+13:00) Nuku'alofa
        /// </summary>
        [WindowsTimeZone("Tonga Standard Time", null)]
        Nuku_alofa,

        /// <summary>
        /// (UTC+13:00) Samoa
        /// </summary>
        [WindowsTimeZone("Samoa Standard Time", "Pacific/Pago_Pago")]
        Samoa,

        /// <summary>
        /// (UTC+14:00) Kiritimati Island
        /// </summary>
        [WindowsTimeZone("Line Islands Standard Time", null)]
        Kiritimati_Island,
    }

    // ReSharper restore UnusedMember.Global
    // ReSharper restore InconsistentNaming
    // ReSharper restore InconsistentNaming

    internal class WindowsTimeZoneGenerator
    {
        internal void PrintWindowTimeZone()
        {
            // This code needs to be executed from .NET 4.0 (non portable)

            //foreach (var t in TimeZoneInfo.GetSystemTimeZones())
            //{
            //    var name = t.DisplayName;
            //    var regex = new Regex(@"\(UTC([\+\-]\d\d:\d\d)?\) ");
            //    var result = regex.Replace(name, string.Empty);
            //    result = result.Replace("(", string.Empty);
            //    result = result.Replace(")", string.Empty);

            //    var regex2 = new Regex(@"(, | - | & | +|[ ,\+\-'\.])");
            //    result = regex2.Replace(result, "_");

            //    var summary = "/// <summary>\r\n/// {0}\r\n/// </summary>\r\n";
            //    var flags = "[TwitterTimeZoneDescription(\"{1}\", null)]\r\n";
            //    var enumName = "{2},\r\n";

            //    var enumEntry = string.Format("{0}{1}{2}", summary, flags, enumName);
            //    Debug.WriteLine(string.Format(enumEntry, t.DisplayName, t.Id, result));
            //}
        }
    }
}