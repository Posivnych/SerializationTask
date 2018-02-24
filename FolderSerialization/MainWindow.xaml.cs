using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace FolderSerialization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private (string[] directories, (string name, byte[] data)[] files) displayedDirectories;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Serialize_Button_Click(object sender, RoutedEventArgs e)
        {
            SerializeWindow window = new SerializeWindow();
            window.Owner = this;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.ShowDialog();
        }

        private void Display_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Binary files |*.dat|All files (*.*)|*.*";
                var result = dialog.ShowDialog();
                var s = result.ToString();
                if (result.ToString() == string.Empty ||
                    result.ToString() == "Cancel")
                {
                    System.Windows.MessageBox
                        .Show("Please select correct file!", "Info");
                }
                else
                {
                    displayedDirectories = Helper.Deserialize(dialog.FileName);
                    foreach (var a in displayedDirectories.directories)
                    {
                        displayedFolders.Items.Add(a);
                    }
                }
            }
        }

        private void Deserialize_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                if (result.ToString() == "OK")
                {
                    if (displayedFolders.Items.Count == 0)
                    {
                        System.Windows.MessageBox.Show(
                            "Please press display and select file to deserialize!",
                            "Info");
                        return;
                    }

                    if (displayedFolders.SelectedItems.Count == 0)
                    {
                        Helper.Unpack(dialog.SelectedPath, displayedDirectories);
                    }
                    else
                    {
                        var selectedDirectories = displayedFolders.
                            SelectedItems.Cast<string>().ToArray();
                        var neededDirectories = new List<string>();
                        foreach (var dir in selectedDirectories)
                        {
                            neededDirectories.AddRange(displayedDirectories.
                                directories.
                                Where(x => x.
                                IndexOf(dir) == 0));
                        }

                        List<(string name, byte[] data)> files =
                            new List<(string name, byte[] data)>();
                        foreach (var element in selectedDirectories)
                        {
                            files.AddRange(displayedDirectories.files.
                                Where(x => x.name.Contains(element)).
                                ToList());
                        }

                        Helper.Unpack(dialog.SelectedPath,
                            (neededDirectories.ToArray(), files.ToArray()));
                    }
                }
            }
        }
    }
}
