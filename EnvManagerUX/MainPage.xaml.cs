using EnvManagerUX.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EnvManagerUX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string userglyph = "\uE13D";
        string systemglyph = "\uE977";
        ObservableCollection<EnvVar> userList = new ObservableCollection<EnvVar>();
        ObservableCollection<EnvVar> systemList = new ObservableCollection<EnvVar>();
        ObservableCollection<EnvVar> allList = new ObservableCollection<EnvVar>();
        public MainPage()
        {
            this.InitializeComponent();
            userList.Add(new EnvVar() { Icon = userglyph, Name = "OneDrive", Value = @"C:\Users\nielslaute\OneDrive - Microsoft" });
            userList.Add(new EnvVar() { Icon = userglyph, Name = "OneDriveCommercial", Value = @"C:\Users\nielslaute\OneDrive - Microsoft" });
            userList.Add(new EnvVar() { Icon = userglyph, Name = "OneDriveConsumer", Value = @"C:\Users\nielslaute\OneDrive" });
            userList.Add(new EnvVar() { Icon = userglyph, Name = "Path", Value = @"%USERPROFILE%\AppData\Local\Microsoft\WindowsApps" });
            userList.Add(new EnvVar() { Value = @"C:\Users\nielslaute\AppData\Local\GitHubDesktop\bin" });
            userList.Add(new EnvVar() { Value = @"C:\Users\nielslaute\AppData\Local\Programs\Microsoft VS Code\bin" });
            userList.Add(new EnvVar() { Value = @"%USERPROFILE%\.dotnet\tools" });
            userList.Add(new EnvVar() { Icon = userglyph, Name = "TEMP", Value = @"%USERPROFILE%\AppData\Local\Temp" });
            userList.Add(new EnvVar() { Icon = userglyph, Name = "TMP", Value = @"%USERPROFILE%\AppData\Local\Temp" });

            systemList.Add(new EnvVar() { Icon = systemglyph, Name = "ComSpec", Value = @"%SystemRoot%\system32\cmd.exe" });
            systemList.Add(new EnvVar() { Icon = systemglyph, Name = "DriverData", Value = @"C:\Windows\System32\Drivers\DriverData" });
            systemList.Add(new EnvVar() { Icon = systemglyph, Name = "OS", Value = @"Windows_NET" });

            systemList.Add(new EnvVar() { Icon = systemglyph, Name = "JAVA_HOME", Value = @"C:\Program Files\Java\8\jdk1.8.0_151" });
            systemList.Add(new EnvVar() { Icon = systemglyph, Name = "javaroot", Value = "%JAVA_HOME%\bin" });

            //java10List.Add(new EnvVar() { Icon = systemglpyh, Name = "JAVA_HOME", Value = @"C:\Program Files\Java\10\jdk1.8.0_151" });
            //java10List.Add(new EnvVar() { Icon = userglyph, Name = "javaroot", Value = "%JAVA_HOME%\bin" });

            foreach (var item in userList)
            {
                allList.Add(item);
            }

            foreach (var item in systemList)
            {
                allList.Add(item);
            }
        }

        private void List_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        private async void List_Drop(object sender, DragEventArgs e)
        {
            ListView target = (ListView)sender;

            if (e.DataView.Contains(StandardDataFormats.Text))
            {
                DragOperationDeferral def = e.GetDeferral();
                string s = await e.DataView.GetTextAsync();
                string[] items = s.Split('\n');
                foreach (string item in items)
                {

                    // Create Contact object from string, add to existing target ListView
                    string[] info = item.Split(" ", 3);
                    EnvVar temp = new EnvVar();
                    temp.Icon = info[0];
                    temp.Name = info[1];
                    temp.Value = info[2];
    

                    // Find the insertion index:
                    Windows.Foundation.Point pos = e.GetPosition(target.ItemsPanelRoot);

                    // If the target ListView has items in it, use the heigh of the first item
                    //      to find the insertion index.
                    int index = 0;
                    if (target.Items.Count != 0)
                    {
                        // Get a reference to the first item in the ListView
                        ListViewItem sampleItem = (ListViewItem)target.ContainerFromIndex(0);

                        // Adjust itemHeight for margins
                        double itemHeight = sampleItem.ActualHeight + sampleItem.Margin.Top + sampleItem.Margin.Bottom;

                        // Find index based on dividing number of items by height of each item
                        index = Math.Min(target.Items.Count - 1, (int)(pos.Y / itemHeight));

                        // Find the item being dropped on top of.
                        ListViewItem targetItem = (ListViewItem)target.ContainerFromIndex(index);

                        // If the drop position is more than half-way down the item being dropped on
                        //      top of, increment the insertion index so the dropped item is inserted
                        //      below instead of above the item being dropped on top of.
                        Windows.Foundation.Point positionInItem = e.GetPosition(targetItem);
                        if (positionInItem.Y > itemHeight / 2)
                        {
                            index++;
                        }

                        // Don't go out of bounds
                        index = Math.Min(target.Items.Count, index);
                    }
                    // Only other case is if the target ListView has no items (the dropped item will be
                    //      the first). In that case, the insertion index will remain zero.

                    //// Find correct source list
                    //if (target.Name == "AllVars")
                    //{
                    //    userList.Insert(index, temp);
                    //    foreach (EnvVar contact in SystemListView.Items)
                    //    {
                    //        if (contact.Icon == temp.Icon && contact.Name == temp.Name && contact.Value == temp.Value)
                    //        {
                    //            java8List.Remove(contact);
                    //            break;
                    //        }
                    //    }
                    //}
                    //else if (target.Name == "Java8ListView")
                    //{
                    //    java8List.Insert(index, temp);
                    //    foreach (EnvVar contact in userList.Items)
                    //    {
                    //        if (contact.Icon == temp.Icon && contact.Name == temp.Name && contact.Value == temp.Value)
                    //        {
                    //            SystemListView.Remove(contact);
                    //            break;
                    //        }
                    //    }
                    //}
                }

                e.AcceptedOperation = DataPackageOperation.Move;
                def.Complete();
            }
        }

        private void List_DragEnter(object sender, DragEventArgs e)
        {
            // We don't want to show the Move icon
            e.DragUIOverride.IsGlyphVisible = false;
        }


        private void List_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            if (e.Items.Count == 1)
            {
                // Prepare ListViewItem to be moved
                EnvVar tmp = (EnvVar)e.Items[0];

                e.Data.SetText(tmp.Icon + " " + tmp.Name + " " + tmp.Value);
                e.Data.RequestedOperation = DataPackageOperation.Move;
            }
        }

        private async void VarButton_Click(object sender, RoutedEventArgs e)
        {
           NameBox.Text = "";
           ValueBox.Text = "";
            var result = await NewDialog.ShowAsync();
        }

        private async void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var result = await ProfileDialog.ShowAsync();
        }

        private void NewDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            EnvVar x = new EnvVar();
            x.Name = NameBox.Text;
            x.Value = ValueBox.Text;

            if (TypeBox.SelectedIndex == 0)
            {
                x.Icon = userglyph;
                userList.Add(x);
            }
            else
            {
                x.Icon = systemglyph;
                systemList.Add(x);
            }

        
            allList.Add(x);
        }
    }
}


