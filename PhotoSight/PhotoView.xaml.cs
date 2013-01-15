using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight
{
    public partial class PhotoView
    {
        #region Dependency properties

        #region List<Picture> Pictures

        public List<Picture> Pictures
        {
            get { return (List<Picture>)GetValue(PicturesProperty); }
            set { SetValue(PicturesProperty, value); }
        }

        public static readonly DependencyProperty PicturesProperty =
            DependencyProperty.Register(
                "Pictures", typeof (List<Picture>), typeof (PhotoView), new PropertyMetadata(null));

        #endregion

        #region Picture SelectedPicture
        
        public Picture SelectedPicture
        {
            get { return (Picture)GetValue(SelectedPictureProperty); }
            set { SetValue(SelectedPictureProperty, value); }
        }

        public static readonly DependencyProperty SelectedPictureProperty =
            DependencyProperty.Register(
                "SelectedPicture",
                typeof (Picture),
                typeof (PhotoView),
                new PropertyMetadata(null, SelectedPicturePropertyChanged));

        private static void SelectedPicturePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PhotoView) d).OnSelectedPictureChanged();
        }

        #endregion

        #endregion

        public PhotoView()
        {
            InitializeComponent();
        }

        private void OnSelectedPictureChanged()
        {
            if (SelectedPicture != null)
            {
                var post = new PostSubmitter
                    {
                        Url = "http://sltv.org.ua/upload.php",
                        Parameters = new Dictionary<string, object>
                            {
                                {"sid", App.Sid},
                                {"uploadedfile", Utils.StreamToByteArray(SelectedPicture.GetImage())}
                            }
                    };
                post.Submit();
            }
        }

        #region Overrides of Page

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.SelectedPicture != null && !string.IsNullOrWhiteSpace(App.Sid))
            {
                Pictures = new List<Picture>(App.SelectedPicture.Album.Pictures);
                SelectedPicture = App.SelectedPicture;
            }
        }

        #endregion
    }
}