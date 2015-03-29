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
using Bing.Speech;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Andreweknow2._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SpeechRecognizer SR;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        private void clicked(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            daBox.Text = b.Content.ToString();
        }

        private void SpeakButton_Clicked(object sender, RoutedEventArgs e)
        {

        }

 
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var credentials = new SpeechAuthorizationParameters();
            credentials.ClientId = "Andreweknow";
            credentials.ClientSecret = "k+gEHcP3wiwPmAzuw27ZtakgOJllIkDlBAsHTu1HLkE=";
            SR = new SpeechRecognizer("en-US", credentials);
        }
        

        
    }


}
