using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LastPass_Hello_v2
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer1;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        GoogleAuth auth;
        public MainPage()
        {
            this.InitializeComponent();
            auth = new GoogleAuth();
            timer1 = new DispatcherTimer();
            timer1.Interval = new TimeSpan(1000);

            timer1.Tick += timer1_tick;

            timer1.Start();
            
            
        }

        void timer1_tick(object sender, object e)
        {
            if (App.started == true)
            {
                Texte.Text = auth.OneTimePassword.ToString();
                int progress = auth.SecondsToGo;
                if (progress > progressb.Maximum)
                {
                    progressb.Maximum = progress;
                }
                progressb.Value = progress;
            }
            else if (App.authorized == true)
            {
                if (localSettings.Values.ContainsKey("token"))
                {
                    auth.SecretBase32 = localSettings.Values["token"].ToString();
                    auth.Identity = localSettings.Values["id"].ToString();
                    Username.Text = localSettings.Values["id"].ToString();
                    Texte.Text = auth.OneTimePassword.ToString();
                }
                App.started = true;
                ChPara.IsEnabled = true;
            }
        }

        private async void savebutton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["token"] = Token.Text;
            localSettings.Values["id"] = Username.Text;
            ContentDialog ok = new ContentDialog();
            ok.Content = "Saved! You must restart LastPass Hello.";
            ok.IsPrimaryButtonEnabled = true;
            ok.PrimaryButtonText = "Close";
            await ok.ShowAsync();
            App.Current.Exit();
        }

        private void ChPara_Click(object sender, RoutedEventArgs e)
        {
            if (ChPara.IsChecked == true)
            {
                Username.IsEnabled = true;
                Token.IsEnabled = true;
                savebutton.IsEnabled = true;
                Token.Text = localSettings.Values["token"].ToString();
            }
            else
            {
                Username.IsEnabled = false;
                Token.IsEnabled = false;
                savebutton.IsEnabled = false;
                Token.Text = "";
            }
        }
    }
}
