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
        private string _songName = "Macintosh Plus - 花の専門店 (Flower Specialty Store)"; // Replace with your song info

        public MainWindow()
        {
            InitializeComponent();
            _player = new SoundPlayer();
            _isPlaying = false;
        }

        // Quits the program ya idiot, what'd ya think it was gonna do delete system32
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // This plays the music
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string songFileName = "花の専門店.wav";
            PlayMusicInExecutableDirectory(songFileName);

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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PlayMusicInExecutableDirectory(string fileName)
        {
            string execDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(execDirectory, fileName);

            if (File.Exists(filePath))
            {
                _player = new SoundPlayer(filePath);
                _player.Play();
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
                    _player.PlayLooping();
                    _isPlaying = true;
                    UpdateSongInfo();

                }
            }
            else
            {
                System.Windows.MessageBox.Show("File not found: " + filePath, "Error", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
            }
        }
    }
}
