using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Phone.Controls.Primitives;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight
{
    public class LoopingPicturesDataSource : ILoopingSelectorDataSource
    {
        private readonly List<Picture> _pictures;
        
        private int _index;
        private object _selectedItem;

        private int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (_index >= _pictures.Count)
                {
                    _index = 0;
                }
                if (_index < 0)
                {
                    _index = _pictures.Count - 1;
                }
            }
        }

        public LoopingPicturesDataSource(List<Picture> pictures)
        {
            _pictures = pictures;
            if(_pictures.Count > 0)
            {
                SelectedItem = _pictures[0];
            }
        }

        private void OnSelectionChanged(object oldSelectedItem, object newSelectedItem)
        {
            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new SelectionChangedEventArgs(new[] {oldSelectedItem}, new[] {newSelectedItem}));
            }
        }

        #region Implementation of ILoopingSelectorDataSource

        public object GetNext(object relativeTo)
        {
            var relativePicture = relativeTo as Picture;
            if (relativePicture != null && _pictures != null && _pictures.Count > 0)
            {
                Index = _pictures.IndexOf(relativePicture) + 1;
                return _pictures[Index];
            }
            return null;
        }

        public object GetPrevious(object relativeTo)
        {
            var relativePicture = relativeTo as Picture;
            if (relativePicture != null && _pictures != null && _pictures.Count > 0)
            {
                Index = _pictures.IndexOf(relativePicture) - 1;
                return _pictures[Index];
            }
            return null;
        }

        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                // this will use the Equals method if it is overridden for the data source item class
                if (!Equals(_selectedItem, value))
                {
                    // save the previously selected item so that we can use it 
                    // to construct the event arguments for the SelectionChanged event
                    object previousSelectedItem = _selectedItem;
                    _selectedItem = value;
                    // fire the SelectionChanged event
                    OnSelectionChanged(previousSelectedItem, _selectedItem);
                }
            }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        #endregion
    }
}
