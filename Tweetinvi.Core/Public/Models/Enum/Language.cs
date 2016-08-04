﻿using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Languages available on Twitter. Some codes returned by Twitter are not documented and will return Undefined.
    /// If a language code is not supported but you know which language it represents, please to open an issue.
    /// </summary>
    public enum Language
    {
        [Language("Undefined", "undefined", "xx")] Undefined = 0,
        [Language("Highland Popoluca", "poi")] HighlandPopoluca,
        [Language("Abkhaz", "ab")] Abkhaz,
        [Language("Afar", "aa")] Afar,
        [Language("Afrikaans", "af")] Afrikaans,
        [Language("Akan", "ak")] Akan,
        [Language("Albanian", "sq")] Albanian,
        [Language("Amharic", "am")] Amharic,
        [Language("Arabic", "ar")] Arabic,
        [Language("Aragonese", "an")] Aragonese,
        [Language("Armenian", "hy")] Armenian,
        [Language("Assamese", "as")] Assamese,
        [Language("Avar", "av")] Avaric,
        [Language("Avestan", "ae")] Avestan,
        [Language("Aymara", "ay")] Aymara,
        [Language("Azerbaijani", "az")] Azerbaijani,
        [Language("Bambara", "bm")] Bambara,
        [Language("Bashkir", "ba")] Bashkir,
        [Language("Basque", "eu")] Basque,
        [Language("Belarusian", "be")] Belarusian,
        [Language("Bengali", "bn")] Bengali,
        [Language("Bihari", "bh")] Bihari,
        [Language("Bislama", "bi")] Bislama,
        [Language("Bosnian", "bs")] Bosnian,
        [Language("Breton", "br")] Breton,
        [Language("Bulgarian", "bg")] Bulgarian,
        [Language("Burmese", "my")] Burmese,
        [Language("Catalan", "ca")] Catalan,
        [Language("Chamorro", "ch")] Chamorro,
        [Language("Chechen", "ce")] Chechen,
        [Language("Chewa", "ny")] Chewa,
        [Language("Chinese", "zh")] Chinese,
        [Language("Chuvash", "cv")] Chuvash,
        [Language("Cornish", "kw")] Cornish,
        [Language("Corsican", "co")] Corsican,
        [Language("Cree", "cr")] Cree,
        [Language("Croatian", "hr")] Croatian,
        [Language("Czech", "cs")] Czech,
        [Language("Danish", "da")] Danish,
        [Language("Maldivian", "dv")] Maldivian,
        [Language("Dutch", "nl")] Dutch,
        [Language("Dzongkha", "dz")] Dzongkha,
        [Language("English", "en")] English,
        [Language("Esperanto", "eo")] Esperanto,
        [Language("Estonian", "et")] Estonian,
        [Language("Ewe", "ee")] Ewe,
        [Language("Faroese", "fo")] Faroese,
        [Language("Fijian", "fj")] Fijian,
        [Language("Finnish", "fi")] Finnish,
        [Language("French", "fr")] French,
        [Language("Fula", "ff")] Fula,
        [Language("Galician", "gl")] Galician,
        [Language("Georgian", "ka")] Georgian,
        [Language("German", "de")] German,
        [Language("Greek", "el")] Greek,
        [Language("Guarani", "gn")] Guarani,
        [Language("Gujarati", "gu")] Gujarati,
        [Language("Haitian Creole", "ht")] HaitianCreole,
        [Language("Hausa", "ha")] Hausa,
        [Language("Hebrew", "he", "iw")] Hebrew,
        [Language("Herero", "hz")] Herero,
        [Language("Hindi", "hi")] Hindi,
        [Language("Hiri Motu", "ho")] HiriMotu,
        [Language("Hungarian", "hu")] Hungarian,
        [Language("Interlingua", "ia")] Interlingua,
        [Language("Indonesian", "id", "in")] Indonesian,
        [Language("Interlingue", "ie")] Interlingue,
        [Language("Irish", "ga")] Irish,
        [Language("Igbo", "ig")] Igbo,
        [Language("Inupiat", "ik")] Inupiat,
        [Language("Ido", "io")] Ido,
        [Language("Icelandic", "is")] Icelandic,
        [Language("Italian", "it")] Italian,
        [Language("Inuktitut", "iu")] Inuktitut,
        [Language("Japanese", "ja", "jp")] Japanese,
        [Language("Javanese", "jv")] Javanese,
        [Language("Greenlandic", "kl")] Greenlandic,
        [Language("Kannada", "kn")] Kannada,
        [Language("Kanuri", "kr")] Kanuri,
        [Language("Kashmiri", "ks")] Kashmiri,
        [Language("Kazakh", "kk")] Kazakh,
        [Language("Khmer", "km")] Khmer,
        [Language("Kikuyu", "ki")] Kikuyu,
        [Language("Kinyarwanda", "rw")] Kinyarwanda,
        [Language("Kyrgyz", "ky")] Kyrgyz,
        [Language("Komi", "kv")] Komi,
        [Language("Kongo", "kg")] Kongo,
        [Language("Korean", "ko")] Korean,
        [Language("Kurdish", "ku")] Kurdish,
        [Language("Kwanyama", "kj")] Kwanyama,
        [Language("Latin", "la")] Latin,
        [Language("Luxembourgish", "lb")] Luxembourgish,
        [Language("Luganda", "lg")] Luganda,
        [Language("Limburgish", "li")] Limburgish,
        [Language("Lingala", "ln")] Lingala,
        [Language("Lao", "lo")] Lao,
        [Language("Lithuanian", "lt")] Lithuanian,
        [Language("Luba-Katanga", "lu")] LubaKatanga,
        [Language("Latvian", "lv")] Latvian,
        [Language("Manx", "gv")] Manx,
        [Language("Macedonian", "mk")] Macedonian,
        [Language("Malagasy", "mg")] Malagasy,
        [Language("Malay", "ms")] Malay,
        [Language("Malayalam", "ml")] Malayalam,
        [Language("Maltese", "mt")] Maltese,
        [Language("Māori", "mi")] Maori,
        [Language("Marathi", "mr")] Marathi,
        [Language("Marshallese", "mh")] Marshallese,
        [Language("Mongolian", "mn")] Mongolian,
        [Language("Nauruan", "na")] Nauruan,
        [Language("Navajo", "nv")] Navajo,
        [Language("Norwegian Bokmål", "nb")] NorwegianBokmal,
        [Language("Zimbabwean Ndebele", "nd")] ZimbabweanNdebele,
        [Language("Nepali", "ne")] Nepali,
        [Language("Ndonga", "ng")] Ndonga,
        [Language("Norwegian Nynorsk", "nn")] NorwegianNynorsk,
        [Language("Norwegian", "no")] Norwegian,
        [Language("Nuosu", "ii")] Nuosu,
        [Language("SouthernNdebele", "nr")] SouthernNdebele,
        [Language("Occitan", "oc")] Occitan,
        [Language("Ojibwe", "oj")] Ojibwe,
        [Language("Church Slavonic", "cu")] ChurchSlavonic,
        [Language("Oromo", "om")] Oromo,
        [Language("Odia", "or")] Odia,
        [Language("Ossetian", "os")] Ossetian,
        [Language("Punjabi", "pa")] Punjabi,
        [Language("Pali", "pi")] Pali,
        [Language("Persian", "fa")] Persian,
        [Language("Polish", "pl")] Polish,
        [Language("Pashto", "ps")] Pashto,
        [Language("Portuguese", "pt")] Portuguese,
        [Language("Quechua", "qu")] Quechua,
        [Language("Romansh", "rm")] Romansh,
        [Language("Kirundi", "rn")] Kirundi,
        [Language("Romanian", "ro")] Romanian,
        [Language("Russian", "ru")] Russian,
        [Language("Sanskrit", "sa")] Sanskrit,
        [Language("Sardinian", "sc")] Sardinian,
        [Language("Sindhi", "sd")] Sindhi,
        [Language("Northern Sami", "se")] NorthernSami,
        [Language("Samoan", "sm")] Samoan,
        [Language("Sango", "sg")] Sango,
        [Language("Serbian", "sr")] Serbian,
        [Language("Scottish Gaelic", "gd")] ScottishGaelic,
        [Language("Shona", "sn")] Shona,
        [Language("Sinhalese", "si")] Sinhalese,
        [Language("Slovak", "sk")] Slovak,
        [Language("Slovene", "sl")] Slovene,
        [Language("Somali", "so")] Somali,
        [Language("Sotho", "st")] Sotho,
        [Language("Spanish", "es")] Spanish,
        [Language("Sundanese", "su")] Sundanese,
        [Language("Swahili", "sw")] Swahili,
        [Language("Swazi", "ss")] Swazi,
        [Language("Swedish", "sv")] Swedish,
        [Language("Tamil", "ta")] Tamil,
        [Language("Telugu", "te")] Telugu,
        [Language("Tajik", "tg")] Tajik,
        [Language("Thai", "th")] Thai,
        [Language("Tigrinya", "ti")] Tigrinya,
        [Language("Tibetan", "bo")] Tibetan,
        [Language("Turkmen", "tk")] Turkmen,
        [Language("Tagalog", "tl")] Tagalog,
        [Language("Tswana", "tn")] Tswana,
        [Language("Tonga", "to")] Tonga,
        [Language("Turkish", "tr")] Turkish,
        [Language("Tsonga", "ts")] Tsonga,
        [Language("Tatar", "tt")] Tatar,
        [Language("Twi", "tw")] Twi,
        [Language("Tahitian", "ty")] Tahitian,
        [Language("Uyghur", "ug")] Uyghur,
        [Language("Ukrainian", "uk")] Ukrainian,
        [Language("Urdu", "ur")] Urdu,
        [Language("Uzbek", "uz")] Uzbek,
        [Language("Venda", "ve")] Venda,
        [Language("Vietnamese", "vi")] Vietnamese,
        [Language("Volapük", "vo")] Volapuk,
        [Language("Walloon", "wa")] Walloon,
        [Language("Welsh", "cy")] Welsh,
        [Language("Wolof", "wo")] Wolof,
        [Language("Western Frisian", "fy")] WesternFrisian,
        [Language("Xhosa", "xh")] Xhosa,
        [Language("Yiddish", "yi")] Yiddish,
        [Language("Yoruba", "yo")] Yoruba,
        [Language("Zhuang", "za")] Zhuang,
        [Language("Zulu", "zu")] Zulu,

        [Language("Not Referenced", "un")] UN_NotReferenced
    }
}