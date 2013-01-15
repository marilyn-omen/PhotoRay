using System;
using System.Windows.Data;

namespace PhotoSight.Converters
{
    [Flags]
    public enum TextTransform
    {
        None = 0x0,
        ToUpper = 0x1,
        ToLower = 0x2,
        Capitalize = 0x4
    }

    public class StringConverter : IValueConverter
    {
        public TextTransform Transform { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var result = value.ToString();
            switch (Transform)
            { 
                case TextTransform.ToUpper:
                    result = result.ToUpperInvariant();
                    break;
                case TextTransform.ToLower:
                    result = result.ToLowerInvariant();
                    break;
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
