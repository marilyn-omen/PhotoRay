using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Xna.Framework.Media;

namespace PhotoSight
{
    public partial class PhotoView
    {
        #region Dependency properties

        #region LoopingPicturesDataSource PicturesSource

        public LoopingPicturesDataSource PicturesSource
        {
            get { return (LoopingPicturesDataSource)GetValue(PicturesSourceProperty); }
            set { SetValue(PicturesSourceProperty, value); }
        }

        public static readonly DependencyProperty PicturesSourceProperty =
            DependencyProperty.Register(
                "PicturesSource", typeof(LoopingPicturesDataSource), typeof(PhotoView), new PropertyMetadata(null));

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
        }

        private void UploadPicture(Picture picture)
        {
            if (picture != null && !IsUploading)
            {
                var worker = new BackgroundWorker();
                worker.DoWork += (sender, args) =>
                    {
                        var post = new PostSubmitter
                        {
                            Url = "http://sltv.org.ua/upload.php",
                            Parameters = new Dictionary<string, object>
                            {
                                {"sid", App.Sid},
                                {"uploadedfile", Utils.StreamToByteArray(((Picture) args.Argument).GetImage())}
                            }
                        };
                        post.Submit();
                    };
                worker.RunWorkerCompleted += (sender, args) =>
                    {
                        IsUploading = false;
                    };
                IsUploading = true;
                worker.RunWorkerAsync(picture);
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
                UploadPicture(App.SelectedPicture);
                PicturesSource = new LoopingPicturesDataSource(new List<Picture>(App.SelectedPicture.Album.Pictures));
                PicturesSource.SelectionChanged += (sender, args) =>
                    {
                        if (args.AddedItems.Count > 0)
                        {
                            UploadPicture(args.AddedItems[0] as Picture);
                        }
                    };
            }
        }

        #endregion
    }
}