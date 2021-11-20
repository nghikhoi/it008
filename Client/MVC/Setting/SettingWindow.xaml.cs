using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Threading;
using System.Windows.Markup;
using UI.Components;
using UI.Lang;
using UI.Models;
using UI.MVC;


namespace UI
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, IView {

        public string FullName {
            get => Profile.FirstName + " " + Profile.LastName;
            set {}
        }

        public Gender Gender { get => Profile.Gender; set => Profile.Gender = value; }
        public string Email { get => Profile.Email; set => Profile.Email = value; }
        public string FirstName { get => Profile.FirstName; set => Profile.FirstName = value; }
        public string LastName { get => Profile.LastName; set => Profile.LastName = value; }
        public DateTime BirthDay { get => Profile.BirthDay; set => Profile.BirthDay = value; }

        private UserProfile OriginalProfile;
        private UserProfile Profile;

        public SettingWindow()
        {
            updateProfile(new UserProfile() {
                Email = "test@gmail.com",
                FirstName = "Nguyen",
                LastName = "A",
                Gender = Gender.Female,
                BirthDay = new DateTime()
            });
            InitializeComponent();
        }

        public void updateProfile(UserProfile profile) {
            OriginalProfile = profile;
            cloneProfile();
        }
        
        private void cloneProfile() {
            //Set từng cái để đảm bảo data cập nhật lên view thông qua PropertyChangedTrigger
            if (Profile == null)
                Profile = OriginalProfile;
            Email = OriginalProfile.Email;
            FirstName = OriginalProfile.FirstName;
            LastName = OriginalProfile.LastName;
            BirthDay = OriginalProfile.BirthDay;
            Gender = OriginalProfile.Gender;
        }

        #region EventHandler

        private void DockPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Image_PreviewMouseDown_close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            // nơi thực hiện các nhiệm vụ background
        }

        private void ChangeNameBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeName changeName = new ChangeName();
            FullFade.Visibility = Visibility.Visible;
            changeName.ShowDialog();
            if (!changeName.IsActive)
            {
                FullFade.Visibility = Visibility.Hidden;
            }

        }

        private void ChangePassWord_btn_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePassWord changePassWord = new ChangePassWord();
            ////Lock backgroundWindow
            //changePassWord.Owner = this;
            FullFade.Visibility = Visibility.Visible;
            changePassWord.ShowDialog();
            if (!changePassWord.IsActive)
            {
                FullFade.Visibility = Visibility.Hidden;
            }
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            HomeWindow module = ModuleContainer.GetModule<ChatWindow>().view;
            module.make_clear();
            this.Close();
        }


        private void deactivate_setting(object sender, EventArgs e)
        {
            HomeWindow module = ModuleContainer.GetModule<ChatWindow>().view;
            module.make_clear();
        }

        #endregion

        private void CbSex_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            Console.WriteLine(Profile.Gender + ":" + Gender);
        }

        private void UsernameBox_OnTextChanged(object sender, TextChangedEventArgs e) {
            Console.WriteLine(Profile.Email + ":" + Email);
        }

        private void SettingWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            cloneProfile();
        }

    }
    
    public class GenderEnum : MarkupExtension {
		
        public Type EnumType { get; set; }

        public GenderEnum(Type enumType) {
            if (enumType == null || !enumType.IsEnum)
                throw new Exception("EnumType must not be null and of type Enum");
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            return Enum.GetValues(EnumType)
                .Cast<object>()
                .Where(e => (Gender) e != Gender.None)
                .Select(e => new { Value = e, Display = Language.getText("General.Gender." + e) });
        }
		
    }
    
}
