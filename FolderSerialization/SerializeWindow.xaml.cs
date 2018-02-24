using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;

namespace FolderSerialization
{
    /// <summary>
    /// Interaction logic for SerializeWindow.xaml
    /// </summary>
    public partial class SerializeWindow : Window
    {
        private string pathToDeserialize = ConfigurationManager.AppSettings["path"];

        public SerializeWindow()
        {
            InitializeComponent();
            destinationPath.Text = pathToDeserialize;
        }

        private void Serialize_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                var path = System.IO.Path.Combine(pathToDeserialize,
                    FileName.Text);
                path = string.Concat(path, ".dat");
                if (result.ToString() == "OK")
                {
                    Helper.Serialize(dialog.SelectedPath, path);
                    System.Windows.MessageBox.Show("Object was serialized", "Info");
                    Close();
                }
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Change_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    destinationPath.Text = dialog.SelectedPath;
                    pathToDeserialize = dialog.SelectedPath;
                }
            }
        }
    }
}
