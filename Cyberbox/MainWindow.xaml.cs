using Microsoft.VisualBasic;
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

// This entire program was written by a trans girl.
// I'm not helping the trans girl programmer stereotype much am i...
namespace Cyberbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SoundPlayer _player;
        private bool _isPlaying;
        private string _songName = "Bensound - The Elevator Bossanova"; // Replace with your song info
        
        // Main window... what else you want
        public MainWindow()
        {
            InitializeComponent();
            
            _player = new SoundPlayer();
            _isPlaying = false;
        }

        // Quits the program ya idiot, what'd ya think it was gonna do delete system32
        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        // This plays the music
        private void BGM_Click(object sender, RoutedEventArgs e)
        {
            string songFileName = @"BGM\bensound-theelevatorbossanova.wav";
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

        // Plays the background music depending on the current state... Chuck persons ecco jams vol. 1
        // is a vibe btw
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

        // This will run the converter to export data to .wav depending on the designated show file type
        private void Export_Click(object sender, RoutedEventArgs e)
        {

        }

        // Drops down a menu to select different options
        // (Note: Not sure if im gonna keep this because all this software does is
        // take the show files and converts them to .wav. You dont really need any extra options for that.
        // all you need is to import your file. If you accidentally selected the wrong format type for
        // the show file, then all you need is to select import and redo the import
        // cause trying to setup an auto determine to determine an exact stage type for the show
        // would be difficult as one show file would/could work with multiple stages and shows.
        // But i will setup an auto determine to generalize stage types for that format. So if you import
        // a .cshw it will give you the stage options for that format so you just have to select if
        // its for modern cybers or ptt cybers.)
        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        // Opens the window to select a show file
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Window1 Import_Click = new Window1();
            Import_Click.Show();
        }
    }
}
