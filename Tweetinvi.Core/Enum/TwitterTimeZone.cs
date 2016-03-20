using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tweetinvi.Core.Enum
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable UnusedMember.Global

    public class TwitterTimeZoneAttribute : Attribute
    {
        public string TZinfo { get; private set; }
        public string DisplayValue { get; private set; }

        public TwitterTimeZoneAttribute(string tzinfo, string displayValue)
        {
            TZinfo = tzinfo;
            DisplayValue = displayValue;
        }
    }

    /// <summary>
    /// As described on http://api.rubyonrails.org/classes/ActiveSupport/TimeZone.html
    /// </summary>
    public enum TwitterTimeZone
    {
        [TwitterTimeZone("Pacific/Midway", "International Date Line West")]
        International_Date_Line_West,

        [TwitterTimeZone("Pacific/Midway", "Midway Island")]
        Midway_Island,

        [TwitterTimeZone("Pacific/Pago_Pago", "American Samoa")]
        American_Samoa,

        [TwitterTimeZone("Pacific/Honolulu", "Hawaii")]
        Hawaii,

        [TwitterTimeZone("America/Juneau", "Alaska")]
        Alaska,

        [TwitterTimeZone("America/Tijuana", "Tijuana")]
        Tijuana,

        [TwitterTimeZone("America/Phoenix", "Arizona")]
        Arizona,

        [TwitterTimeZone("America/Chihuahua", "Chihuahua")]
        Chihuahua,

        [TwitterTimeZone("America/Mazatlan", "Mazatlan")]
        Mazatlan,

        [TwitterTimeZone("America/Regina", "Saskatchewan")]
        Saskatchewan,

        [TwitterTimeZone("America/Mexico_City", "Guadalajara")]
        Guadalajara,

        [TwitterTimeZone("America/Mexico_City", "Mexico City")]
        Mexico_City,

        [TwitterTimeZone("America/Monterrey", "Monterrey")]
        Monterrey,

        [TwitterTimeZone("America/Guatemala", "Central America")]
        Central_America,

        [TwitterTimeZone("America/Bogota", "Bogota")]
        Bogota,

        [TwitterTimeZone("America/Lima", "Lima")]
        Lima,

        [TwitterTimeZone("America/Lima", "Quito")]
        Quito,

        [TwitterTimeZone("America/Caracas", "Caracas")]
        Caracas,

        [TwitterTimeZone("America/La_Paz", "La Paz")]
        La_Paz,

        [TwitterTimeZone("America/Santiago", "Santiago")]
        Santiago,

        [TwitterTimeZone("America/St_Johns", "Newfoundland")]
        Newfoundland,

        [TwitterTimeZone("America/Sao_Paulo", "Brasilia")]
        Brasilia,

        [TwitterTimeZone("America/Argentina/Buenos_Aires", "Buenos Aires")]
        Buenos_Aires,

        [TwitterTimeZone("America/Montevideo", "Montevideo")]
        Montevideo,

        [TwitterTimeZone("America/Guyana", "Georgetown")]
        Georgetown,

        [TwitterTimeZone("America/Godthab", "Greenland")]
        Greenland,

        [TwitterTimeZone("Atlantic/South_Georgia", "Mid-Atlantic")]
        Mid_Atlantic,

        [TwitterTimeZone("Atlantic/Azores", "Azores")]
        Azores,

        [TwitterTimeZone("Atlantic/Cape_Verde", "Cape Verde Is.")]
        Cape_Verde_Island,

        [TwitterTimeZone("Europe/Dublin", "Dublin")]
        Dublin,

        [TwitterTimeZone("Europe/London", "Edinburgh")]
        Edinburgh,

        [TwitterTimeZone("Europe/Lisbon", "Lisbon")]
        Lisbon,

        [TwitterTimeZone("Europe/London", "London")]
        London,

        [TwitterTimeZone("Africa/Casablanca", "Casablanca")]
        Casablanca,

        [TwitterTimeZone("Africa/Monrovia", "Monrovia")]
        Monrovia,

        [TwitterTimeZone("Etc/UTC", "UTC")]
        UTC,

        [TwitterTimeZone("Europe/Belgrade", "Belgrade")]
        Belgrade,

        [TwitterTimeZone("Europe/Bratislava", "Bratislava")]
        Bratislava,

        [TwitterTimeZone("Europe/Budapest", "Budapest")]
        Budapest,

        [TwitterTimeZone("Europe/Ljubljana", "Ljubljana")]
        Ljubljana,

        [TwitterTimeZone("Europe/Prague", "Prague")]
        Prague,

        [TwitterTimeZone("Europe/Sarajevo", "Sarajevo")]
        Sarajevo,

        [TwitterTimeZone("Europe/Skopje", "Skopje")]
        Skopje,

        [TwitterTimeZone("Europe/Warsaw", "Warsaw")]
        Warsaw,

        [TwitterTimeZone("Europe/Zagreb", "Zagreb")]
        Zagreb,

        [TwitterTimeZone("Europe/Brussels", "Brussels")]
        Brussels,

        [TwitterTimeZone("Europe/Copenhagen", "Copenhagen")]
        Copenhagen,

        [TwitterTimeZone("Europe/Madrid", "Madrid")]
        Madrid,

        [TwitterTimeZone("Europe/Paris", "Paris")]
        Paris,

        [TwitterTimeZone("Europe/Amsterdam", "Amsterdam")]
        Amsterdam,

        [TwitterTimeZone("Europe/Berlin", "Berlin")]
        Berlin,

        [TwitterTimeZone("Europe/Berlin", "Bern")]
        Bern,

        [TwitterTimeZone("Europe/Rome", "Rome")]
        Rome,

        [TwitterTimeZone("Europe/Stockholm", "Stockholm")]
        Stockholm,

        [TwitterTimeZone("Europe/Vienna", "Vienna")]
        Vienna,

        [TwitterTimeZone("Africa/Algiers", "West Central Africa")]
        West_Central_Africa,

        [TwitterTimeZone("Europe/Bucharest", "Bucharest")]
        Bucharest,

        [TwitterTimeZone("Africa/Cairo", "Cairo")]
        Cairo,

        [TwitterTimeZone("Europe/Helsinki", "Helsinki")]
        Helsinki,

        [TwitterTimeZone("Europe/Kiev", "Kyiv")]
        Kyiv,

        [TwitterTimeZone("Europe/Riga", "Riga")]
        Riga,

        [TwitterTimeZone("Europe/Sofia", "Sofia")]
        Sofia,

        [TwitterTimeZone("Europe/Tallinn", "Tallinn")]
        Tallinn,

        [TwitterTimeZone("Europe/Vilnius", "Vilnius")]
        Vilnius,

        [TwitterTimeZone("Europe/Athens", "Athens")]
        Athens,

        [TwitterTimeZone("Europe/Istanbul", "Istanbul")]
        Istanbul,

        [TwitterTimeZone("Europe/Minsk", "Minsk")]
        Minsk,

        [TwitterTimeZone("Asia/Jerusalem", "Jerusalem")]
        Jerusalem,

        [TwitterTimeZone("Africa/Harare", "Harare")]
        Harare,

        [TwitterTimeZone("Africa/Johannesburg", "Pretoria")]
        Pretoria,

        [TwitterTimeZone("Europe/Kaliningrad", "Kaliningrad")]
        Kaliningrad,

        [TwitterTimeZone("Europe/Moscow", "Moscow")]
        Moscow,

        [TwitterTimeZone("Europe/Moscow", "St. Petersburg")]
        St_Petersburg,

        [TwitterTimeZone("Europe/Volgograd", "Volgograd")]
        Volgograd,

        [TwitterTimeZone("Europe/Samara", "Samara")]
        Samara,

        [TwitterTimeZone("Asia/Kuwait", "Kuwait")]
        Kuwait,

        [TwitterTimeZone("Asia/Riyadh", "Riyadh")]
        Riyadh,

        [TwitterTimeZone("Africa/Nairobi", "Nairobi")]
        Nairobi,

        [TwitterTimeZone("Asia/Baghdad", "Baghdad")]
        Baghdad,

        [TwitterTimeZone("Asia/Tehran", "Tehran")]
        Tehran,

        [TwitterTimeZone("Asia/Muscat", "Abu Dhabi")]
        Abu_Dhabi,

        [TwitterTimeZone("Asia/Muscat", "Muscat")]
        Muscat,

        [TwitterTimeZone("Asia/Baku", "Baku")]
        Baku,

        [TwitterTimeZone("Asia/Tbilisi", "Tbilisi")]
        Tbilisi,

        [TwitterTimeZone("Asia/Yerevan", "Yerevan")]
        Yerevan,

        [TwitterTimeZone("Asia/Kabul", "Kabul")]
        Kabul,

        [TwitterTimeZone("Asia/Yekaterinburg", "Ekaterinburg")]
        Ekaterinburg,

        [TwitterTimeZone("Asia/Karachi", "Islamabad")]
        Islamabad,

        [TwitterTimeZone("Asia/Karachi", "Karachi")]
        Karachi,

        [TwitterTimeZone("Asia/Tashkent", "Tashkent")]
        Tashkent,

        [TwitterTimeZone("Asia/Kolkata", "Chennai")]
        Chennai,

        [TwitterTimeZone("Asia/Kolkata", "Kolkata")]
        Kolkata,

        [TwitterTimeZone("Asia/Kolkata", "Mumbai")]
        Mumbai,

        [TwitterTimeZone("Asia/Kolkata", "New Delhi")]
        New_Delhi,

        [TwitterTimeZone("Asia/Kathmandu", "Kathmandu")]
        Kathmandu,

        [TwitterTimeZone("Asia/Dhaka", "Astana")]
        Astana,

        [TwitterTimeZone("Asia/Dhaka", "Dhaka")]
        Dhaka,

        [TwitterTimeZone("Asia/Colombo", "Sri Jayawardenepura")]
        Sri_Jayawardenepura,

        [TwitterTimeZone("Asia/Almaty", "Almaty")]
        Almaty,

        [TwitterTimeZone("Asia/Novosibirsk", "Novosibirsk")]
        Novosibirsk,

        [TwitterTimeZone("Asia/Rangoon", "Rangoon")]
        Rangoon,

        [TwitterTimeZone("Asia/Bangkok", "Bangkok")]
        Bangkok,

        [TwitterTimeZone("Asia/Bangkok", "Hanoi")]
        Hanoi,

        [TwitterTimeZone("Asia/Jakarta", "Jakarta")]
        Jakarta,

        [TwitterTimeZone("Asia/Krasnoyarsk", "Krasnoyarsk")]
        Krasnoyarsk,

        [TwitterTimeZone("Asia/Shanghai", "Beijing")]
        Beijing,

        [TwitterTimeZone("Asia/Chongqing", "Chongqing")]
        Chongqing,

        [TwitterTimeZone("Asia/Hong_Kong", "Hong Kong")]
        Hong_Kong,

        [TwitterTimeZone("Asia/Urumqi", "Urumqi")]
        Urumqi,

        [TwitterTimeZone("Asia/Kuala_Lumpur", "Kuala Lumpur")]
        Kuala_Lumpur,

        [TwitterTimeZone("Asia/Singapore", "Singapore")]
        Singapore,

        [TwitterTimeZone("Asia/Taipei", "Taipei")]
        Taipei,

        [TwitterTimeZone("Australia/Perth", "Perth")]
        Perth,

        [TwitterTimeZone("Asia/Irkutsk", "Irkutsk")]
        Irkutsk,

        [TwitterTimeZone("Asia/Ulaanbaatar", "Ulaanbaatar")]
        Ulaanbaatar,

        [TwitterTimeZone("Asia/Seoul", "Seoul")]
        Seoul,

        [TwitterTimeZone("Asia/Tokyo", "Osaka")]
        Osaka,

        [TwitterTimeZone("Asia/Tokyo", "Sapporo")]
        Sapporo,

        [TwitterTimeZone("Asia/Tokyo", "Tokyo")]
        Tokyo,

        [TwitterTimeZone("Asia/Yakutsk", "Yakutsk")]
        Yakutsk,

        [TwitterTimeZone("Australia/Darwin", "Darwin")]
        Darwin,

        [TwitterTimeZone("Australia/Adelaide", "Adelaide")]
        Adelaide,

        [TwitterTimeZone("Australia/Melbourne", "Canberra")]
        Canberra,

        [TwitterTimeZone("Australia/Melbourne", "Melbourne")]
        Melbourne,

        [TwitterTimeZone("Australia/Sydney", "Sydney")]
        Sydney,

        [TwitterTimeZone("Australia/Brisbane", "Brisbane")]
        Brisbane,

        [TwitterTimeZone("Australia/Hobart", "Hobart")]
        Hobart,

        [TwitterTimeZone("Asia/Vladivostok", "Vladivostok")]
        Vladivostok,

        [TwitterTimeZone("Pacific/Guam", "Guam")]
        Guam,

        [TwitterTimeZone("Pacific/Port_Moresby", "Port Moresby")]
        Port_Moresby,

        [TwitterTimeZone("Asia/Magadan", "Magadan")]
        Magadan,

        [TwitterTimeZone("Asia/Srednekolymsk", "Srednekolymsk")]
        Srednekolymsk,

        [TwitterTimeZone("Pacific/Guadalcanal", "Solomon Is.")]
        Solomon_Island,

        [TwitterTimeZone("Pacific/Noumea", "New Caledonia")]
        New_Caledonia,

        [TwitterTimeZone("Pacific/Fiji", "Fiji")]
        Fiji,

        [TwitterTimeZone("Asia/Kamchatka", "Kamchatka")]
        Kamchatka,

        [TwitterTimeZone("Pacific/Majuro", "Marshall Is.")]
        Marshall_Island,

        [TwitterTimeZone("Pacific/Auckland", "Auckland")]
        Auckland,

        [TwitterTimeZone("Pacific/Auckland", "Wellington")]
        Wellington,

        [TwitterTimeZone("Pacific/Tongatapu", "Nuku'alofa")]
        Nuku_alofa,

        [TwitterTimeZone("Pacific/Fakaofo", "Tokelau Is.")]
        Tokelau_Island,

        [TwitterTimeZone("Pacific/Chatham", "Chatham Is.")]
        Chatham_Island,

        [TwitterTimeZone("Pacific/Apia", "Samoa")]
        Samoa,


    }

}

