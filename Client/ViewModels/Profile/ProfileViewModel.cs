
using System;
using System.Collections.Generic;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using UI.Command;
using UI.Components;
using UI.Models;
using UI.Models.Message;
using UI.Network.Packets.AfterLoginRequest.Profile;
using UI.Network.RestAPI;
using UI.Services;
using UI.Utils;

namespace UI.ViewModels {
    public class ProfileViewModel : InitializableViewModel {

        #region Properties

        public string UserID
        {
            get => _appSession.SessionID;
        }

        private bool _CanUpdateProfile;
        public bool CanUpdateProfile {
            get => _CanUpdateProfile;
            set {
                _CanUpdateProfile = value;
                OnPropertyChanged("CanUpdateProfile");
            }
        }

        private void updateConfirmOpacity() {
            int compare = _userProfileHolder.UserProfile.CompareTo(Profile);
            CanUpdateProfile = compare != 0;
        }

        public string FullName {
            get => FirstName + " " + LastName;
        }

        public Gender Gender {
            get => Profile.Gender;
            set {
                Profile.Gender = value;
                updateConfirmOpacity();
                OnPropertyChanged("Gender");
            }
        }

        public string Email {
            get => Profile.Email;
            set {
                Profile.Email = value;
                updateConfirmOpacity();
                OnPropertyChanged("Email");
            }
        }

        public string FirstName {
            get => Profile.FirstName;
            set {
                Profile.FirstName = value;
                updateConfirmOpacity();
                OnPropertyChanged("FirstName");
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string LastName {
            get => Profile.LastName;
            set {
                Profile.LastName = value;
                updateConfirmOpacity();
                OnPropertyChanged("LastName");
                OnPropertyChanged(nameof(FullName));
            }
        }

        public DateTime BirthDay {
            get => Profile.BirthDay;
            set {
                Profile.BirthDay = value;
                updateConfirmOpacity();
                OnPropertyChanged("BirthDay");
            }
        }

        #region ChangeName

        private string _tempFirstName;
        public string TempFirstName {
            get => Profile.FirstName;
            set {
                _tempFirstName = value;
                OnPropertyChanged(nameof(TempFirstName));
            }
        }
        
        private string _tempLastName;
        public string TempLastName {
            get => Profile.LastName;
            set {
                _tempLastName = value;
                OnPropertyChanged(nameof(TempLastName));
            }
        }

        #endregion

        #region ChangePassword

        private string _password;
        public string Password {
            get => _password;
            set {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanChangePassword));
            }
        }
        
        private string _newPassword;
        public string NewPassword {
            get => _newPassword;
            set {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
                OnPropertyChanged(nameof(CanChangePassword));
            }
        }
        
        private string _repeatNewPassword;
        public string RepeatNewPassword {
            get => _repeatNewPassword;
            set {
                _repeatNewPassword = value;
                OnPropertyChanged(nameof(RepeatNewPassword));
                OnPropertyChanged(nameof(CanChangePassword));
            }
        }

        public bool CanChangePassword {
            get => FastCodeUtils.NotEmptyStrings(Password, NewPassword, RepeatNewPassword) &&
                   string.CompareOrdinal(NewPassword, RepeatNewPassword) == 0;
        }

        #endregion

        private UserProfile Profile = new UserProfile();
        private readonly IUserProfileHolder _userProfileHolder;
        private readonly IAppSession _appSession;

        #endregion

        #region Command

        public ICommand UpdateProfileCommand { get; private set; }
        public ICommand ChangeNameCancelCommand { get; private set; }
        public ICommand CleanChangePasswordCommand { get; private set; }
        public ICommand AcceptChangePasswordCommand { get; private set; }
        public ICommand ChangeImageCommand { get; private set; }
        
        #endregion

        public ProfileViewModel(IUserProfileHolder userProfileHolder, IAppSession appSession) : base() {
            _userProfileHolder = userProfileHolder;
            _appSession = appSession;
            UpdateProfileCommand = new RelayCommand<object>(o => CanUpdateProfile,
                o => _userProfileHolder.UpdateUserProfile(Profile));
            ChangeNameCancelCommand = new RelayCommand<object>(null, o => CancelChangeName());
            CleanChangePasswordCommand = new RelayCommand<object>(null, o => CleanChangePassword());
            AcceptChangePasswordCommand = new RelayCommand<object>(null, o => ChangePassword());
            ChangeImageCommand = new RelayCommand<object>(null, o => ChangeAvatar());
            InitializeCommand.Execute(null);
        }

        private void ChangeAvatar()
        {
            List<string> paths = new List<string>();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true) {
                // Open document 
                string Selectedfilename = dlg.FileName;
                paths.Add(Selectedfilename);
                ProfileAPI.AvatarUpload(paths[0], () => {
                    App.Current.Dispatcher.Invoke(() => {
                        AvatarDisplayer.UpdateAllInstance(UserID);
                    });
                }, error => {

                });
            }
        }

        protected override void Initialize(object parameter = null) {
            _userProfileHolder.LoadProfile();
            CloneProfile();
        }

        private void ChangePassword()
        {
            ModifyPassword request = new ModifyPassword();
            request.OldPassword = Password;
            request.NewPassword = NewPassword;
            CleanChangePassword();
            bool res = false;
            DataAPI.getData<ModifyPasswordResult>(request, result =>
            {
                res = result.Result;
            });
            if (res) {
                DialogHost.Show(new AnnouncementDialog(Language.getText("changepass.success")));
            } else {
                DialogHost.Show(new AnnouncementDialog(Language.getText("changepass.Fail")));
            }
        }

        private void CancelChangeName() {
            UserProfile origin = _userProfileHolder.UserProfile;
            if (origin == null)
                return;
            FirstName = origin.FirstName;
            LastName = origin.LastName;
        }

        private void CleanChangePassword() {
            Password = "";
            NewPassword = "";
            RepeatNewPassword = "";
        }

        private void CloneProfile() {
            //Set từng cái để đảm bảo data cập nhật lên view thông qua PropertyChangedTrigger
            if (Profile == null)
                Profile = new UserProfile();
            if (_userProfileHolder.UserProfile == null)
                return;
            Email = _userProfileHolder.UserProfile.Email;
            FirstName = _userProfileHolder.UserProfile.FirstName;
            LastName = _userProfileHolder.UserProfile.LastName;
            BirthDay = _userProfileHolder.UserProfile.BirthDay;
            Gender = _userProfileHolder.UserProfile.Gender;
        }

    }
}