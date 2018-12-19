using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project___Seat_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        EditFilesWindow editWindow = new EditFilesWindow();
        Dictionary<int,User> Users_List = new Dictionary<int, User>();
        Dictionary<int,Clan> Clans_List = new Dictionary<int, Clan>();
        Dictionary<int,Seat> Seats_List = new Dictionary<int, Seat>();
        List<Brush> Colors_List;
        List<TextBox> SeatsTextBoxes;
        int selectedSeat, hoveredSeat, selectedClan, selectedUser = 0;
        bool seatHoverMode = true;
        const int TOTALSEATS = 134;
        LinearGradientBrush lgb;

        public MainWindow()
        {
            InitializeComponent();
            SeatsTextBoxes = new List<TextBox>() { Seat_1, Seat_2, Seat_3, Seat_4, Seat_5, Seat_6, Seat_7, Seat_8, Seat_9, Seat_10, Seat_11, Seat_12, Seat_13, Seat_14, Seat_15, Seat_16, Seat_17, Seat_18, Seat_19, Seat_20, Seat_21, Seat_22, Seat_23, Seat_24, Seat_25, Seat_26, Seat_27, Seat_28, Seat_29, Seat_30, Seat_31, Seat_32, Seat_33, Seat_34, Seat_35, Seat_36, Seat_37, Seat_38, Seat_39, Seat_40, Seat_41, Seat_42, Seat_43, Seat_44, Seat_45, Seat_46, Seat_47, Seat_48, Seat_49, Seat_50, Seat_51, Seat_52, Seat_53, Seat_54, Seat_55, Seat_56, Seat_57, Seat_58, Seat_59, Seat_60, Seat_61, Seat_62, Seat_63, Seat_64, Seat_65, Seat_66, Seat_67, Seat_68, Seat_69, Seat_70, Seat_71, Seat_72, Seat_73, Seat_74, Seat_75, Seat_76, Seat_77, Seat_78, Seat_79, Seat_80, Seat_81, Seat_82, Seat_83, Seat_84, Seat_85, Seat_86, Seat_87, Seat_88, Seat_89, Seat_90, Seat_91, Seat_92, Seat_93, Seat_94, Seat_95, Seat_96, Seat_97, Seat_98, Seat_99, Seat_100, Seat_101, Seat_102, Seat_103, Seat_104, Seat_105, Seat_106, Seat_107, Seat_108, Seat_109, Seat_110, Seat_111, Seat_112, Seat_113, Seat_114, Seat_115, Seat_116, Seat_117, Seat_118, Seat_119, Seat_120, Seat_121, Seat_122, Seat_123, Seat_124, Seat_125, Seat_126, Seat_127, Seat_128, Seat_129, Seat_130, Seat_131, Seat_132, Seat_133, Seat_134 };
            lgb = CreateLinearGradientBrush();
            Colors_List = new List<Brush>() { Brushes.Yellow, Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Violet, Brushes.Orange, Brushes.YellowGreen, Brushes.Navy, Brushes.OrangeRed, Brushes.LightGreen, Brushes.Magenta, Brushes.Gold, lgb, Brushes.HotPink, Brushes.DeepSkyBlue, Brushes.OliveDrab };
            ReloadData();
            UpdateComboBox_cbxSelectClan();
            UpdateComboBox_cbxCUClan();
            UpdateSeatsOnImage();
            UpdateLegend();
        }

        private void SaveToFiles()
        {
            string[] userLinesToSave = new string[Users_List.Count];
            for (int i = 1; i <= Users_List.Count; i++)
                userLinesToSave[i - 1] = $"{Users_List[i].ID};{Users_List[i].Username};{Users_List[i].Clan.ClanName};{Users_List[i].AssignedSeat};{Users_List[i].Voornaam};{Users_List[i].Achternaam}";
            System.IO.File.WriteAllLines("Users.csv", userLinesToSave);

            List<string> tempclans = new List<string>();
            foreach (KeyValuePair<int,Clan> c in Clans_List)
            {
                tempclans.Add(c.Value.ClanName);
            }
            string clansToSave = String.Join(";", tempclans);
            System.IO.File.WriteAllText("Clans.csv", clansToSave);
        }

        private void ReloadData()
        {
            Users_List.Clear();
            Clans_List.Clear();
            Seats_List.Clear();

            string[] Users = System.IO.File.ReadAllLines("Users.csv");
            string[] Clans = System.IO.File.ReadAllText("Clans.csv").Split(';');

            // Clans zonder userList maken
            for (int i = 0; i < Clans.Length; i++)
            {
                Clans_List.Add(i + 1, new Clan(i + 1, Clans[i], Colors_List[i], new List<User>()));
            }

            // UserList maken
            foreach (string s in Users)
            {
                Clan tempClan = new Clan(-1, "No Clan", Brushes.White, new List<User>());
                string[] user = s.Split(';');
                foreach (KeyValuePair<int, Clan> clan in Clans_List)
                    if (clan.Value.ClanName == user[2])
                        tempClan = clan.Value;
                int tempseat = (user[3] != "0"? Convert.ToInt32(user[3]) : 0);
                Users_List.Add(Convert.ToInt32(user[0]), new User(Convert.ToInt32(user[0]), user[1], tempClan, tempseat, user[4], user[5]));
            }

            // SeatsList aanmaken
            for (int i = 1; i <= TOTALSEATS; i++)
            {
                User occupier = null;
                foreach (KeyValuePair<int, User> u in Users_List)
                    if (u.Value.AssignedSeat == i)
                        occupier = u.Value;
                Seats_List.Add(i, new Seat(i, occupier, SeatsTextBoxes[i - 1]));
            }

            // userList en userCount aan Clans toevoegen
            foreach (KeyValuePair<int, Clan> clan in Clans_List)
            {
                List<User> tempPlayerlist = new List<User>();
                for (int i = 1; i <= Users_List.Count; i++)
                    if (Users_List[i].Clan.ClanName == clan.Value.ClanName)
                        tempPlayerlist.Add(Users_List[i]);
                clan.Value.Users = tempPlayerlist;
            }
        }


        // UpdateStuffEnVariabelen
        private void UpdateSeatsOnImage()
        {
            for (int i = 1; i <= TOTALSEATS; i++)
            {
                Seats_List[i].SeatTextBox.Background = Seats_List[i].ClanColor;
            }
        }

        private void UpdateLegend()
        {
            spColors.Children.Clear();
            spClans.Children.Clear();

            spColors.Children.Add(new TextBox() { Background = Brushes.White, Padding = new Thickness(0, 0, 0, 0), Focusable = false, Cursor = Cursors.Arrow, BorderBrush = Brushes.Black, Height = 14.5 });
            spClans.Children.Add(new TextBox() { Text = "Empty seat", Focusable = false, Cursor = Cursors.Arrow, FontSize = 10, VerticalContentAlignment = VerticalAlignment.Center, HorizontalContentAlignment = HorizontalAlignment.Left, BorderBrush = Brushes.White, Height = 14.5 });
            for (int i = 1; i <= Clans_List.Count; i++)
            {
                spColors.Children.Add(new TextBox() { Background = Colors_List[i - 1], Padding = new Thickness(0, 0, 0, 0), Focusable = false, Cursor = Cursors.Arrow, BorderBrush = Brushes.Black, Height = 14.5 });
                spClans.Children.Add(new TextBox() { Text = Clans_List[i].ClanName, Tag = Clans_List[i], Focusable = false, Cursor = Cursors.Arrow, FontSize = 10, VerticalContentAlignment = VerticalAlignment.Top, HorizontalContentAlignment = HorizontalAlignment.Left, BorderBrush = null, Height = 14.5 });
            }
        }

        private void UpdateComboBox_cbxSelectClan()
        {
            cbxSelectClan.Items.Clear();
            foreach (KeyValuePair<int, Clan> c in Clans_List)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = c.Value.ClanName;
                cbi.Tag = c.Value;
                cbi.Background = c.Value.ClanColor;
                cbxSelectClan.Items.Add(cbi);
            }
            if (selectedClan != 0)
            {
                Clan tempClan = null;
                foreach (KeyValuePair<int, Clan> c in Clans_List)
                {
                    if (Clans_List[selectedClan].ClanName == c.Value.ClanName)
                        tempClan = c.Value;
                }
                if (tempClan != null)
                    cbxSelectClan.SelectedIndex = Clans_List.Last().Key - 1;
            }
        }

        private void UpdateComboBox_cbxCUClan()
        {
            cbxCUClan.Items.Clear();
            foreach (KeyValuePair<int, Clan> c in Clans_List)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = c.Value.ClanName;
                cbi.Tag = c.Value;
                cbi.Background = c.Value.ClanColor;
                cbxCUClan.Items.Add(cbi);
            }
        }

        private void UpdateListBox_lbxSelectedUserItems()
        {
            lbxSelectUser.Items.Clear();

            if (selectedClan != 0)
            {
                foreach (User u in Clans_List[selectedClan].Users)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    lbi.Content = u.Username;
                    lbi.Tag = u;
                    lbi.Background = (u.AssignedSeat != 0 ? Brushes.GreenYellow : Brushes.White);
                    lbxSelectUser.Items.Add(lbi);
                }
            }
        }

        private void UpdateSelectedSeat(int pSelectedSeat)  // SelectedStuff links
        {
            selectedSeat = pSelectedSeat;

            // Seat geselecteerd
            if (selectedSeat > 0)
            {
                lblSelectedSeat.Content = selectedSeat.ToString();
                lblSelectedSeat.Foreground = (Seats_List[selectedSeat].Occupier == null ? Brushes.GreenYellow : Brushes.Red);
                ShowSeatInfo(selectedSeat);
            }
            // geen Seat geselecteerd
            else if (selectedSeat == 0)
            {
                RemoveSeatInfo();
                seatHoverMode = true;
            }
        }

        private void UpdateSelectedUser(int pSelectedUser) // SelectedStuff rechts
        {
            selectedUser = pSelectedUser;

            // User geselecteerd
            if (selectedUser != 0)
            {
                string seat = (Users_List[selectedUser].AssignedSeat == 0 ? "None" : Users_List[selectedUser].AssignedSeat.ToString());
                lblSelectedUser.Content = Users_List[selectedUser].Username;
                tbkUserInfo.Text = "User ID: " + Users_List[selectedUser].ID +
                    "\nUsername: " + Users_List[selectedUser].Username +
                    $"\nName: { Users_List[selectedUser].Voornaam} { Users_List[selectedUser].Achternaam}" +
                    "\nAssigned Seat: " + seat;
                lblCurrentSeat.Content = seat;
            }
            // geen User geselecteerd
            else
            {
                lblSelectedUser.Content = "";
                tbkUserInfo.Text = "";
                lblCurrentSeat.Content = "";
            }
        }

        private void UpdateSelectedClan(int pSelectedClan) // SelectedStuff rechts
        {
            selectedClan = pSelectedClan;

            ShowClanInfo(selectedClan);
            UpdateListBox_lbxSelectedUserItems();

            // Clan geselecteerd
            if (selectedClan != 0)
            {
                lblClanColor.Background = Clans_List[selectedClan].ClanColor;
                lblSelectedClan.Content = Clans_List[selectedClan].ClanName;
                lblTotalUsers.Content = Clans_List[selectedClan].Users.Count.ToString();
            }
            // geen Clan geselecteerd
            else
            {
                lblClanColor.Background = Brushes.White;
                lblSelectedClan.Content = "";
                lblTotalUsers.Content = "";
            }
        }


        // LinearGradientBrush maken voor fancy kleurtje
        private LinearGradientBrush CreateLinearGradientBrush()
        {
            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.EndPoint = new Point(0.5, 1);
            lgb.MappingMode = BrushMappingMode.RelativeToBoundingBox;
            lgb.StartPoint = new Point(0.5, 0);
            TransformGroup transGroup = new TransformGroup();
            transGroup.Children.Add(new ScaleTransform(1, 1, 0.5, 0.5));
            transGroup.Children.Add(new SkewTransform(0, 0, 0.5, 0.5));
            lgb.Transform = transGroup;
            GradientStopCollection gsc = new GradientStopCollection();
            gsc.Add(new GradientStop(Colors.Yellow, 0.14));
            gsc.Add(new GradientStop(Colors.Blue, 1));
            lgb.GradientStops = gsc;
            return lgb;
        }


        private void imgZaalplan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateSelectedSeat(0);
        }


        // Menu Items
        private void MenuItem_Reload_Files(object sender, RoutedEventArgs e)
        {
            ReloadData();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            UpdateLegend();
            UpdateSeatsOnImage();
        }

        private void MenuItem_EUsers_Click(object sender, RoutedEventArgs e)
        {
            editWindow.filePath = "Users.csv";
            editWindow.tbxEdit.Text = System.IO.File.ReadAllText(editWindow.filePath);
            editWindow.content = editWindow.tbxEdit.Text;
            editWindow.Title = @"Edit Users - Users.csv";
            editWindow.windowTitle = editWindow.Title;
            editWindow.Show();
        }

        private void MenuItem_EClans_Click(object sender, RoutedEventArgs e)
        {
            editWindow.filePath = "Clans.csv";
            editWindow.tbxEdit.Text = System.IO.File.ReadAllText(editWindow.filePath);
            editWindow.content = editWindow.tbxEdit.Text;
            editWindow.Title = @"Edit Clans - Clans.csv";
            editWindow.windowTitle = editWindow.Title;
            editWindow.Show();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            CloseProject();
        }


        // Seat Info
        private void Seat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tempTbx = (TextBox)sender;
            UpdateSelectedSeat(Convert.ToInt32(tempTbx.Text));
            seatHoverMode = false;
        }

        private void Seat_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBox tempTbx = (TextBox)sender;
            hoveredSeat = Convert.ToInt32(tempTbx.Text);
            if (seatHoverMode)
                ShowSeatInfo(hoveredSeat);
        }

        private void Seat_MouseLeave(object sender, MouseEventArgs e)
        {
            if (seatHoverMode)
                RemoveSeatInfo();
            hoveredSeat = 0;
        }

        private void ShowSeatInfo(int seatNumber)
        {
            User occupier = Seats_List[seatNumber].Occupier;
            if (occupier != null)
            {
                tbkSeatInfo.Text = "\nSeat " + seatNumber
                    + "\nUser: " + occupier.Username
                    + "\nClan: " + occupier.Clan.ClanName;
            }
            else
            {
                tbkSeatInfo.Text = "\nThis seat is empty.\nEmpty seats don't have a user assigned.";
            }
        }

        private void RemoveSeatInfo()
        {
            tbkSeatInfo.Text = "\nNo seat selected.";
            lblSelectedSeat.Content = "";
        }


        // Clan Info
        private void ShowClanInfo(int pSelectedClan)
        {
            if (pSelectedClan != 0)
            {
                tbkClanInfo.Text = "\nClan: " + Clans_List[pSelectedClan].ClanName
                    + "\nUser count: " + Clans_List[pSelectedClan].Users.Count;
            }
            else
                tbkClanInfo.Text = "\nEmpty seats don't have a clan assigned,\nbecause there is no user assigned.";
        }


        // Clan ComboBox
        private void cbxSelectClan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = (ComboBoxItem)cbxSelectClan.SelectedItem;
            if (cbi != null)
            {
                Clan tempClan = (Clan)cbi.Tag;
                UpdateSelectedClan(tempClan.ID);
                UpdateSelectedUser(0);
            }
        }

        // Users ListBox
        private void lbxSelectUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem lbi = (ListBoxItem)lbxSelectUser.SelectedItem;
            if (lbi != null)
            {
                User tempUser = (User)lbi.Tag;
                UpdateSelectedUser(tempUser.ID);
            }
        }


        private void btnAssignSeat_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeat != 0 && selectedUser != 0)
            {
                if (Seats_List[selectedSeat].Occupier == null)
                {
                    if (Users_List[selectedUser].AssignedSeat != 0)
                        Seats_List[Users_List[selectedUser].AssignedSeat].Occupier = null;
                    Seats_List[selectedSeat].Occupier = Users_List[selectedUser];
                    Users_List[selectedUser].AssignedSeat = selectedSeat;
                    SaveToFiles();
                    ReloadData();
                    UpdateListBox_lbxSelectedUserItems();
                    UpdateSelectedUser(selectedUser);
                    UpdateSelectedSeat(selectedSeat);
                    UpdateSeatsOnImage();
                }
            }
        }

        private void btnRemoveSeat_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != 0 && Users_List[selectedUser].AssignedSeat != 0)
            {
                Seats_List[Users_List[selectedUser].AssignedSeat].Occupier = null;
                Users_List[selectedUser].AssignedSeat = 0;
                SaveToFiles();
                ReloadData();
                UpdateListBox_lbxSelectedUserItems();
                UpdateSelectedUser(selectedUser);
                UpdateSelectedSeat(selectedSeat);
                UpdateSeatsOnImage();
            }
        }


        // Create Tabs
        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxCUUsername.Text))
            {
                bool available = true;
                foreach (KeyValuePair<int, User> u in Users_List)
                    if (u.Value.Username == tbxCUUsername.Text)
                        available = false;
                if (available)
                {
                    if (cbxCUClan.SelectedItem != null)
                    {
                        if (!string.IsNullOrWhiteSpace(tbxCUVoornaam.Text) && !string.IsNullOrWhiteSpace(tbxCUAchternaam.Text))
                        {
                            ComboBoxItem cbi = (ComboBoxItem)cbxCUClan.SelectedItem;
                            Clan tempClan = (Clan)cbi.Tag;
                            int id = Users_List.Count + 1;
                            User tempUser = new User(id, tbxCUUsername.Text, tempClan, tbxCUVoornaam.Text, tbxCUAchternaam.Text);
                            Users_List.Add(id, tempUser);
                            tempClan.Users.Add(tempUser);
                            SaveToFiles();
                            if (tempClan.ClanName == Clans_List[selectedClan].ClanName)
                            {
                                AddUserToListBox_lbxSelectUser(tempUser);
                                UpdateSelectedClan(selectedClan);
                            }
                            ResetCreateUserTab();
                            UpdateSeatsOnImage();
                            ShowClanInfo(selectedClan);
                            MessageBox.Show($"User '{tempUser.Username}' successfully created.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                            lblCUErrors.Content = "First name and Last name can not be empty!";
                    }
                    else
                        lblCUErrors.Content = "Please select a clan to join or create a new clan.";
                }
                else
                    lblCUErrors.Content = "Username is already used!";
            }
            else
                lblCUErrors.Content = "Username can not be empty!";
        }

        private void btnCreateClan_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxCCClanname.Text))
            {
                bool available = true;
                foreach (KeyValuePair<int, Clan> c in Clans_List)
                    if (c.Value.ClanName.ToLower() == tbxCCClanname.Text.ToLower())
                        available = false;
                if (available)
                {
                    Brush color = Colors_List[Clans_List.Count];
                    int id = Clans_List.Count + 1;
                    Clan tempClan = new Clan(id ,tbxCCClanname.Text, color);
                    Clans_List.Add(id, tempClan);
                    SaveToFiles();
                    ResetCreateClanTab();
                    UpdateLegend();
                    MessageBox.Show($"Clan '{tempClan.ClanName}' successfully created.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                    lblCCErrors.Content = "Clan name is already in use!";
            }
            else
                lblCCErrors.Content = "Clan name can not be empty!";
        }


        // Reset Tabs
        private void ResetCreateUserTab()
        {
            tbxCUUsername.Text = "";
            tbxCUVoornaam.Text = "";
            tbxCUAchternaam.Text = "";
            lblCUErrors.Content = "";
            UpdateComboBox_cbxCUClan();
        }

        private void ResetCreateClanTab()
        {
            tbxCCClanname.Text = "";
            lblCCErrors.Content = "";
            AddClanToComboBoxes(Clans_List.Last().Value);
        }

        private void AddUserToListBox_lbxSelectUser(User pUser)
        {
            ListBoxItem lbi = new ListBoxItem();
            lbi.Content = pUser.Username;
            lbi.Tag = pUser;
            lbxSelectUser.Items.Add(lbi);
        }

        private void AddClanToComboBoxes(Clan pClan)
        {
            ComboBoxItem cbi1 = new ComboBoxItem();
            ComboBoxItem cbi2 = new ComboBoxItem();
            cbi1.Content = pClan.ClanName;
            cbi2.Content = pClan.ClanName;
            cbi1.Tag = pClan;
            cbi2.Tag = pClan;
            cbi1.Background = pClan.ClanColor;
            cbi2.Background = pClan.ClanColor;
            cbxSelectClan.Items.Add(cbi1);
            cbxCUClan.Items.Add(cbi2);
        }

        // Closing stuff
        private void Mainwindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CloseProject();
        }

        private void CloseProject()
        {
            editWindow.shutdown = true;
            editWindow.Close();
            Application.Current.Shutdown();
        }
    }
}
