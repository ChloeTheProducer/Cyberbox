using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Cyberbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SoundPlayer _player;
        private bool _isPlaying;
        private string _tempFilePath;
        private string _songName = "Macintosh Plus - 花の専門店 (Flower Specialty Store)"; // Replace with your song info

        public MainWindow()
        {
            InitializeComponent();
            _player = new SoundPlayer();
            ExtractEmbeddedAudioResource();
            _isPlaying = false;
        }

        // Quits the program ya idiot, what'd ya think it was gonna do delete system32
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExtractEmbeddedAudioResource()
        {
            // Get the current assembly
            var assembly = Assembly.GetExecutingAssembly();

            // Define the resource path within the assembly
            string resourceName = "WpfApp.Resources.花の専門店.wav"; // Adjust namespace and resource path

            // Extract the embedded resource to a temporary file
            _tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "C:\\Users\\Chloe\\source\\repos\\Cyberbox\\Cyberbox\\花の専門店.wav花の専門店.wav");

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new FileNotFoundException("Resource not found", resourceName);
                }

                using (FileStream fileStream = new FileStream(_tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }
            }
        }

        // This plays the music
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                // Stop the music
                _player.Stop();
                _isPlaying = false;
                UpdateSongInfo();
            }
            else
            {
                // Play and loop the music
                _player.SoundLocation = _tempFilePath;
                _player.PlayLooping();
                _isPlaying = true;
                UpdateSongInfo();
            }
        }

        // Updates the song info for the currently playing song... bruh
        private void UpdateSongInfo()
        {
            if (_isPlaying)
            {
                songInfoTextBlock.Text = $"Now Playing: {_songName}";
            }
            else
            {
                songInfoTextBlock.Text = "No Song Playing";
            }
        }

    }
}
