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

        private static string GetAppVersion()
        {
            var asm = Assembly.GetExecutingAssembly();
            var parts = asm.FullName.Split(',');
            return parts[1].Split('=')[1];
        }
    }
}