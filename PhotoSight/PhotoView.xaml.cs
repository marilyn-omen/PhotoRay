using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight
{
    public partial class PhotoView
    {
        #region Fields

        private List<Picture> _allPictures;
        private readonly PostSubmitter _post;

        #endregion

        #region Dependency properties

        #region ObservableCollection<Picture> Pictures

        public ObservableCollection<Picture> Pictures
        {
            get { return (ObservableCollection<Picture>)GetValue(PicturesProperty); }
            set { SetValue(PicturesProperty, value); }
        }

        public static readonly DependencyProperty PicturesProperty =
            DependencyProperty.Register(
                "Pictures", typeof(ObservableCollection<Picture>), typeof(PhotoView), new PropertyMetadata(null));

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
            ((PhotoView) d).OnSelectedPictureChanged(e.NewValue as Picture);
        }

        #endregion

        #region bool IsUploading

        public bool IsUploading
        {
            get { return (bool)GetValue(IsUploadingProperty); }
            set { SetValue(IsUploadingProperty, value); }
        }

        public static readonly DependencyProperty IsUploadingProperty =
            DependencyProperty.Register("IsUploading", typeof (bool), typeof (PhotoView), new PropertyMetadata(false));

        #endregion

        #endregion

        public PhotoView()
        {
            InitializeComponent();

            _post = new PostSubmitter
                {
                    Url = "http://photosight.azurewebsites.net/upload.php"
                };
            _post.Completed += (sender, args) => Dispatcher.BeginInvoke(() => { IsUploading = false; });
        }

        private void UploadPicture(Picture picture)
        {
            if (picture != null && !IsUploading)
            {
                IsUploading = true;
                _post.Parameters = new Dictionary<string, object>
                    {
                        {"sid", App.Sid},
                        {"uploadedfile", Utils.StreamToByteArray(picture.GetImage())}
                    };
                _post.Submit();
            }
        }

        private void OnSelectedPictureChanged(Picture newPicture)
        {
            if(newPicture != null)
            {
                UploadPicture(newPicture);
                var picIdx = _allPictures.IndexOf(newPicture);
                var pivotIdx = Pictures.IndexOf(newPicture);
                var nextIdx = (pivotIdx + 1) % 3;
                var prevIdx = ((pivotIdx - 1) < 0) ? 2 : (pivotIdx - 1);
                Pictures[nextIdx] = _allPictures[LoopIncrement(picIdx)];
                Pictures[prevIdx] = _allPictures[LoopDecrement(picIdx)];
                
            }
        }

        private int LoopIncrement(int n)
        {
            n++;
            if(n >= _allPictures.Count)
            {
                n = 0;
            }
            return n;
        }

        private int LoopDecrement(int n)
        {
            n--;
            if(n < 0)
            {
                n = _allPictures.Count - 1;
            }
            return n;
        }

        #region Overrides of Page

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            _allPictures = new List<Picture>(App.SelectedPicture.Album.Pictures);
            if(_allPictures.Count == 0)
                return;

            var selectedIdx = _allPictures.IndexOf(App.SelectedPicture);
            selectedIdx = LoopDecrement(selectedIdx);
            Pictures = new ObservableCollection<Picture>
                {
                    _allPictures[LoopDecrement(selectedIdx)],
                    _allPictures[selectedIdx],
                    _allPictures[LoopIncrement(selectedIdx)]
                };
            SelectedPicture = App.SelectedPicture;
        }

        #endregion
    }
}