﻿using System;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using UI.CustomControls;
using UI.Models;
using UI.MVC;
using UI.Network;
using UI.Network.Packets.Login;
using UI.Network.Packets.Register;
using UI.Utils;

namespace UI.MVC {
	
	public partial class AuthenticationController : IController {

		private WindowLogIn loginView;
		private WindowRegister registerView;
		private string hash;

		public AuthenticationController(WindowLogIn loginView, WindowRegister registerView) {
			this.loginView = loginView;
			this.registerView = registerView;
		}

		public void showLogin() {
			if (registerView.IsVisible)
				registerView.Hide();
			loginView.Show();
		}

		public void showRegister() {
			if (loginView.IsVisible)
				loginView.Hide();
			registerView.Show();
		}

		public void close() {
			if (registerView.IsVisible)
				registerView.Close();
			if (loginView.IsVisible)
				loginView.Close();
		}

		public void doLogin(LoginInfo info) {
			LoginData data = new LoginData();
			data.Username = info.Username;
			data.Passhash = HashUtils.MD5(info.Password);
			_ = ChatConnection.Instance.Send(data);
		}

		public void doRegister(RegisterInfo info) {
			RegisterData packet = new RegisterData();
			packet.Email = info.Username;
			packet.PassHashed = HashUtils.MD5(info.Password);
			packet.FirstName = info.Firstname;
			packet.LastName = info.Lastname;
			packet.DoB = info.DayOfBirth;
			packet.Gender = info.Gender;
			_ = ChatConnection.Instance.Send(packet);
		}
		
		public void EnterMainWindow()
		{
			hash = HashUtils.MD5(loginView.PasswordBox.Password);
			close();
			App.Instance.openHomeWindow(hash);
		}

		public void LoginResponde(int statusCode) {
			DialogHost.CloseDialogCommand.Execute(null, null);
			if (statusCode == LoginRespondeCode.LOGIN_SUCCESS)
			{
				/*AppConfig.Instance.SavedAccount.Clear();
				if (LgRemember && AppConfig.Instance.SavedAccount.TryAdd(LgUserName, lgPassword))
				{
					AppConfig.Save();
				}*/
				EnterMainWindow();
			}
			else
			{
				string[] msgs;
				switch (statusCode)
				{
					case 404:
						msgs = new [] { "Invalid username or password", "Check your info and try again" };
						break;
					case 401:
						msgs = new [] { "Invalid username or password", "Check your info and try again" };
						break;
					case 403:
						msgs = new [] { "Your account got banned", "Please contact the admin to get more information" };
						break;
					default:
						msgs = new [] { "Unhandle status code " + statusCode };
						break;
				}
				Dialogs.openAnnouncement(msgs);
			}
		}

		public void RegisterResponse(int statusCode) {
			DialogHost.CloseDialogCommand.Execute(null, null);
			if (statusCode == 200)
			{
				/*AppConfig.Instance.SavedAccount.Clear();
				if (LgRemember && AppConfig.Instance.SavedAccount.TryAdd(LgUserName, lgPassword))
				{
					AppConfig.Save();
				}*/
				showLogin();
			}
			else
			{
				string[] msgs;
				switch (statusCode)
				{
					case 404:
						msgs = new[] { Lang.Language.getText("Authentication.InvalidUser") };
						break;
					case 401:
						msgs = new [] {Lang.Language.getText("Authentication.InvalidUser") };
						break;
					case 403:
						msgs = new [] { Lang.Language.getText("Authentication.Banned") };
						break;
					default:
						msgs = new [] { Lang.Language.getText("Authentication.Unhandled") };
						break;
				}
				Dialogs.openAnnouncement(msgs);
			}
		}

	}

}