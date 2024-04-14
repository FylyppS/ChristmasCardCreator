using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace test
{
    public class ColorNameConverter : IValueConverter
    {
        // Metoda konwertująca wartość
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Sprawdź, czy wartość jest niepusta i czy jest to string
            if (value != null && value is string colorName)
            {
                string translatedColorName = TranslateColorName(colorName);

                return translatedColorName;
            }

            // Jeśli wartość jest null lub nie jest stringiem, zwróć pustą wartość
            return string.Empty;
        }

        // Metoda odwrotna konwertująca wartość (niepotrzebna dla tego przypadku, więc pozostawiamy pustą)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        //private string GetTranslatedColorName(string colorName)
        //{
        //    // Spróbuj pobrać tłumaczenie koloru z pliku Resources
        //    string translatedColorName = Properties.Resources.ResourceManager.GetString(colorName, null);

        //    // Jeśli nie ma tłumaczenia, zwróć oryginalną nazwę koloru
        //    return translatedColorName;
        //}

        // Metoda do tłumaczenia nazwy koloru
        private string TranslateColorName(string colorName)
        {
            switch (colorName)
            {
                case "AliceBlue":
                    return "Błękit Alicji";
                case "AntiqueWhite":
                    return "Biel antyczna";
                case "Aqua":
                    return "Turkusowy";
                case "Aquamarine":
                    return "Akwamaryna";
                case "Azure":
                    return "Błękitny";
                case "Beige":
                    return "Beżowy";
                case "Bisque":
                    return "Biszkoptowy";
                case "Black":
                    return "Czarny";
                case "BlanchedAlmond":
                    return "Migdałowy";
                case "Blue":
                    return "Niebieski";
                case "BlueViolet":
                    return "Fioletowoniebieski";
                case "Brown":
                    return "Brązowy";
                case "BurlyWood":
                    return "Drewno jesionowe";
                case "CadetBlue":
                    return "Błękit kadetowy";
                case "Chartreuse":
                    return "Chartreuse";
                case "Chocolate":
                    return "Czekoladowy";
                case "Coral":
                    return "Koralowy";
                case "CornflowerBlue":
                    return "Chabrowy";
                case "Cornsilk":
                    return "Kukurydziany";
                case "Crimson":
                    return "Szkarłatny";
                case "Cyan":
                    return "Cyjan";
                case "DarkBlue":
                    return "Ciemnoniebieski";
                case "DarkCyan":
                    return "Ciemnocyjanowy";
                case "DarkGoldenrod":
                    return "Ciemnozłoty";
                case "DarkGray":
                    return "Ciemnoszary";
                case "DarkGreen":
                    return "Ciemnozielony";
                case "DarkKhaki":
                    return "Ciemne khaki";
                case "DarkMagenta":
                    return "Ciemna magenta";
                case "DarkOliveGreen":
                    return "Ciemnozielony oliwkowy";
                case "DarkOrange":
                    return "Ciemnopomarańczowy";
                case "DarkOrchid":
                    return "Ciemnofiołkowy";
                case "DarkRed":
                    return "Ciemnoczerwony";
                case "DarkSalmon":
                    return "Ciemny łososiowy";
                case "DarkSeaGreen":
                    return "Ciemnozielony morski";
                case "DarkSlateBlue":
                    return "Ciemnogranatowy";
                case "DarkSlateGray":
                    return "Ciemnografitowy";
                case "DarkTurquoise":
                    return "Ciemnoturkusowy";
                case "DarkViolet":
                    return "Ciemnofioletowy";
                case "DeepPink":
                    return "Głęboki róż";
                case "DeepSkyBlue":
                    return "Głęboki błękit nieba";
                case "DimGray":
                    return "Przygaszony szary";
                case "DodgerBlue":
                    return "Dodger blue";
                case "Firebrick":
                    return "Ceglasty";
                case "FloralWhite":
                    return "Biały kwiatowy";
                case "ForestGreen":
                    return "Zielony leśny";
                case "Fuchsia":
                    return "Fuksja";
                case "Gainsboro":
                    return "Gainsboro";
                case "GhostWhite":
                    return "GhostWhite";
                case "Gold":
                    return "Złoty";
                case "Goldenrod":
                    return "Złotawy";
                case "Gray":
                    return "Szary";
                case "Green":
                    return "Zielony";
                case "GreenYellow":
                    return "Zielonożółty";
                case "Honeydew":
                    return "Miodowy";
                case "HotPink":
                    return "Gorący róż";
                case "IndianRed":
                    return "Indyjski czerwony";
                case "Indigo":
                    return "Indygo";
                case "Ivory":
                    return "Kość słoniowa";
                case "Khaki":
                    return "Khaki";
                case "Lavender":
                    return "Lawendowy";
                case "LavenderBlush":
                    return "Lawendowy róż";
                case "LawnGreen":
                    return "Zielony trawnikowy";
                case "LemonChiffon":
                    return "Cytrynowy";
                case "LightBlue":
                    return "Jasnoniebieski";
                case "LightCoral":
                    return "Jasny koralowy";
                case "LightCyan":
                    return "Jasny cyjan";
                case "LightGoldenrodYellow":
                    return "Jasny złotawy";
                case "LightGray":
                    return "Jasnoszary";
                case "LightGreen":
                    return "Jasnozielony";
                case "LightPink":
                    return "Jasnoróżowy";
                case "LightSalmon":
                    return "Jasny łososiowy";
                case "LightSeaGreen":
                    return "Jasny morski zielony";
                case "LightSkyBlue":
                    return "Jasny błękit nieba";
                case "LightSlateGray":
                    return "Jasny grafitowy";
                case "LightSteelBlue":
                    return "Jasny stalowy niebieski";
                case "LightYellow":
                    return "Jasnożółty";
                case "Lime":
                    return "Limonkowy";
                case "LimeGreen":
                    return "Limonkowy zielony";
                case "Linen":
                    return "Lniany";
                case "Magenta":
                    return "Magenta";
                case "Maroon":
                    return "Kasztanowy";
                case "MediumAquamarine":
                    return "Średnia akwamaryna";
                case "MediumBlue":
                    return "Średni niebieski";
                case "MediumOrchid":
                    return "Średni fiołkowy";
                case "MediumPurple":
                    return "Średni fioletowy";
                case "MediumSeaGreen":
                    return "Średni zielonomorski";
                case "MediumSlateBlue":
                    return "Średni grafitowoniebieski";
                case "MediumSpringGreen":
                    return "Średni wiosenny zielony";
                case "MediumTurquoise":
                    return "Średni turkusowy";
                case "MediumVioletRed":
                    return "Średni fiołkowoczerwony";
                case "MidnightBlue":
                    return "Granatowy";
                case "MintCream":
                    return "Miętowy";
                case "MistyRose":
                    return "Mglisty róż";
                case "Moccasin":
                    return "Mokasynowy";
                case "NavajoWhite":
                    return "Biel Navajo";
                case "Navy":
                    return "Granatowy morski";
                case "OldLace":
                    return "OldLace";
                case "Olive":
                    return "Oliwkowy";
                case "OliveDrab":
                    return "Oliwkowy zielony";
                case "Orange":
                    return "Pomarańczowy";
                case "OrangeRed":
                    return "Czerwony pomarańczowy";
                case "Orchid":
                    return "Fiołkowy";
                case "PaleGoldenrod":
                    return "Blady złotawy";
                case "PaleGreen":
                    return "Blady zielony";
                case "PaleTurquoise":
                    return "Blady turkusowy";
                case "PaleVioletRed":
                    return "Blady fioletowoczerwony";
                case "PapayaWhip":
                    return "Kolba papai";
                case "PeachPuff":
                    return "Brzoskwiniowy";
                case "Peru":
                    return "Peruwiański";
                case "Pink":
                    return "Różowy";
                case "Plum":
                    return "Śliwkowy";
                case "PowderBlue":
                    return "Proszkowy niebieski";
                case "Purple":
                    return "Fioletowy";
                case "Red":
                    return "Czerwony";
                case "RosyBrown":
                    return "Różowy brąz";
                case "RoyalBlue":
                    return "Królewski niebieski";
                case "SaddleBrown":
                    return "Brąz siodłowy";
                case "Salmon":
                    return "Łososiowy";
                case "SandyBrown":
                    return "Piaskowy brąz";
                case "SeaGreen":
                    return "Zielonomorski";
                case "SeaShell":
                    return "Muszelkowy";
                case "Sienna":
                    return "Sienny";
                case "Silver":
                    return "Srebrny";
                case "SkyBlue":
                    return "Błękit nieba";
                case "SlateBlue":
                    return "Grafitowoniebieski";
                case "SlateGray":
                    return "Grafitowy";
                case "Snow":
                    return "Śnieżny";
                case "SpringGreen":
                    return "Wiosenny zielony";
                case "SteelBlue":
                    return "Stalowy niebieski";
                case "Tan":
                    return "Tan";
                case "Teal":
                    return "Zielonomodry";
                case "Thistle":
                    return "Karczochowy";
                case "Tomato":
                    return "Pomidorowy";
                case "Transparent":
                    return "Przezroczysty";
                case "Turquoise":
                    return "Turkusowy";
                case "Violet":
                    return "Fiolet";
                case "Wheat":
                    return "Pszeniczny";
                case "White":
                    return "Biały";
                case "WhiteSmoke":
                    return "Biały dym";
                case "Yellow":
                    return "Żółty";
                case "YellowGreen":
                    return "Zielonożółty";
                default:
                    return colorName; // Zwróć oryginalną wartość, jeśli nie ma tłumaczenia
            }

        }
    }
}
