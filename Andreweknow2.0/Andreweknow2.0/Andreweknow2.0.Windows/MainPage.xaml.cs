using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Media.SpeechSynthesis;
using Windows.Media;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Andreweknow2._0
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
           
    {
        DataWriter dataWriter;
        SpeechRecognizer SR;
        StreamSocket socket;
        SpeechSynthesizer speech;
        SpeechSynthesisStream stream;
        MediaElement mediaElement1;
        bool connected;

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
            speech = new SpeechSynthesizer();
            mediaElement1 = this.media;
            speech.Voice = SpeechSynthesizer.AllVoices[2];
       
            try
            {
                var result = await SR.RecognizeSpeechToTextAsync();
                daBox.Text = result.Text;
               
            }
            catch (Exception a)
            {
                daBox.Text = a.ToString();
            }
            bool open, lights, garage, on, heat, off, unlock, lock1, num, close;
            open = lights = garage = on = heat = off = unlock = lock1 = num = close = false;
            int temp = 0;
            int number = 0;
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            String[] text = daBox.Text.ToLower().Split(delimiterChars);
            foreach (string s in text)
            {
                if (s == "open")
                    open = true;
                if (s == "lights")
                    lights = true;
                if (s == "garage")
                    garage = true;
                if (s == "on")
                    on = true;
                if (s == "heat" || s == "heater")
                    heat = true;
                if (s == "off")
                    off = true;
                if (s == "unlock")
                    unlock = true;
                if (s == "lock")
                    lock1 = true;
                if (int.TryParse(s, out temp)){
                    num = true;
                    number = temp;
                }
                if (s == "close")
                    close = true;
            }
            if (open && garage)
            {
                await SendCommand("2");//send signal to garage
                stream= await speech.SynthesizeTextToStreamAsync("The Garage is opening.");
            }
            else if (on && lights)
            {
                await SendCommand("1");//send signal to lights
                stream= await speech.SynthesizeTextToStreamAsync("The Lights are turning on.");
            }
            else if (heat)
            {

                if (num)
                {
                    await SendCommand("3");
                    stream = await speech.SynthesizeTextToStreamAsync("The heater is set to " + number + ".");
                }

                else if (on)
                {
                    await SendCommand("3");//send signal for heating
                    stream = await speech.SynthesizeTextToStreamAsync("The heater is now on.");
                }
            }
            else if (off)
            {
                await SendCommand("0");//kill Kyle
                stream = await speech.SynthesizeTextToStreamAsync("They're off");
            }
            else if(lock1)
            {
                await SendCommand("4");
                stream= await speech.SynthesizeTextToStreamAsync("Locked");
            }
            else if(unlock)
            {
                await SendCommand("5");
                stream = await speech.SynthesizeTextToStreamAsync("Unlocked");
            }
            else if (close && garage)
            {
                await SendCommand("6");
                stream = await speech.SynthesizeTextToStreamAsync("Garage Closed");
            }
            else
                stream = null;
            if (stream != null)
            {
                this.media.AutoPlay = true;
                this.media.SetSource(stream, stream.ContentType);
                this.media.Play();
            }
        }
        private async Task<bool> connect()
        {
            socket = new StreamSocket();
            HostName deviceHostName = new HostName("20:14:10:14:07:50");

            if (socket != null)
            {
                try
                {
                    await socket.ConnectAsync(deviceHostName, "1");
                    dataWriter = new DataWriter(socket.OutputStream);
                    return true;
                }
                catch (Exception) { daBox.Text = this.ToString(); return false; };
            }
            else { return false; }
        }
        public async Task<uint> SendCommand(string command)
        {
            uint sentCommandSize = 0;
            if (dataWriter != null && connected)
            {
                uint commandSize = dataWriter.MeasureString(command);
                dataWriter.WriteByte((byte)commandSize);
                sentCommandSize = dataWriter.WriteString(command);
                await dataWriter.StoreAsync();
            }
            return sentCommandSize;
        }
 
        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            connected = await connect();
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


        private void disc_clicked(object sender, RoutedEventArgs e)
        {
            if(socket!=null)
                socket.Dispose();
        }
       
        
    }


}
