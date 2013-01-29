using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoRay
{
    public partial class PhotoView : IDisposable
    {
        #region Fields

        private MediaLibrary _library;
        private List<Picture> _allPictures;
        private readonly PostSubmitter _post;
        private bool _doNotUploadFlag;
        private bool _disposed;

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
                    Url = "http://photoray.sltv.org.ua/upload.php"
                };
            _post.Completed += (sender, args) => Dispatcher.BeginInvoke(() => { IsUploading = false; });
        }

        private void OnSelectedPictureChanged(Picture newPicture)
        {
            if(newPicture != null)
            {
                App.SelectedPictureId = new PictureId(newPicture.Name, newPicture.Date);
                if (_doNotUploadFlag)
                {
                    _doNotUploadFlag = false;
                }
                else
                {
                    UploadPicture(newPicture);
                }
                var picIdx = _allPictures.IndexOf(newPicture);
                var pivotIdx = Pictures.IndexOf(newPicture);
                var nextIdx = (pivotIdx + 1)%3;
                var prevIdx = ((pivotIdx - 1) < 0) ? 2 : (pivotIdx - 1);
                Pictures[nextIdx] = _allPictures[LoopIncrement(picIdx)];
                Pictures[prevIdx] = _allPictures[LoopDecrement(picIdx)];
            }
        }

        #region Helper methods

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

        private int LoopIncrement(int n)
        {
            n++;
            if (n >= _allPictures.Count)
            {
                n = 0;
            }
            return n;
        }

        private int LoopDecrement(int n)
        {
            n--;
            if (n < 0)
            {
                n = _allPictures.Count - 1;
            }
            return n;
        }

        #endregion

        #region Overrides of Page

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (App.SelectedPictureId == null)
            {
                throw new InvalidOperationException("Selected picture was not set before navigating to Photo view");
            }
            if (string.IsNullOrWhiteSpace(App.SelectedAlbum))
            {
                throw new InvalidOperationException("Selected album was not set before navigating to Photo view");
            }
            if (string.IsNullOrWhiteSpace(App.Sid))
            {
                throw new InvalidOperationException("Client SID was not set before navigating to Photo view");
            }

            _library = new MediaLibrary();
            var album = _library.RootPictureAlbum.Albums.First(a => a.Name == App.SelectedAlbum);
            _allPictures = new List<Picture>(album.Pictures);
            if (_allPictures.Count == 0)
                return;

            var selectedPicture = _allPictures.First(p => App.SelectedPictureId.Equals(p));
            var selectedIdx = _allPictures.IndexOf(selectedPicture);
            selectedIdx = LoopDecrement(selectedIdx);
            Pictures = new ObservableCollection<Picture>
                {
                    _allPictures[LoopDecrement(selectedIdx)],
                    _allPictures[selectedIdx],
                    _allPictures[LoopIncrement(selectedIdx)]
                };
            if (!e.IsNavigationInitiator)
            {
                _doNotUploadFlag = true;
            }
            SelectedPicture = selectedPicture;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~PhotoView()
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