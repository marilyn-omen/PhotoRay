using System;
using System.Globalization;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhotoRay
{
    public partial class App
    {
        public static string Sid { get; set; }
        public static string SelectedAlbum { get; set; }
        public static PictureId SelectedPictureId { get; set; }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += OnApplicationUnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        private void SaveExceptionInfo(Exception e)
        {
            if (!IsolatedStorageSettings.ApplicationSettings.Contains("CrashReport"))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("CrashReport", e.ToString());
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["CrashReport"] = e.ToString();
            }
        }

        public static ApplicationBar BuildLocalizedApplicationBar(EventHandler aboutClickHandler)
        {
            var appBar = new ApplicationBar { Mode = ApplicationBarMode.Minimized };
            var aboutItem = new ApplicationBarMenuItem(AppResources.AboutMenuText);
            aboutItem.Click += aboutClickHandler;
            appBar.MenuItems.Add(aboutItem);
            return appBar;
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void ApplicationLaunching(object sender, LaunchingEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;

            if (!IsolatedStorageSettings.ApplicationSettings.Contains("FirstRun"))
            {
                IsolatedStorageSettings.ApplicationSettings.Add("FirstRun", true);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["FirstRun"] = false;
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void ApplicationActivated(object sender, ActivatedEventArgs e)
        {
            if (e.IsApplicationInstancePreserved)
            {
                return;
            }

            if (PhoneApplicationService.Current.State.ContainsKey("Sid"))
            {
                Sid = PhoneApplicationService.Current.State["Sid"].ToString();
            }

            if (PhoneApplicationService.Current.State.ContainsKey("Album"))
            {
                SelectedAlbum = PhoneApplicationService.Current.State["Album"].ToString();
            }

            if (PhoneApplicationService.Current.State.ContainsKey("Picture"))
            {
                SelectedPictureId = PhoneApplicationService.Current.State["Picture"] as PictureId;
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void ApplicationDeactivated(object sender, DeactivatedEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("Sid"))
            {
                PhoneApplicationService.Current.State["Sid"] = Sid;
            }
            else
            {
                PhoneApplicationService.Current.State.Add("Sid", Sid);
            }

            if (PhoneApplicationService.Current.State.ContainsKey("Album"))
            {
                PhoneApplicationService.Current.State["Album"] = SelectedAlbum;
            }
            else
            {
                PhoneApplicationService.Current.State.Add("Album", SelectedAlbum);
            }

            if (PhoneApplicationService.Current.State.ContainsKey("Picture"))
            {
                PhoneApplicationService.Current.State["Picture"] = SelectedPictureId;
            }
            else
            {
                PhoneApplicationService.Current.State.Add("Picture", SelectedPictureId);
            }
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void ApplicationClosing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void OnRootFrameNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            SaveExceptionInfo(e.Exception);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void OnApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            SaveExceptionInfo(e.ExceptionObject);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool _phoneApplicationInitialized;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (_phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += OnRootFrameNavigationFailed;

            // Ensure we don't initialize again
            _phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}