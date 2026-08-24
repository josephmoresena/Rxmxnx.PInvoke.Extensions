# Enums

## `Iso639P1`

ISO 639-1 two-letter language codes, plus `Iv` (0x00) for the .NET invariant culture.

`NativeUtilities.GetIso639P1(CultureInfo)` and `NativeUtilities.UserInterfaceIso639P1` map `CultureInfo` onto this enum. The library uses it for localized exception messages (English, Arabic, Chinese, French, German, Italian, Japanese, Portuguese, Russian, Spanish).

| Code | Value | Language |
|------|-------|----------------------------|
| Iv   | 0x00  | .NET Invariant |
| Ab   | 0x01  | Abkhazian |
| Aa   | 0x02  | Afar |
| Af   | 0x03  | Afrikaans |
| Ak   | 0x04  | Akan |
| Sq   | 0x05  | Albanian |
| Am   | 0x06  | Amharic |
| Ar   | 0x07  | Arabic |
| An   | 0x08  | Aragonese |
| Hy   | 0x09  | Armenian |
| As   | 0x0A  | Assamese |
| Av   | 0x0B  | Avaric |
| Ae   | 0x0C  | Avestan |
| Ay   | 0x0D  | Aymara |
| Az   | 0x0E  | Azerbaijani |
| Bm   | 0x0F  | Bambara |
| Ba   | 0x10  | Bashkir |
| Eu   | 0x11  | Basque |
| Be   | 0x12  | Belarusian |
| Bn   | 0x13  | Bengali |
| Bi   | 0x14  | Bislama |
| Bs   | 0x15  | Bosnian |
| Br   | 0x16  | Breton |
| Bg   | 0x17  | Bulgarian |
| My   | 0x18  | Burmese |
| Ca   | 0x19  | Catalan; Valencian |
| Ch   | 0x1A  | Chamorro |
| Ce   | 0x1B  | Chechen |
| Ny   | 0x1C  | Chichewa; Chewa; Nyanja |
| Zh   | 0x1D  | Chinese |
| Cv   | 0x1E  | Chuvash |
| Kw   | 0x1F  | Cornish |
| Co   | 0x20  | Corsican |
| Cr   | 0x21  | Cree |
| Cs   | 0x22  | Czech |
| Da   | 0x23  | Danish |
| Dv   | 0x24  | Divehi; Dhivehi; Maldivian |
| Nl   | 0x25  | Dutch; Flemish |
| Dz   | 0x26  | Dzongkha |
| En   | 0x27  | English |
| Eo   | 0x28  | Esperanto |
| Et   | 0x29  | Estonian |
| Ee   | 0x2A  | Ewe |
| Fo   | 0x2B  | Faroese |
| Fj   | 0x2C  | Fijian |
| Fi   | 0x2D  | Finnish |
| Fr   | 0x2E  | French |
| Ff   | 0x2F  | Fulah |
| Gl   | 0x30  | Galician |
| Ka   | 0x31  | Georgian |
| De   | 0x32  | German |
| El   | 0x33  | Greek, Modern |
| Gn   | 0x34  | Guarani |
| Gu   | 0x35  | Gujarati |
| Ht   | 0x36  | Haitian; Haitian Creole |
| Ha   | 0x37  | Hausa |
| He   | 0x38  | Hebrew |
| Hz   | 0x39  | Herero |
| Hi   | 0x3A  | Hindi |
| Ho   | 0x3B  | Hiri Motu |
| Hu   | 0x3C  | Hungarian |
| Is   | 0x3D  | Icelandic |
| Io   | 0x3E  | Ido |
| Ig   | 0x3F  | Igbo |
| Id   | 0x40  | Indonesian |
| Ia   | 0x41  | Interlingua |
| Ie   | 0x42  | Interlingue |
| Iu   | 0x43  | Inuktitut |
| Ik   | 0x44  | Inupiaq |
| Ga   | 0x45  | Irish |
| It   | 0x46  | Italian |
| Ja   | 0x47  | Japanese |
| Jv   | 0x48  | Javanese |
| Kl   | 0x49  | Kalaallisut |
| Kn   | 0x4A  | Kannada |
| Kr   | 0x4B  | Kanuri |
| Ks   | 0x4C  | Kashmiri |
| Kk   | 0x4D  | Kazakh |
| Ki   | 0x4E  | Kikuyu |
| Rw   | 0x4F  | Kinyarwanda |
| Ky   | 0x50  | Kyrgyz |
| Kv   | 0x51  | Komi |
| Kg   | 0x52  | Kongo |
| Ko   | 0x53  | Korean |
| Kj   | 0x54  | Kuanyama |
| Ku   | 0x55  | Kurdish |
| Lo   | 0x56  | Lao |
| La   | 0x57  | Latin |
| Lv   | 0x58  | Latvian |
| Li   | 0x59  | Limburgish |
| Ln   | 0x5A  | Lingala |
| Lt   | 0x5B  | Lithuanian |
| Lu   | 0x5C  | Luba-Katanga |
| Lb   | 0x5D  | Luxembourgish |
| Mk   | 0x5E  | Macedonian |
| Mg   | 0x5F  | Malagasy |
| Ms   | 0x60  | Malay |
| Ml   | 0x61  | Malayalam |
| Mt   | 0x62  | Maltese |
| Gv   | 0x63  | Manx |
| Mi   | 0x64  | Maori |
| Mr   | 0x65  | Marathi |
| Mh   | 0x66  | Marshallese |
| Mn   | 0x67  | Mongolian |
| Na   | 0x68  | Nauru |
| Nv   | 0x69  | Navajo |
| Ng   | 0x6A  | Ndonga |
| Ne   | 0x6B  | Nepali |
| Nd   | 0x6C  | North Ndebele |
| Se   | 0x6D  | Northern Sami |
| No   | 0x6E  | Norwegian |
| Nb   | 0x6F  | Norwegian Bokmål |
| Nn   | 0x70  | Norwegian Nynorsk |
| Ii   | 0x71  | Nuosu |
| Oc   | 0x72  | Occitan |
| Oj   | 0x73  | Ojibwa |
| Or   | 0x74  | Oriya |
| Om   | 0x75  | Oromo |
| Os   | 0x76  | Ossetian |
| Pa   | 0x77  | Panjabi |
| Pi   | 0x78  | Pali |
| Fa   | 0x79  | Persian |
| Pl   | 0x7A  | Polish |
| Ps   | 0x7B  | Pashto |
| Pt   | 0x7C  | Portuguese |
| Qu   | 0x7D  | Quechua |
| Ro   | 0x7E  | Romanian |
| Rm   | 0x7F  | Romansh |
| Rn   | 0x80  | Rundi |
| Ru   | 0x81  | Russian |
| Sm   | 0x82  | Samoan |
| Sg   | 0x83  | Sango |
| Sa   | 0x84  | Sanskrit |
| Sc   | 0x85  | Sardinian |
| Gd   | 0x86  | Scottish Gaelic |
| Sr   | 0x87  | Serbian |
| Sn   | 0x88  | Shona |
| Sd   | 0x89  | Sindhi |
| Si   | 0x8A  | Sinhalese |
| Sk   | 0x8B  | Slovak |
| Sl   | 0x8C  | Slovenian |
| So   | 0x8D  | Somali |
| St   | 0x8E  | Southern Sotho |
| Nr   | 0x8F  | South Ndebele |
| Es   | 0x90  | Spanish |
| Su   | 0x91  | Sundanese |
| Sw   | 0x92  | Swahili |
| Ss   | 0x93  | Swati |
| Sv   | 0x94  | Swedish |
| Tl   | 0x95  | Tagalog |
| Ty   | 0x96  | Tahitian |
| Tg   | 0x97  | Tajik |
| Ta   | 0x98  | Tamil |
| Tt   | 0x99  | Tatar |
| Te   | 0x9A  | Telugu |
| Th   | 0x9B  | Thai |
| Bo   | 0x9C  | Tibetan |
| Ti   | 0x9D  | Tigrinya |
| To   | 0x9E  | Tonga |
| Ts   | 0x9F  | Tsonga |
| Tn   | 0xA0  | Tswana |
| Tr   | 0xA1  | Turkish |
| Tk   | 0xA2  | Turkmen |
| Tw   | 0xA3  | Twi |
| Ug   | 0xA4  | Uighur |
| Uk   | 0xA5  | Ukrainian |
| Ur   | 0xA6  | Urdu |
| Uz   | 0xA7  | Uzbek |
| Ve   | 0xA8  | Venda |
| Vi   | 0xA9  | Vietnamese |
| Vo   | 0xAA  | Volapük |
| Wa   | 0xAB  | Walloon |
| Cy   | 0xAC  | Welsh |
| Fy   | 0xAD  | Western Frisian |
| Wo   | 0xAE  | Wolof |
| Xh   | 0xAF  | Xhosa |
| Yi   | 0xB0  | Yiddish |
| Yo   | 0xB1  | Yoruba |
| Za   | 0xB2  | Zhuang |
| Zu   | 0xB3  | Zulu |

The original catalog spelled Belarusian as “Belaurian”; the table above uses the standard English name. The enum member remains `Be`.
