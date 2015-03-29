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
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Storage.Streams;


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

        private async void SpeakButton_Clicked(object sender, RoutedEventArgs e)
        {
            
            try
            {
                var result = await SR.RecognizeSpeechToTextAsync();
                daBox.Text = result.Text;
            }
            catch (Exception)
            {
                daBox.Text = "Error Occured";
            }
            if (daBox.Text == "Open the Garage")
                send_Data("garagecommand");//send signal to garage
            if (daBox.Text == "Turn on the lights")
                send_Data("lightcommand");//send signal to lights
            if (daBox.Text == "Turn on the heat")
                send_Data("heatcommand");//send signal for heat

            
        }
        private async void send_Data(string command)
        {
            StreamSocket socket = new StreamSocket();
            HostName deviceHostName= new HostName("");
            DataWriter dataWriter = new DataWriter(socket.OutputStream);
            await socket.ConnectAsync(deviceHostName, "1");

            if(dataWriter != null)
            {
                uint commandSize = dataWriter.MeasureString(command);
                dataWriter.WriteByte((byte)commandSize);
                dataWriter.WriteString(command);
                await dataWriter.StoreAsync();
            }
        }
 
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var credentials = new SpeechAuthorizationParameters();
            credentials.ClientId = "Andreweknow";
            credentials.ClientSecret = "k+gEHcP3wiwPmAzuw27ZtakgOJllIkDlBAsHTu1HLkE=";
            SR = new SpeechRecognizer("en-US", credentials);
            SpeechControl.Tips = new string[]
            {
                "For more accurate results, try using a headset microphone.",
                "Speak with a consistent volume.",
                "Speak in a natural rhythm with clear consonants.",
                "Speak with a slow to moderate tempo.",
                "Background noise may interfere with accurate speech recognition."
            };
            SpeechControl.SpeechRecognizer = SR;
            SpeakButton_Clicked(null, null);
        }
        
    }


}
