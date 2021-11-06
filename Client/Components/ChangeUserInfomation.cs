using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Command;
using UI.Models;

namespace UI.Components
{
    class ChangeUserInfomation : BaseComponent
    {
        public ICommand ClickImageCommand { get; set; }
        public ICommand ClickChangeImageCommand { get; set; }
        public ICommand RunDialogCommand { get; set; }

        private string _visibility;
        public string Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }
        private string _image;
        public string Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged();
            }
        }
        public ChangeUserInfomation()
        {
            user_infomation userinfo = new user_infomation();
            Visibility = "Collapsed";
            Image = "";
            ClickImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                if (!Visibility.Equals("Collapsed"))
                    Visibility = "Collapsed";
                else
                    Visibility = "Visible";
            });
            ClickChangeImageCommand = new RelayCommand<object>(
            (p) => { return true; },
            (p) =>
            {
                OpenFileDialog openfile = new OpenFileDialog
                {
                    Title = "Select a picture",
                    Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" + "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" + "Portable Network Graphic (*.png)|*.png"
                };
                if (openfile.ShowDialog() == true)
                {
                    Image = openfile.FileName;
                    Visibility = "Collapsed";
                }
            });
        }
    }
}
