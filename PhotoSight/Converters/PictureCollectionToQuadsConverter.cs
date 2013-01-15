using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight.Converters
{
    public class PictureCollectionToQuadsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = new List<List<Picture>>();
            var collection = value as PictureCollection;
            if (collection != null)
            {
                var totalCount = collection.Count;
                var linesCount = (int)Math.Ceiling(totalCount / 4.0);
                for (var i = 0; i < linesCount; i++)
                {
                    var line = new List<Picture>();
                    for (var j = i * 4; j < i * 4 + 4 && j < totalCount; j++)
                    {
                        line.Add(collection[j]);
                    }
                    result.Add(line);
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
