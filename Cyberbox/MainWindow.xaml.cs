using System;
using System.IO;
using System.Media;
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
        private string _songName = "Cypis - Gdzie jest biały węgorz ? (Zejście)"; // Replace with your song info

        public MainWindow()
        {
            InitializeComponent();
            _player = new SoundPlayer(@"C:\Users\Chloe\Music\Polish cow (English Lyrics Full Version).wav");
            _isPlaying = false;
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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
                _player.PlayLooping();
                _isPlaying = true;
                UpdateSongInfo();
            }

            void UpdateSongInfo()
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
}