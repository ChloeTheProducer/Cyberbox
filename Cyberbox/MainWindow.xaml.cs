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
using System.Windows.Threading;


namespace Cyberbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer _player;
        private bool _isPlaying;
        private string _songName = "Home Depot Theme"; // Replace with your song info


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
        private void BGM_Click(object sender, RoutedEventArgs e)
        {
            string songFileName = "The Home Depot Beat (Full).wav";
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

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Video_Rectangle()
        {

        }
    }
}
