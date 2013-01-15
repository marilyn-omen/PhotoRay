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

        private readonly DispatcherTimer _timer;
        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;

        #endregion

        #region Dependency properties

        #region bool IsCodeScanned

        public bool IsCodeScanned
        {
            get { return (bool)GetValue(IsCodeScannedProperty); }
            set { SetValue(IsCodeScannedProperty, value); }
        }

        public static readonly DependencyProperty IsCodeScannedProperty =
            DependencyProperty.Register("IsCodeScanned", typeof (bool), typeof (ScannerView),
                                        new PropertyMetadata(false));
        
        #endregion

        #endregion

        public ScannerView()
        {
            InitializeComponent();

            _timer = new DispatcherTimer
                         {
                             Interval = TimeSpan.FromMilliseconds(250)
                         };
            _timer.Tick += (o, arg) => ScanPreviewBuffer();
            CameraButtons.ShutterKeyHalfPressed += (o, arg) => _photoCamera.Focus();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            App.Sid = null;
            _photoCamera = new PhotoCamera();
            _photoCamera.Initialized += OnPhotoCameraInitialized;
            ScannerPreviewVideo.SetSource(_photoCamera);
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
                _timer.Stop();
                _photoCamera.Initialized -= OnPhotoCameraInitialized;
                _photoCamera = null;
                App.Sid = sid;
                NavigationService.Navigate(new Uri("/AlbumView.xaml", UriKind.RelativeOrAbsolute));
            }
        }
    }
}