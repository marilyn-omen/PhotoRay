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
            if (obj == null)
            {
                return false;
            }
            return obj.Name == Name && obj.Date.Equals(Date);
        }

        public bool Equals(PictureId obj)
        {
            if (obj == null)
            {
                return false;
            }
            return obj.Name == Name && obj.Date.Equals(Date);
        }
    }
}
