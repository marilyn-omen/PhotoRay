using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoRay
{
    public partial class AlbumView : IDisposable
    {
        #region Fields

        private bool _isNewPageInstance;
        private MediaLibrary _library;
        private bool _disposed;

        #endregion

        #region Dependency properties

        #region List<PictureAlbum> Albums

        public List<PictureAlbum> Albums
        {
            get { return (List<PictureAlbum>)GetValue(AlbumsProperty); }
            set { SetValue(AlbumsProperty, value); }
        }

        public static readonly DependencyProperty AlbumsProperty =
            DependencyProperty.Register(
                "Albums", typeof(List<PictureAlbum>), typeof(AlbumView), new PropertyMetadata(null));

        #endregion

        #region PictureAlbum SelectedAlbum

        public PictureAlbum SelectedAlbum
        {
            get { return (PictureAlbum) GetValue(SelectedAlbumProperty); }
            set { SetValue(SelectedAlbumProperty, value); }
        }

        public static readonly DependencyProperty SelectedAlbumProperty =
            DependencyProperty.Register(
                "SelectedAlbum", typeof (PictureAlbum), typeof (AlbumView), new PropertyMetadata(null));

        #endregion

        #endregion

        public AlbumView()
        {
            InitializeComponent();
            _isNewPageInstance = true;
            ApplicationBar =
                App.BuildLocalizedApplicationBar(
                    (sender, args) => NavigationService.Navigate(new Uri("/AboutView.xaml", UriKind.RelativeOrAbsolute)));
        }

        private void OnPhotoTap(object sender, GestureEventArgs e)
        {
            var image = sender as Image;
            if (image == null)
                return;
            var picture = image.Tag as Picture;
            if (picture == null)
                return;

            App.SelectedPictureId = new PictureId(picture.Name, picture.Date);
            NavigationService.Navigate(new Uri("/PhotoView.xaml", UriKind.RelativeOrAbsolute));
        }

        #region Overrides of Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_isNewPageInstance)
            {
                _library = new MediaLibrary();
                _isNewPageInstance = false;
                Albums = _library.RootPictureAlbum.Albums.Where(album => album.Pictures.Count != 0).ToList();
                SelectedAlbum = string.IsNullOrWhiteSpace(App.SelectedAlbum)
                                    ? Albums.FirstOrDefault()
                                    : Albums.FirstOrDefault(album => album.Name == App.SelectedAlbum);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.SelectedAlbum = SelectedAlbum.Name;

            base.OnNavigatedFrom(e);
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~AlbumView()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // managed (dispose)
                _library.Dispose();
            }

            // unmanaged (null)
            _library = null;
            _disposed = true;
        }

        #endregion
    }
}