using System;
using System.Windows;
using CNetwork;
using CNetwork.Sessions;
using CNetwork.Utils;
using DotNetty.Buffers;
using UI.Models;
using UI.MVC;

namespace UI.Network.Packets.AfterLoginRequest.Profile
{
    public class GetSelfProfileResult : IPacket
    {
        public UserProfile Profile;

        public void Decode(IByteBuffer buffer)
        {
            Profile.FirstName = ByteBufUtils.ReadUTF8(buffer);
            Profile.LastName = ByteBufUtils.ReadUTF8(buffer);
            Profile.Town = ByteBufUtils.ReadUTF8(buffer);
            Profile.Email = ByteBufUtils.ReadUTF8(buffer);
            Profile.BirthDay = new DateTime(buffer.ReadLong());
            Profile.Gender = (Gender) buffer.ReadInt();
            // Avatar ID
        }

        public IByteBuffer Encode(IByteBuffer byteBuf)
        {
            throw new NotImplementedException();
        }

        public void Handle(ISession session)
        {
            // Display information on setting tab
            var module = ModuleContainer.GetModule<SettingPage>();
            module.controller.updateProfile(Profile);
            /*Application.Current.Dispatcher.Invoke(() =>
            {
                var setting = SettingPage.Instance;
                setting.FirstNameInp.Text = FirstName;
                setting.LastNameInp.Text = LastName;
                setting.Email.Text = Email;
                setting.BirthdayInp.SelectedDate = DateOfBirth;
                setting.GenderInp.SelectedIndex = (int)Gender - 1;
                setting.AddressInp.Text = Town;

                MainWindow.Instance.ProfileContext.FullName.Content = FirstName + " " + LastName;
                // Bind to Avatar displayer
            });*/
        }
    }
}
