using System;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Devices;
using Microsoft.Phone.Tasks;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;

namespace PhotoRay
{
    public partial class ScannerView : IDisposable
    {
        #region Fields

        private DispatcherTimer _timer;
        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;
        private bool _disposed;

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

        private void StartInitialization()
        {
            IsInitializing = true;
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

        private void ShowFirstRunNotice()
        {
            if ((bool) IsolatedStorageSettings.ApplicationSettings["FirstRun"])
            {
                IsolatedStorageSettings.ApplicationSettings["FirstRun"] = false;
                IsInitializing = false;
                MessageBox.Show(AppResources.NoticeText, AppResources.NoticeTitle, MessageBoxButton.OK);
                IsInitializing = true;
            }
        }

        private void CheckForCrashReport()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("CrashReport"))
            {
                var report = IsolatedStorageSettings.ApplicationSettings["CrashReport"].ToString();
                if (!string.IsNullOrWhiteSpace(report))
                {
                    IsInitializing = false;
                    if (MessageBox.Show(AppResources.CrashMessageText, AppResources.CrashMessageTitle, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        var emailTask = new EmailComposeTask
                        {
                            To = AppResources.SupportEmail,
                            Subject = AppResources.CrashReportEmailSubject,
                            Body = report
                        };
                        emailTask.Show();
                    }
                    IsInitializing = true;
                }
                IsolatedStorageSettings.ApplicationSettings.Remove("CrashReport");
            }
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
            
            if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
            {
                App.Sid = "debug";
                NavigationService.Navigate(new Uri("/AlbumView.xaml", UriKind.RelativeOrAbsolute));
                return;
            }

            App.Sid = null;

            ThreadPool.QueueUserWorkItem(stateInfo => Dispatcher.BeginInvoke(() =>
                {
                    ShowFirstRunNotice();
                    CheckForCrashReport();
                    StartInitialization();
                }));
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Microsoft.Devices.Environment.DeviceType == DeviceType.Emulator)
            {
                return;
            }

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

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ScannerView()
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
                _photoCamera.Dispose();
            }

            // unmanaged (null)
            _photoCamera = null;
            _disposed = true;
        }

        #endregion
    }
}