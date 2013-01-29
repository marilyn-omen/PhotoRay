using System;
using Microsoft.Xna.Framework.Media;

namespace PhotoRay
{
    public class PictureId
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public PictureId()
        {
        }

        public PictureId(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        public bool Equals(Picture obj)
        {
            return obj.Name == Name && obj.Date.Equals(Date);
        }
    }
}
