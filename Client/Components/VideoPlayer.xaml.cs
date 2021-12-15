using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;

namespace UI.Components {
	/// <summary>
	/// Interaction logic for VideoWindow.xaml
	/// </summary>
	public partial class VideoPlayer : UserControl {
		DispatcherTimer timer;
		bool isDragging = false, mediaPlaying = false;
		public double playinit { get; set; } = 0;

		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(Uri)
			, typeof(VideoPlayer), new FrameworkPropertyMetadata(null));
		public Uri Source {
			get => (Uri) GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}
		
		public VideoPlayer() {
			InitializeComponent();
			timer = new DispatcherTimer();
			timer.Interval = TimeSpan.FromMilliseconds(200);
			timer.Tick += new EventHandler(timer_tick);
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e) {
			if (e.Property == SourceProperty) {
				SetSource(Source);
			}
			base.OnPropertyChanged(e);
		}

		public void SetSource(Uri uri)
        {
            if (uri == null)
            {
                VideoControl.Stop();
                return;
            }
			var tl = new MediaTimeline(uri);
			VideoControl.Clock = tl.CreateClock(true) as MediaClock;
		}
		
		void timer_tick(object sender, EventArgs e) {
			if (isDragging)
				return;
			mediaPlaying = true;
			if (!isDragging) {
				SeekBar.Value = VideoControl.Clock.CurrentTime.Value.TotalSeconds;
			}
			mediaPlaying = false;
		}

		private void Loaded_acitivity(object sender, RoutedEventArgs e) {
		}

		private void on_screen_play_button_click(object sender, RoutedEventArgs e) {
			if (VideoControl.Clock == null) return;
			if (VideoControl.Clock.IsPaused) {
				VideoControl.Clock.Controller.Resume();
				Screen_Icon.Kind = PackIconKind.Pause;
				control_icon.Kind = PackIconKind.Pause;
				DoubleAnimation myDoubleAnimation = new DoubleAnimation();
				myDoubleAnimation.From = 1.0;
				myDoubleAnimation.To = 0.0;
				myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
				Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
			}
			else {
				VideoControl.Clock.Controller.Pause();
				control_icon.Kind = PackIconKind.Play;
				Screen_Icon.Kind = PackIconKind.Play;
				Screen_Icon.Opacity = 1;
				DoubleAnimation myDoubleAnimation = new DoubleAnimation();
				myDoubleAnimation.From = 1.0;
				myDoubleAnimation.To = 0.0;
				myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
				Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
			}
		}

		private void Show_controller(object sender, MouseEventArgs e) {
			DoubleAnimation myDoubleAnimation = new DoubleAnimation();
			myDoubleAnimation.From = 0.0;
			myDoubleAnimation.To = 1.0;
			myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
			Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
		}

		private void Hide_controller(object sender, MouseEventArgs e) {
			DoubleAnimation myDoubleAnimation = new DoubleAnimation();
			myDoubleAnimation.From = 1.0;
			myDoubleAnimation.To = 0.0;
			myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
			Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
		}

		private void SeekBar_DragStarted(object sender, DragStartedEventArgs e) {
			isDragging = true;
		}

		private void SeekBar_DragCompleted(object sender, DragCompletedEventArgs e) {
			isDragging = false;
			VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(SeekBar.Value), TimeSeekOrigin.BeginTime);
		}

		private void VideoControll_opened(object sender, RoutedEventArgs e) {
			if (VideoControl.NaturalDuration.HasTimeSpan) {
				//VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
				Screen_Icon.Kind = PackIconKind.Play;
				Screen_Icon.Opacity = 1;
				TimeSpan ts = VideoControl.NaturalDuration.TimeSpan;
				SeekBar.Maximum = ts.TotalSeconds;
				SeekBar.SmallChange = 1;
				SeekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
				Volume_seeker.Value = 100;
				VideoControl.Volume = 1;
				VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(playinit), TimeSeekOrigin.BeginTime);
				VideoControl.Clock.Controller.Pause();
			}
			timer.Start();
		}

		private void Play_on_controlbar(object sender, RoutedEventArgs e) {
			if (VideoControl.Clock == null) return;
			if (VideoControl.Clock.IsPaused) {
				VideoControl.Clock.Controller.Resume();
				control_icon.Kind = PackIconKind.Pause;
				Screen_Icon.Kind = PackIconKind.Pause;
				DoubleAnimation myDoubleAnimation = new DoubleAnimation();
				myDoubleAnimation.From = 1;
				myDoubleAnimation.To = 0.0;
				myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
				Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
			}
			else {
				VideoControl.Clock.Controller.Pause();
				control_icon.Kind = PackIconKind.Play;
				Screen_Icon.Kind = PackIconKind.Play;
				Screen_Icon.Opacity = 1;
				DoubleAnimation myDoubleAnimation = new DoubleAnimation();
				myDoubleAnimation.From = 1.0;
				myDoubleAnimation.To = 0.0;
				myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
				Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
			}
		}

		private void On_Seek(object sender, RoutedPropertyChangedEventArgs<double> e) {
			if (VideoControl.Clock == null) return;
			if (isDragging || mediaPlaying)
				return;
			TimeSpan ts = TimeSpan.FromSeconds(SeekBar.Value);
			VideoControl.Clock.Controller.Seek(ts, TimeSeekOrigin.BeginTime);
			VideoControl.Clock.Completed += clock_completed;
		}

		private void Volume_change(object sender, RoutedPropertyChangedEventArgs<double> e) {
			VideoControl.Volume = Volume_seeker.Value / 100;
			if (Volume_seeker.Value >= 70)
				VolumeIcon.Kind = PackIconKind.VolumeHigh;
			if (Volume_seeker.Value < 70 && Volume_seeker.Value > 20)
				VolumeIcon.Kind = PackIconKind.VolumeMedium;
			if (Volume_seeker.Value <= 20 && Volume_seeker.Value != 0)
				VolumeIcon.Kind = PackIconKind.VolumeLow;
			if (Volume_seeker.Value == 0) {
				VolumeIcon.Kind = PackIconKind.VolumeMute;
			}
		}

		private void clock_completed(object sender, EventArgs e) {
			if (VideoControl.Clock == null) return;
			VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
			VideoControl.Clock.Controller.Pause();
			Screen_Icon.Kind = PackIconKind.Play;
			Screen_Icon.Opacity = 1;
			control_icon.Kind = PackIconKind.Play;
		}
	}
}