namespace Tweetinvi.Core.Enum.Internal.Generators
{
    // GENERATED FROM
    internal class TwitterTimeZoneGenerator
    {
        internal static void PrintTwitterTimeZone()
        {
            // ReSharper disable once ConvertToConstant.Local
            string rubyOnRailsDocumentation = "{\"International Date Line West\"=>\"Pacific/Midway\",\"Midway Island\"=>\"Pacific/Midway\",\"American Samoa\"=>\"Pacific/Pago_Pago\",\"Hawaii\"=>\"Pacific/Honolulu\",\"Alaska\"=>\"America/Juneau\",\"Pacific Time (US & Canada)\"=>\"America/Los_Angeles\",\"Tijuana\"=>\"America/Tijuana\",\"Mountain Time (US & Canada)\"=>\"America/Denver\",\"Arizona\"=>\"America/Phoenix\",\"Chihuahua\"=>\"America/Chihuahua\",\"Mazatlan\"=>\"America/Mazatlan\",\"Central Time (US & Canada)\"=>\"America/Chicago\",\"Saskatchewan\"=>\"America/Regina\",\"Guadalajara\"=>\"America/Mexico_City\",\"Mexico City\"=>\"America/Mexico_City\",\"Monterrey\"=>\"America/Monterrey\",\"Central America\"=>\"America/Guatemala\",\"Eastern Time (US & Canada)\"=>\"America/New_York\",\"Indiana (East)\"=>\"America/Indiana/Indianapolis\",\"Bogota\"=>\"America/Bogota\",\"Lima\"=>\"America/Lima\",\"Quito\"=>\"America/Lima\",\"Atlantic Time (Canada)\"=>\"America/Halifax\",\"Caracas\"=>\"America/Caracas\",\"La Paz\"=>\"America/La_Paz\",\"Santiago\"=>\"America/Santiago\",\"Newfoundland\"=>\"America/St_Johns\",\"Brasilia\"=>\"America/Sao_Paulo\",\"Buenos Aires\"=>\"America/Argentina/Buenos_Aires\",\"Montevideo\"=>\"America/Montevideo\",\"Georgetown\"=>\"America/Guyana\",\"Greenland\"=>\"America/Godthab\",\"Mid-Atlantic\"=>\"Atlantic/South_Georgia\",\"Azores\"=>\"Atlantic/Azores\",\"Cape Verde Is.\"=>\"Atlantic/Cape_Verde\",\"Dublin\"=>\"Europe/Dublin\",\"Edinburgh\"=>\"Europe/London\",\"Lisbon\"=>\"Europe/Lisbon\",\"London\"=>\"Europe/London\",\"Casablanca\"=>\"Africa/Casablanca\",\"Monrovia\"=>\"Africa/Monrovia\",\"UTC\"=>\"Etc/UTC\",\"Belgrade\"=>\"Europe/Belgrade\",\"Bratislava\"=>\"Europe/Bratislava\",\"Budapest\"=>\"Europe/Budapest\",\"Ljubljana\"=>\"Europe/Ljubljana\",\"Prague\"=>\"Europe/Prague\",\"Sarajevo\"=>\"Europe/Sarajevo\",\"Skopje\"=>\"Europe/Skopje\",\"Warsaw\"=>\"Europe/Warsaw\",\"Zagreb\"=>\"Europe/Zagreb\",\"Brussels\"=>\"Europe/Brussels\",\"Copenhagen\"=>\"Europe/Copenhagen\",\"Madrid\"=>\"Europe/Madrid\",\"Paris\"=>\"Europe/Paris\",\"Amsterdam\"=>\"Europe/Amsterdam\",\"Berlin\"=>\"Europe/Berlin\",\"Bern\"=>\"Europe/Berlin\",\"Rome\"=>\"Europe/Rome\",\"Stockholm\"=>\"Europe/Stockholm\",\"Vienna\"=>\"Europe/Vienna\",\"West Central Africa\"=>\"Africa/Algiers\",\"Bucharest\"=>\"Europe/Bucharest\",\"Cairo\"=>\"Africa/Cairo\",\"Helsinki\"=>\"Europe/Helsinki\",\"Kyiv\"=>\"Europe/Kiev\",\"Riga\"=>\"Europe/Riga\",\"Sofia\"=>\"Europe/Sofia\",\"Tallinn\"=>\"Europe/Tallinn\",\"Vilnius\"=>\"Europe/Vilnius\",\"Athens\"=>\"Europe/Athens\",\"Istanbul\"=>\"Europe/Istanbul\",\"Minsk\"=>\"Europe/Minsk\",\"Jerusalem\"=>\"Asia/Jerusalem\",\"Harare\"=>\"Africa/Harare\",\"Pretoria\"=>\"Africa/Johannesburg\",\"Kaliningrad\"=>\"Europe/Kaliningrad\",\"Moscow\"=>\"Europe/Moscow\",\"St. Petersburg\"=>\"Europe/Moscow\",\"Volgograd\"=>\"Europe/Volgograd\",\"Samara\"=>\"Europe/Samara\",\"Kuwait\"=>\"Asia/Kuwait\",\"Riyadh\"=>\"Asia/Riyadh\",\"Nairobi\"=>\"Africa/Nairobi\",\"Baghdad\"=>\"Asia/Baghdad\",\"Tehran\"=>\"Asia/Tehran\",\"Abu Dhabi\"=>\"Asia/Muscat\",\"Muscat\"=>\"Asia/Muscat\",\"Baku\"=>\"Asia/Baku\",\"Tbilisi\"=>\"Asia/Tbilisi\",\"Yerevan\"=>\"Asia/Yerevan\",\"Kabul\"=>\"Asia/Kabul\",\"Ekaterinburg\"=>\"Asia/Yekaterinburg\",\"Islamabad\"=>\"Asia/Karachi\",\"Karachi\"=>\"Asia/Karachi\",\"Tashkent\"=>\"Asia/Tashkent\",\"Chennai\"=>\"Asia/Kolkata\",\"Kolkata\"=>\"Asia/Kolkata\",\"Mumbai\"=>\"Asia/Kolkata\",\"New Delhi\"=>\"Asia/Kolkata\",\"Kathmandu\"=>\"Asia/Kathmandu\",\"Astana\"=>\"Asia/Dhaka\",\"Dhaka\"=>\"Asia/Dhaka\",\"Sri Jayawardenepura\"=>\"Asia/Colombo\",\"Almaty\"=>\"Asia/Almaty\",\"Novosibirsk\"=>\"Asia/Novosibirsk\",\"Rangoon\"=>\"Asia/Rangoon\",\"Bangkok\"=>\"Asia/Bangkok\",\"Hanoi\"=>\"Asia/Bangkok\",\"Jakarta\"=>\"Asia/Jakarta\",\"Krasnoyarsk\"=>\"Asia/Krasnoyarsk\",\"Beijing\"=>\"Asia/Shanghai\",\"Chongqing\"=>\"Asia/Chongqing\",\"Hong Kong\"=>\"Asia/Hong_Kong\",\"Urumqi\"=>\"Asia/Urumqi\",\"Kuala Lumpur\"=>\"Asia/Kuala_Lumpur\",\"Singapore\"=>\"Asia/Singapore\",\"Taipei\"=>\"Asia/Taipei\",\"Perth\"=>\"Australia/Perth\",\"Irkutsk\"=>\"Asia/Irkutsk\",\"Ulaanbaatar\"=>\"Asia/Ulaanbaatar\",\"Seoul\"=>\"Asia/Seoul\",\"Osaka\"=>\"Asia/Tokyo\",\"Sapporo\"=>\"Asia/Tokyo\",\"Tokyo\"=>\"Asia/Tokyo\",\"Yakutsk\"=>\"Asia/Yakutsk\",\"Darwin\"=>\"Australia/Darwin\",\"Adelaide\"=>\"Australia/Adelaide\",\"Canberra\"=>\"Australia/Melbourne\",\"Melbourne\"=>\"Australia/Melbourne\",\"Sydney\"=>\"Australia/Sydney\",\"Brisbane\"=>\"Australia/Brisbane\",\"Hobart\"=>\"Australia/Hobart\",\"Vladivostok\"=>\"Asia/Vladivostok\",\"Guam\"=>\"Pacific/Guam\",\"Port Moresby\"=>\"Pacific/Port_Moresby\",\"Magadan\"=>\"Asia/Magadan\",\"Srednekolymsk\"=>\"Asia/Srednekolymsk\",\"Solomon Is.\"=>\"Pacific/Guadalcanal\",\"New Caledonia\"=>\"Pacific/Noumea\",\"Fiji\"=>\"Pacific/Fiji\",\"Kamchatka\"=>\"Asia/Kamchatka\",\"Marshall Is.\"=>\"Pacific/Majuro\",\"Auckland\"=>\"Pacific/Auckland\",\"Wellington\"=>\"Pacific/Auckland\",\"Nuku'alofa\"=>\"Pacific/Tongatapu\",\"Tokelau Is.\"=>\"Pacific/Fakaofo\",\"Chatham Is.\"=>\"Pacific/Chatham\",\"Samoa\"=>\"Pacific/Apia\"}";

            var getInfoRegex = new Regex("\"(?<description>\\w(?:\\w|\\-|\\s|\\.|\\')*)\"=>\"(?<tzinfo>\\w+(?:/\\w+)+)\"");
            var results = getInfoRegex.Matches(rubyOnRailsDocumentation);

            foreach (var timeZoneInfos in results.OfType<Match>())
            {
                var initialDescription = timeZoneInfos.Groups["description"].Value;
                var description = initialDescription.Replace(" ", "_");
                description = description.Replace("'", "_");
                description = description.Replace("-", "_");
                description = description.Replace("Is.", "Island");

                var tzinfo = timeZoneInfos.Groups["tzinfo"].Value;

                var flags = string.Format("[TwitterTimeZone(\"{0}\", \"{1}\")]\r\n", tzinfo, initialDescription);
                var name = string.Format("{0},\r\n", description);

                Debug.WriteLine("{0}{1}", flags, name);
            }
        }
    }
}