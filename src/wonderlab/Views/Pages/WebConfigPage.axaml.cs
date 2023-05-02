using Avalonia.Controls;
using MinecraftLaunch.Modules.Models.Download;
using wonderlab.Class.Enum;
using wonderlab.ViewModels.Pages;

namespace wonderlab.Views.Pages
{
    public partial class WebConfigPage : UserControl {   
        public static WebConfigPageViewModel ViewModel { get; set; } = new();
        public WebConfigPage() {       
            InitializeComponent();
            DataContext = ViewModel;

            if (App.LauncherData.IssuingBranch == IssuingBranch.Lsaac) {          
                lsaac.IsChecked = true;
            }
            else {
                albert.IsChecked = true;
            }

            if (App.LauncherData.CurrentDownloadAPI.Host == APIManager.Mcbbs.Host) {
                mcbbs.IsChecked = true;
            }
            else if (App.LauncherData.CurrentDownloadAPI.Host == APIManager.Bmcl.Host) {
                bmcl.IsChecked = true;
            }
            else if (App.LauncherData.CurrentDownloadAPI.Host == APIManager.Mojang.Host) {
                official.IsChecked = true;
            }
        }
    }
}
