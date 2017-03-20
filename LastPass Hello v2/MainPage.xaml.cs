using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
        DispatcherTimer timerfast;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        GoogleAuth auth;
        Boolean stopfast = false;

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
                if (Convert.ToBoolean(localSettings.Values["fast"]) == true && stopfast == false)
                {
                    timerfast = new DispatcherTimer();
                    timerfast.Interval = new TimeSpan(0, 0, 2);

                    timerfast.Tick += timerfast_tick;
                    timerfast.Start();
                }
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
                    fastcheck.IsChecked = Convert.ToBoolean(localSettings.Values["fast"]);
                    Texte.Text = auth.OneTimePassword.ToString();
                }
                else
                {
                    localSettings.Values["token"] = "";
                    localSettings.Values["id"] = "";
                    localSettings.Values["fast"] = false;
                }
                App.started = true;
                ChPara.IsEnabled = true;
                copyButton.IsEnabled = true;
            }
        }

        void timerfast_tick(object sender, object e)
        {
            Debug.WriteLine("Start Fast Mode");
            if (fastcheck.IsChecked == true)
            {
                Debug.WriteLine("Fast Mode");
                var dataPackage = new DataPackage();
                dataPackage.SetText(auth.OneTimePassword.ToString());
                Clipboard.SetContent(dataPackage);
                App.Current.Exit();
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

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(auth.OneTimePassword.ToString());
            Clipboard.SetContent(dataPackage);
        }

        private void fastcheck_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values["fast"] = fastcheck.IsChecked;
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F)
            {
                stopfast = true;
            }
        }
    }
}
