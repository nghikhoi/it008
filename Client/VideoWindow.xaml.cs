﻿using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace UI
{
    /// <summary>
    /// Interaction logic for VideoWindow.xaml
    /// </summary>
    public partial class VideoWindow : Window
    {
        DispatcherTimer timer;
        bool isDragging = false, mediaPlaying = false;

        public VideoWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timer_tick);
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

        private void Loaded_acitivity(object sender, RoutedEventArgs e)
        {
            VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
            VideoControl.Clock.Controller.Pause();
            Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            Screen_Icon.Opacity = 1;
        }




        private void on_screen_play_button_click(object sender, RoutedEventArgs e)
        {
            if (VideoControl.Clock.IsPaused)
            {
                VideoControl.Clock.Controller.Resume();
                Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                control_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;

                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 1.0;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));




                Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
            }
            else
            {
                VideoControl.Clock.Controller.Pause();
                control_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                Screen_Icon.Opacity = 1;
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 1.0;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);

            }


        }

        private void Show_controller(object sender, MouseEventArgs e)
        {


            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));




            Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
            Volume_seeker.BeginAnimation(OpacityProperty, myDoubleAnimation);


        }

        private void Hide_controller(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));




            Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
            Volume_seeker.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }



        private void SeekBar_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void SeekBar_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

            isDragging = false;
            VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(SeekBar.Value), TimeSeekOrigin.BeginTime);

        }

        private void VideoControll_opened(object sender, RoutedEventArgs e)
        {
            if (VideoControl.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = VideoControl.NaturalDuration.TimeSpan;
                SeekBar.Maximum = ts.TotalSeconds;
                SeekBar.SmallChange = 1;
                SeekBar.LargeChange = Math.Min(10, ts.Seconds / 10);
                SeekBar.MaxWidth = 750;
                Volume_seeker.Value = 100;
                VideoControl.Volume = 1;
            }
            timer.Start();
        }

        private void Play_on_controlbar(object sender, RoutedEventArgs e)
        {
            if (VideoControl.Clock.IsPaused)
            {
                VideoControl.Clock.Controller.Resume();
                control_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;


                Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Pause;
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 1;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);

            }
            else
            {
                VideoControl.Clock.Controller.Pause();
                control_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;

                Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
                Screen_Icon.Opacity = 1;
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 1.0;
                myDoubleAnimation.To = 0.0;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));

                Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);

            }
        }

        private void On_Seek(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDragging || mediaPlaying)
                return;
            TimeSpan ts = TimeSpan.FromSeconds(SeekBar.Value);
            VideoControl.Clock.Controller.Seek(ts, TimeSeekOrigin.BeginTime);
            VideoControl.Clock.Completed += clock_completed;
        }

        private void Expand_button_click(object sender, RoutedEventArgs e)
        {
            var videomsg = new VideoWindow();
            var tl = new MediaTimeline(VideoControl.Source);
            videomsg.VideoControl.Source = VideoControl.Source;
            videomsg.VideoControl.Clock = tl.CreateClock(true) as MediaClock;
            VideoControl.Clock.Controller.Pause();
            videomsg.Show();
        }

        private void Volume_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VideoControl.Volume = Volume_seeker.Value / 100;
        }

        private void show_volume_bar(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));




            Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
            Volume_seeker.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }

        private void hide_volume_bar(object sender, MouseEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 1.0;
            myDoubleAnimation.To = 0.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));




            Screen_Icon.BeginAnimation(OpacityProperty, myDoubleAnimation);
            Volume_seeker.BeginAnimation(OpacityProperty, myDoubleAnimation);
        }
        private void clock_completed(object sender, EventArgs e)
        {
            VideoControl.Clock.Controller.Seek(TimeSpan.FromSeconds(0), TimeSeekOrigin.BeginTime);
            VideoControl.Clock.Controller.Pause();
            Screen_Icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
            Screen_Icon.Opacity = 1;
            control_icon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Play;
        }
    }
}
