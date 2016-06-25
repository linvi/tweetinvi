using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Models
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// As described on http://api.rubyonrails.org/classes/ActiveSupport/TimeZone.html
    /// </summary>
    public enum TimeZoneFromTwitter
    {
        [TimeZoneFromTwitter("Pacific/Midway", "International Date Line West")]
        International_Date_Line_West,

        [TimeZoneFromTwitter("Pacific/Midway", "Midway Island")]
        Midway_Island,

        [TimeZoneFromTwitter("Pacific/Pago_Pago", "American Samoa")]
        American_Samoa,

        [TimeZoneFromTwitter("Pacific/Honolulu", "Hawaii")]
        Hawaii,

        [TimeZoneFromTwitter("America/Juneau", "Alaska")]
        Alaska,

        [TimeZoneFromTwitter("America/Tijuana", "Tijuana")]
        Tijuana,

        [TimeZoneFromTwitter("America/Phoenix", "Arizona")]
        Arizona,

        [TimeZoneFromTwitter("America/Chihuahua", "Chihuahua")]
        Chihuahua,

        [TimeZoneFromTwitter("America/Mazatlan", "Mazatlan")]
        Mazatlan,

        [TimeZoneFromTwitter("America/Regina", "Saskatchewan")]
        Saskatchewan,

        [TimeZoneFromTwitter("America/Mexico_City", "Guadalajara")]
        Guadalajara,

        [TimeZoneFromTwitter("America/Mexico_City", "Mexico City")]
        Mexico_City,

        [TimeZoneFromTwitter("America/Monterrey", "Monterrey")]
        Monterrey,

        [TimeZoneFromTwitter("America/Guatemala", "Central America")]
        Central_America,

        [TimeZoneFromTwitter("America/Bogota", "Bogota")]
        Bogota,

        [TimeZoneFromTwitter("America/Lima", "Lima")]
        Lima,

        [TimeZoneFromTwitter("America/Lima", "Quito")]
        Quito,

        [TimeZoneFromTwitter("America/Caracas", "Caracas")]
        Caracas,

        [TimeZoneFromTwitter("America/La_Paz", "La Paz")]
        La_Paz,

        [TimeZoneFromTwitter("America/Santiago", "Santiago")]
        Santiago,

        [TimeZoneFromTwitter("America/St_Johns", "Newfoundland")]
        Newfoundland,

        [TimeZoneFromTwitter("America/Sao_Paulo", "Brasilia")]
        Brasilia,

        [TimeZoneFromTwitter("America/Argentina/Buenos_Aires", "Buenos Aires")]
        Buenos_Aires,

        [TimeZoneFromTwitter("America/Montevideo", "Montevideo")]
        Montevideo,

        [TimeZoneFromTwitter("America/Guyana", "Georgetown")]
        Georgetown,

        [TimeZoneFromTwitter("America/Godthab", "Greenland")]
        Greenland,

        [TimeZoneFromTwitter("Atlantic/South_Georgia", "Mid-Atlantic")]
        Mid_Atlantic,

        [TimeZoneFromTwitter("Atlantic/Azores", "Azores")]
        Azores,

        [TimeZoneFromTwitter("Atlantic/Cape_Verde", "Cape Verde Is.")]
        Cape_Verde_Island,

        [TimeZoneFromTwitter("Europe/Dublin", "Dublin")]
        Dublin,

        [TimeZoneFromTwitter("Europe/London", "Edinburgh")]
        Edinburgh,

        [TimeZoneFromTwitter("Europe/Lisbon", "Lisbon")]
        Lisbon,

        [TimeZoneFromTwitter("Europe/London", "London")]
        London,

        [TimeZoneFromTwitter("Africa/Casablanca", "Casablanca")]
        Casablanca,

        [TimeZoneFromTwitter("Africa/Monrovia", "Monrovia")]
        Monrovia,

        [TimeZoneFromTwitter("Etc/UTC", "UTC")]
        UTC,

        [TimeZoneFromTwitter("Europe/Belgrade", "Belgrade")]
        Belgrade,

        [TimeZoneFromTwitter("Europe/Bratislava", "Bratislava")]
        Bratislava,

        [TimeZoneFromTwitter("Europe/Budapest", "Budapest")]
        Budapest,

        [TimeZoneFromTwitter("Europe/Ljubljana", "Ljubljana")]
        Ljubljana,

        [TimeZoneFromTwitter("Europe/Prague", "Prague")]
        Prague,

        [TimeZoneFromTwitter("Europe/Sarajevo", "Sarajevo")]
        Sarajevo,

        [TimeZoneFromTwitter("Europe/Skopje", "Skopje")]
        Skopje,

        [TimeZoneFromTwitter("Europe/Warsaw", "Warsaw")]
        Warsaw,

        [TimeZoneFromTwitter("Europe/Zagreb", "Zagreb")]
        Zagreb,

        [TimeZoneFromTwitter("Europe/Brussels", "Brussels")]
        Brussels,

        [TimeZoneFromTwitter("Europe/Copenhagen", "Copenhagen")]
        Copenhagen,

        [TimeZoneFromTwitter("Europe/Madrid", "Madrid")]
        Madrid,

        [TimeZoneFromTwitter("Europe/Paris", "Paris")]
        Paris,

        [TimeZoneFromTwitter("Europe/Amsterdam", "Amsterdam")]
        Amsterdam,

        [TimeZoneFromTwitter("Europe/Berlin", "Berlin")]
        Berlin,

        [TimeZoneFromTwitter("Europe/Berlin", "Bern")]
        Bern,

        [TimeZoneFromTwitter("Europe/Rome", "Rome")]
        Rome,

        [TimeZoneFromTwitter("Europe/Stockholm", "Stockholm")]
        Stockholm,

        [TimeZoneFromTwitter("Europe/Vienna", "Vienna")]
        Vienna,

        [TimeZoneFromTwitter("Africa/Algiers", "West Central Africa")]
        West_Central_Africa,

        [TimeZoneFromTwitter("Europe/Bucharest", "Bucharest")]
        Bucharest,

        [TimeZoneFromTwitter("Africa/Cairo", "Cairo")]
        Cairo,

        [TimeZoneFromTwitter("Europe/Helsinki", "Helsinki")]
        Helsinki,

        [TimeZoneFromTwitter("Europe/Kiev", "Kyiv")]
        Kyiv,

        [TimeZoneFromTwitter("Europe/Riga", "Riga")]
        Riga,

        [TimeZoneFromTwitter("Europe/Sofia", "Sofia")]
        Sofia,

        [TimeZoneFromTwitter("Europe/Tallinn", "Tallinn")]
        Tallinn,

        [TimeZoneFromTwitter("Europe/Vilnius", "Vilnius")]
        Vilnius,

        [TimeZoneFromTwitter("Europe/Athens", "Athens")]
        Athens,

        [TimeZoneFromTwitter("Europe/Istanbul", "Istanbul")]
        Istanbul,

        [TimeZoneFromTwitter("Europe/Minsk", "Minsk")]
        Minsk,

        [TimeZoneFromTwitter("Asia/Jerusalem", "Jerusalem")]
        Jerusalem,

        [TimeZoneFromTwitter("Africa/Harare", "Harare")]
        Harare,

        [TimeZoneFromTwitter("Africa/Johannesburg", "Pretoria")]
        Pretoria,

        [TimeZoneFromTwitter("Europe/Kaliningrad", "Kaliningrad")]
        Kaliningrad,

        [TimeZoneFromTwitter("Europe/Moscow", "Moscow")]
        Moscow,

        [TimeZoneFromTwitter("Europe/Moscow", "St. Petersburg")]
        St_Petersburg,

        [TimeZoneFromTwitter("Europe/Volgograd", "Volgograd")]
        Volgograd,

        [TimeZoneFromTwitter("Europe/Samara", "Samara")]
        Samara,

        [TimeZoneFromTwitter("Asia/Kuwait", "Kuwait")]
        Kuwait,

        [TimeZoneFromTwitter("Asia/Riyadh", "Riyadh")]
        Riyadh,

        [TimeZoneFromTwitter("Africa/Nairobi", "Nairobi")]
        Nairobi,

        [TimeZoneFromTwitter("Asia/Baghdad", "Baghdad")]
        Baghdad,

        [TimeZoneFromTwitter("Asia/Tehran", "Tehran")]
        Tehran,

        [TimeZoneFromTwitter("Asia/Muscat", "Abu Dhabi")]
        Abu_Dhabi,

        [TimeZoneFromTwitter("Asia/Muscat", "Muscat")]
        Muscat,

        [TimeZoneFromTwitter("Asia/Baku", "Baku")]
        Baku,

        [TimeZoneFromTwitter("Asia/Tbilisi", "Tbilisi")]
        Tbilisi,

        [TimeZoneFromTwitter("Asia/Yerevan", "Yerevan")]
        Yerevan,

        [TimeZoneFromTwitter("Asia/Kabul", "Kabul")]
        Kabul,

        [TimeZoneFromTwitter("Asia/Yekaterinburg", "Ekaterinburg")]
        Ekaterinburg,

        [TimeZoneFromTwitter("Asia/Karachi", "Islamabad")]
        Islamabad,

        [TimeZoneFromTwitter("Asia/Karachi", "Karachi")]
        Karachi,

        [TimeZoneFromTwitter("Asia/Tashkent", "Tashkent")]
        Tashkent,

        [TimeZoneFromTwitter("Asia/Kolkata", "Chennai")]
        Chennai,

        [TimeZoneFromTwitter("Asia/Kolkata", "Kolkata")]
        Kolkata,

        [TimeZoneFromTwitter("Asia/Kolkata", "Mumbai")]
        Mumbai,

        [TimeZoneFromTwitter("Asia/Kolkata", "New Delhi")]
        New_Delhi,

        [TimeZoneFromTwitter("Asia/Kathmandu", "Kathmandu")]
        Kathmandu,

        [TimeZoneFromTwitter("Asia/Dhaka", "Astana")]
        Astana,

        [TimeZoneFromTwitter("Asia/Dhaka", "Dhaka")]
        Dhaka,

        [TimeZoneFromTwitter("Asia/Colombo", "Sri Jayawardenepura")]
        Sri_Jayawardenepura,

        [TimeZoneFromTwitter("Asia/Almaty", "Almaty")]
        Almaty,

        [TimeZoneFromTwitter("Asia/Novosibirsk", "Novosibirsk")]
        Novosibirsk,

        [TimeZoneFromTwitter("Asia/Rangoon", "Rangoon")]
        Rangoon,

        [TimeZoneFromTwitter("Asia/Bangkok", "Bangkok")]
        Bangkok,

        [TimeZoneFromTwitter("Asia/Bangkok", "Hanoi")]
        Hanoi,

        [TimeZoneFromTwitter("Asia/Jakarta", "Jakarta")]
        Jakarta,

        [TimeZoneFromTwitter("Asia/Krasnoyarsk", "Krasnoyarsk")]
        Krasnoyarsk,

        [TimeZoneFromTwitter("Asia/Shanghai", "Beijing")]
        Beijing,

        [TimeZoneFromTwitter("Asia/Chongqing", "Chongqing")]
        Chongqing,

        [TimeZoneFromTwitter("Asia/Hong_Kong", "Hong Kong")]
        Hong_Kong,

        [TimeZoneFromTwitter("Asia/Urumqi", "Urumqi")]
        Urumqi,

        [TimeZoneFromTwitter("Asia/Kuala_Lumpur", "Kuala Lumpur")]
        Kuala_Lumpur,

        [TimeZoneFromTwitter("Asia/Singapore", "Singapore")]
        Singapore,

        [TimeZoneFromTwitter("Asia/Taipei", "Taipei")]
        Taipei,

        [TimeZoneFromTwitter("Australia/Perth", "Perth")]
        Perth,

        [TimeZoneFromTwitter("Asia/Irkutsk", "Irkutsk")]
        Irkutsk,

        [TimeZoneFromTwitter("Asia/Ulaanbaatar", "Ulaanbaatar")]
        Ulaanbaatar,

        [TimeZoneFromTwitter("Asia/Seoul", "Seoul")]
        Seoul,

        [TimeZoneFromTwitter("Asia/Tokyo", "Osaka")]
        Osaka,

        [TimeZoneFromTwitter("Asia/Tokyo", "Sapporo")]
        Sapporo,

        [TimeZoneFromTwitter("Asia/Tokyo", "Tokyo")]
        Tokyo,

        [TimeZoneFromTwitter("Asia/Yakutsk", "Yakutsk")]
        Yakutsk,

        [TimeZoneFromTwitter("Australia/Darwin", "Darwin")]
        Darwin,

        [TimeZoneFromTwitter("Australia/Adelaide", "Adelaide")]
        Adelaide,

        [TimeZoneFromTwitter("Australia/Melbourne", "Canberra")]
        Canberra,

        [TimeZoneFromTwitter("Australia/Melbourne", "Melbourne")]
        Melbourne,

        [TimeZoneFromTwitter("Australia/Sydney", "Sydney")]
        Sydney,

        [TimeZoneFromTwitter("Australia/Brisbane", "Brisbane")]
        Brisbane,

        [TimeZoneFromTwitter("Australia/Hobart", "Hobart")]
        Hobart,

        [TimeZoneFromTwitter("Asia/Vladivostok", "Vladivostok")]
        Vladivostok,

        [TimeZoneFromTwitter("Pacific/Guam", "Guam")]
        Guam,

        [TimeZoneFromTwitter("Pacific/Port_Moresby", "Port Moresby")]
        Port_Moresby,

        [TimeZoneFromTwitter("Asia/Magadan", "Magadan")]
        Magadan,

        [TimeZoneFromTwitter("Asia/Srednekolymsk", "Srednekolymsk")]
        Srednekolymsk,

        [TimeZoneFromTwitter("Pacific/Guadalcanal", "Solomon Is.")]
        Solomon_Island,

        [TimeZoneFromTwitter("Pacific/Noumea", "New Caledonia")]
        New_Caledonia,

        [TimeZoneFromTwitter("Pacific/Fiji", "Fiji")]
        Fiji,

        [TimeZoneFromTwitter("Asia/Kamchatka", "Kamchatka")]
        Kamchatka,

        [TimeZoneFromTwitter("Pacific/Majuro", "Marshall Is.")]
        Marshall_Island,

        [TimeZoneFromTwitter("Pacific/Auckland", "Auckland")]
        Auckland,

        [TimeZoneFromTwitter("Pacific/Auckland", "Wellington")]
        Wellington,

        [TimeZoneFromTwitter("Pacific/Tongatapu", "Nuku'alofa")]
        Nuku_alofa,

        [TimeZoneFromTwitter("Pacific/Fakaofo", "Tokelau Is.")]
        Tokelau_Island,

        [TimeZoneFromTwitter("Pacific/Chatham", "Chatham Is.")]
        Chatham_Island,

        [TimeZoneFromTwitter("Pacific/Apia", "Samoa")]
        Samoa,


    }

    // GENERATED FROM
    internal class TimeZoneFromTwitterGenerator
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