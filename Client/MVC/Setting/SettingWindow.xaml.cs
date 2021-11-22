using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using UI.Annotations;
using UI.Lang;
using UI.Models;
using UI.MVC;
using UI.Network.Packets;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.RestAPI;

namespace UI
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window, IView, INotifyPropertyChanged {

        private bool _CanUpdateProfile;

        public bool CanUpdateProfile {
            get => _CanUpdateProfile;
            set {
                _CanUpdateProfile = value;
                OnPropertyChanged();
            }
        }

        private void updateConfirmOpacity() {
            int compare = OriginalProfile.CompareTo(Profile);
            CanUpdateProfile = compare != 0;
        }

        public string FullName {
            get => FirstName + " " + LastName;
            set {
                updateConfirmOpacity();
                OnPropertyChanged();
            } 
        }

        public Gender Gender {
            get => Profile.Gender;
            set {
                Profile.Gender = value;
                updateConfirmOpacity();
                OnPropertyChanged();
            }
        }

        public string Email {
            get => Profile.Email;
            set {
                Profile.Email = value;
                updateConfirmOpacity();
                OnPropertyChanged();
            }
        }

        public string FirstName {
            get => Profile.FirstName;
            set {
                Profile.FirstName = value;
                updateConfirmOpacity();
                OnPropertyChanged();
            }
        }

        public string LastName {
            get => Profile.LastName;
            set {
                Profile.LastName = value;
                updateConfirmOpacity();
                OnPropertyChanged();
            }
        }

        public DateTime BirthDay {
            get => Profile.BirthDay;
            set {
                Profile.BirthDay = value;
                updateConfirmOpacity();
                OnPropertyChanged();
            }
        }

        private UserProfile OriginalProfile;
        private UserProfile Profile = new UserProfile();
        public string UserID {
            get => ChatModel.Instance.SelfID;
            set {}
        }

        public SettingWindow()
        {
            if (App.IS_LOCAL_DEBUG)
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
                Profile = new UserProfile();
            if (OriginalProfile == null)
                return;
            Email = OriginalProfile.Email;
            FirstName = OriginalProfile.FirstName;
            LastName = OriginalProfile.LastName;
            BirthDay = OriginalProfile.BirthDay;
            Gender = OriginalProfile.Gender;
            FullName = " "; //Call to update
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
            ChangeName changeName = new ChangeName(FirstName, LastName);
            FullFade.Visibility = Visibility.Visible;
            if (changeName.ShowDialog() == true) {
                FirstName = changeName.FirstName;
                LastName = changeName.LastName;
                FullName = " "; //Call to update
            }
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

        private void SettingWindow_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            cloneProfile();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateButton_OnClick(object sender, RoutedEventArgs e) {
            if (!CanUpdateProfile) {
                return;
            }

            UpdateSelfProfile request = new UpdateSelfProfile();
            request.Gender = Profile.Gender;
            request.Town = Profile.Town;
            request.FirstName = Profile.FirstName;
            request.LastName = Profile.LastName;
            request.DateOfBirth = Profile.BirthDay;
            DataAPI.getData<GetSelfProfileResult>(request, result => {
                ChatModel model = ChatModel.Instance;
                model.Profile = result.Profile;
                updateProfile(result.Profile);
            });
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
