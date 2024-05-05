using Avalonia.Data.Converters;
using System.Globalization;

namespace Tsundoku.Helpers
{
    public class TitleLangConverter : IMultiValueConverter
    {
        public static readonly TitleLangConverter Instance = new();

        public object? Convert(IList<object?> values, Type type, object? parameter, CultureInfo culture)
        {
            var titles = values[0] as Dictionary<string, string>;
            string lang = values[1].ToString();
            string title = titles.ContainsKey(lang) ? titles[lang] : titles["Romaji"];

            if (values.Count == 3)
            {
                uint dupeIndex = uint.Parse(values[2].ToString());
                if (dupeIndex != 0)
                {
                    title += $" ({dupeIndex})";
                }
            }
            return title;
        }
    }
}
