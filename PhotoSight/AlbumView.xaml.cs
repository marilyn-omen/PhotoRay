using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight
{
    public partial class AlbumView
    {
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

        #endregion

        public AlbumView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.SelectedPicture = null;
            var lib = new MediaLibrary();
            Albums = lib.RootPictureAlbum.Albums.Where(album => album.Pictures.Count != 0).ToList();
        }

        private void OnPhotoTap(object sender, GestureEventArgs e)
        {
            var image = sender as Image;
            if (image == null)
                return;
            var picture = image.Tag as Picture;
            if (picture == null)
                return;

            App.SelectedPicture = picture;
            NavigationService.Navigate(new Uri("/PhotoView.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}