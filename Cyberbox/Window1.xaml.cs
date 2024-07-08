using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// The import window
namespace Cyberbox
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        public Window1()
        {
            InitializeComponent();
        }

        // Open File Dialog on Click
        private void ShowFile_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Opens a message box to confirm if you wanna cancel or not
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Are you sure you want to cancel importing?";
            string caption = "Cancel Import";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        // saves the show file to ram to load into the software
        private void import_click(object sender, RoutedEventArgs e)
        {

        }
    }
}
