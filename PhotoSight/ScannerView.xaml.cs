using System;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Devices;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;

namespace PhotoSight
{
    public partial class ScannerView
    {
        #region Fields

        private DispatcherTimer _timer;
        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;

        #endregion

        #region Dependency properties

        #region bool IsInitializing

        public bool IsInitializing
        {
            get { return (bool)GetValue(IsInitializingProperty); }
            set { SetValue(IsInitializingProperty, value); }
        }

        public static readonly DependencyProperty IsInitializingProperty =
            DependencyProperty.Register(
                "IsInitializing", typeof(bool), typeof(ScannerView), new PropertyMetadata(false));

        #endregion

        #endregion

        public ScannerView()
        {
            InitializeComponent();
        }

        private void OnShutterKeyHalfPressed(object o, EventArgs arg)
        {
            _photoCamera.Focus();
        }

        private void OnTimerTick(object o, EventArgs arg)
        {
            ScanPreviewBuffer();
        }

        private void OnPhotoCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            var width = Convert.ToInt32(_photoCamera.PreviewResolution.Width);
            var height = Convert.ToInt32(_photoCamera.PreviewResolution.Height);

            _luminance = new PhotoCameraLuminanceSource(width, height);
            _reader = new QRCodeReader();

            Dispatcher.BeginInvoke(() =>
            {
                ScannerPreviewTransform.Rotation = _photoCamera.Orientation;
                _timer.Start();
                IsInitializing = false;
            });
        }

        private void ScanPreviewBuffer()
        {
            try
            {
                _photoCamera.GetPreviewBufferY(_luminance.PreviewBufferY);
                var binarizer = new HybridBinarizer(_luminance);
                var binBitmap = new BinaryBitmap(binarizer);
                var result = _reader.decode(binBitmap);
                Dispatcher.BeginInvoke(() => StartPhotoGallery(result.Text));
            }
            catch
            {
            }
        }

        private void StartPhotoGallery(string sid)
        {
            if (!string.IsNullOrWhiteSpace(sid))
            {
                App.Sid = sid;
                NavigationService.Navigate(new Uri("/AlbumView.xaml", UriKind.RelativeOrAbsolute));
            }
        }

        #region Overrides of Page

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

#if DEBUG
            App.Sid = "test";
            NavigationService.Navigate(new Uri("/AlbumView.xaml", UriKind.RelativeOrAbsolute));
            return;
#endif

            IsInitializing = true;
            App.Sid = null;
            CameraButtons.ShutterKeyHalfPressed += OnShutterKeyHalfPressed;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(250)
            };
            _timer.Tick += OnTimerTick;

            _photoCamera = new PhotoCamera();
            _photoCamera.Initialized += OnPhotoCameraInitialized;
            ScannerPreviewVideo.SetSource(_photoCamera);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            CameraButtons.ShutterKeyHalfPressed -= OnShutterKeyHalfPressed;

            _timer.Stop();
            _timer.Tick -= OnTimerTick;
            _timer = null;
            
            _photoCamera.Initialized -= OnPhotoCameraInitialized;
            _photoCamera.Dispose();
            _photoCamera = null;

            _luminance = null;
            _reader = null;

            base.OnNavigatingFrom(e);
        }

        #endregion
    }
}