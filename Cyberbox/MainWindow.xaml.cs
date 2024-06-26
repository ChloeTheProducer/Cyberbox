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
        private string _songName = "Macintosh Plus - 花の専門店 (Flower Specialty Store)"; // Replace with your song info
        private DispatcherTimer animationTimer;
        private Random random;
        private double animationDuration = 3.0; // Duration of one animation cycle in seconds
        private double colorChangeDuration = 0.5; // Duration for color change in seconds
        private double timer;
        private double scaleMultiplier = 0.1; // Maximum scale change

        private string[] splashTexts =
        {
            "How's the weather", "Hey! Where's Perry?", "Random is a cutie!",
            "Too much benadryl and you'll be meeting the grayman", "Always piss in aisle 4... It's a security camera blindspot",
            "How much rain is too much", "What's 9 + 10", "Creeper... Aw man!", "*Windows 95 Startup Sound*",
            "The cake is a lie", "The lie is a cake", "The beans be crazy", "Roxanne Wolf is cute", "This RR Engine game is enhanced with URP",
            "Nuh uh", "I know what's wrong with it, ain't got no gas in it!", "GRAND DAD!", "Omae wa mu, shinderu", "I'm the trash man", "Can i offer you an egg in this trying time?",
            "Anyway, I started blastin", "This splash text be bussin", "Try and find the secret", "public bool SCIsCool = true;", "This splash text is held together by duct tape and dreams",
            "Wanna play Animal Crossing?", "I'm a goofy goober!", "You're a goofy goober!", "We're all goofy goobers!", "Goofy goofy goofy goober!", "*GMod collision noises*", "Ah, Hello Gordon",
            "Showtape Central is brought to you by players like you, Thank You", "Say goodbye to your knee caps chucklehead!", "The spy is a spy", "CEC Corp. is sus", "Billy Bob was not the imposter", "Quokka's are adorable",
            "Heat from fire, fire from heat", "Mom said it's my turn to program the animatronics", "The only really good thing to come out of Security Breach is Roxanne Wolf",
            "Do a flip", "Skibidi Toilet. Is. Fucking. Dumb.", "youtube.com/watch?v=xconDALayoU", "Liminal spaces and vaporwave are two things that somehow work really well together",
            "Just click play", "The man is in the source", "When you're in, you're in for a real good time", "When you're here, you're family",
            "If the women don't find you handsome, they should at least find you handy", "Keep your stick on the ice", "This week in Showtape Central...", "This is a game, it can change, if it has to, I guess",
            "If at first you dont succeed, use more duct tape", "Quando Omni Flunkus Moritati",
            "If it ain't broke, you're not trying.", "Growing old is mandatory, growing up is optional.",
            "MMBB is the band of all time", "Where there's a will... His names bill", "Where there's a will... there's probably not a way", "Where there's a will...", "If it's not working... try harder",
            "All toasters toast toast", "Sweep it under the rug... It's probably fine", "DO NOT THE COYOTE", "https://www.youtube.com/watch?v=GZ20x4qBJmM", "We should start the winning now",
            "Money will not wait forever", "You cannot hide form pink pony heavy", "It's Perfect", "Is good day for 6s", "You are best of best", "It is good day to be giant man",
            "We make good team", "Hm, is nice", "Sandvich make me strong", "Counter-terrorists win", "Are we rushing, or we going sneaky deaky like", "Easy peasy lemon squeazy", "Bingo bango bongo bish bash bosh"
        };

        public MainWindow()
        {
            InitializeComponent();
            
            _player = new SoundPlayer();
            _isPlaying = false;

            random = new Random();
            SetRandomSplashText();

            animationTimer = new DispatcherTimer();
            animationTimer.Interval = TimeSpan.FromMilliseconds(30); // Update interval
            animationTimer.Tick += AnimateSplashText;
            animationTimer.Start();

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

        private void SetRandomSplashText()
        {
            int index = random.Next(splashTexts.Length);
            SplashTextBlock.Text = splashTexts[index];
        }

        private void AnimateSplashText(object sender, EventArgs e)
        {
            timer += animationTimer.Interval.TotalSeconds;

            // Animate scaling
            double scale = 1 + Math.Sin(timer * Math.PI * 2 / animationDuration) * scaleMultiplier;
            SplashTextBlock.RenderTransform = new ScaleTransform(scale, scale);

            // Animate color change
            byte colorValue = (byte)(255 * (0.5 + 0.5 * Math.Sin(timer * Math.PI * 2 / colorChangeDuration)));
            SplashTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(colorValue, colorValue, 0));

            // Reset timer if animation duration is reached
            if (timer >= animationDuration)
            {
                timer = 0;
                SetRandomSplashText();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
