using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Phone.Tasks;

namespace PhotoRay
{
    public partial class AboutView
    {
        public string AssemblyVersion { get; private set; }

        public AboutView()
        {
            InitializeComponent();
            AssemblyVersion = GetAppVersion();
        }

        private void OnReviewAppTap(object sender, GestureEventArgs e)
        {
            var reviewTask = new MarketplaceReviewTask();
            reviewTask.Show();
        }

        private void OnSendFeedbackAppTap(object sender, GestureEventArgs e)
        {
            var emailTask = new EmailComposeTask
            {
                To = AppResources.SupportEmail
            };
            emailTask.Show();
        }

        private void OnSourcesTap(object sender, GestureEventArgs e)
        {
            var task = new WebBrowserTask
            {
                Uri = new Uri(AppResources.GitHubAddress)
            };
            task.Show();
        }

        private static string GetAppVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            return parts[1].Split('=')[1];
        }
    }
}