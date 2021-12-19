using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI.Components
{
    /// <summary>
    /// Interaction logic for SoundMessage.xaml
    /// </summary>
    public partial class SoundMessage : UserControl
    {
        DispatcherTimer timer;
        bool isDragging = false, mediaPlaying = false;
        public double playinit { get; set; } = 0;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(Uri)
            , typeof(VideoPlayer), new FrameworkPropertyMetadata(null));
        public Uri Source
        {
            get => (Uri)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public SoundMessage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_tick);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == SourceProperty)
            {
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

        void timer_tick(object sender, EventArgs e)
        {
            if (isDragging)
                return;
            mediaPlaying = true;
            if (!isDragging)
            {
                SeekBar.Value = VideoControl.Clock.CurrentTime.Value.TotalSeconds;
            }
            mediaPlaying = false;
        }

		private void SeekBar_DragStarted(object sender, DragStartedEventArgs e)
		{
			isDragging = true;
		}

		private void SeekBar_DragCompleted(object sender, DragCompletedEventArgs e)
		{
			isDragging = false;
			VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(SeekBar.Value), TimeSeekOrigin.BeginTime);
		}

		private void VideoControll_opened(object sender, RoutedEventArgs e)
		{
			if (VideoControl.NaturalDuration.HasTimeSpan)
			{
				//VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
				TimeSpan ts = VideoControl.NaturalDuration.TimeSpan;
				SeekBar.Maximum = ts.TotalSeconds;
				SeekBar.SmallChange = 1;
				SeekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
				VideoControl.Volume = 1;
				VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(playinit), TimeSeekOrigin.BeginTime);
				VideoControl.Clock.Controller.Pause();
			}
			timer.Start();
		}

		private void Play_on_controlbar(object sender, RoutedEventArgs e)
		{
			if (VideoControl.Clock == null) return;
			if (VideoControl.Clock.IsPaused)
			{
				VideoControl.Clock.Controller.Resume();
				control_icon.Kind = PackIconKind.Pause;
			}
			else
			{
				VideoControl.Clock.Controller.Pause();
				control_icon.Kind = PackIconKind.Play;
			}
		}

		private void On_Seek(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (VideoControl.Clock == null) return;
			if (isDragging || mediaPlaying)
				return;
			TimeSpan ts = TimeSpan.FromSeconds(SeekBar.Value);
			VideoControl.Clock.Controller.Seek(ts, TimeSeekOrigin.BeginTime);
			VideoControl.Clock.Completed += clock_completed;
		}
		private void clock_completed(object sender, EventArgs e)
		{
			if (VideoControl.Clock == null) return;
			VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
			VideoControl.Clock.Controller.Pause();
			control_icon.Kind = PackIconKind.Play;
		}

	}
}
