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

namespace Project___Seat_Assignment
{
    /// <summary>
    /// Interaction logic for EditFilesWindow.xaml
    /// </summary>
    public partial class EditFilesWindow : Window
    {
        public bool shutdown = false;
        public string filePath;
        public string content;
        public string windowTitle;

        public EditFilesWindow()
        {
            InitializeComponent();
        }

        private void EditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (shutdown)
            { }
            else
            {
                e.Cancel = true;
                if (tbxEdit.Text != content)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to Close this window?\nAll your changes will be undone.", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        tbxEdit.Text = "";
                        EditWindow.Title = "";
                        EditWindow.Hide();
                    }
                }
                else
                {
                    tbxEdit.Text = "";
                    EditWindow.Title = "";
                    EditWindow.Hide();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (tbxEdit.Text != content)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to Close this window?\nAll your changes will be undone.", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result == MessageBoxResult.Yes)
                {
                    tbxEdit.Text = "";
                    EditWindow.Title = "";
                    EditWindow.Hide();
                }
            }
            else
            {
                tbxEdit.Text = "";
                EditWindow.Title = "";
                EditWindow.Hide();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbxEdit.Text != content)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to Save?\nThis will overwrite the current file and may cause problems or break the program.\nThis action can not be undone.", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    System.IO.File.WriteAllText(filePath, tbxEdit.Text);
                    tbxEdit.Text = "";
                    EditWindow.Title = "";
                    EditWindow.Hide();
                }
            }
            else
            {
                tbxEdit.Text = "";
                EditWindow.Title = "";
                EditWindow.Hide();
            }
        }

        private void tbxEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxEdit.Text != content)
                EditWindow.Title = windowTitle + "*";
            else
                EditWindow.Title = windowTitle;
        }
    }
}
