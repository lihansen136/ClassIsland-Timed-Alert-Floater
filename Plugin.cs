using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; 
using System.Windows.Media.Imaging; 
using System.Windows.Threading; 
using System.Media;
using Microsoft.Extensions.Logging;

namespace ClassIsland.CustomCountdownPlugin
{
    public class Plugin
    {
        public Window floatingWindow;
        public TextBox timeInput;
        public Button startButton;
        public DispatcherTimer timer;
        private TimeSpan remainingTime;

        public Plugin()
        {
            InitializeFloatingWindow();
            InitializeTimer();
        }

        private void InitializeFloatingWindow()
        {
            floatingWindow = new Window
            {
                Title = "倒计时插件",
                Width = 250,
                Height = 150,
                WindowStyle = WindowStyle.None,
                AllowsTransparency = true,
                Background = Brushes.Transparent,
                Topmost = true,
                ShowInTaskbar = false
            };

            Grid grid = new Grid();
            grid.Background = new SolidColorBrush(Color.FromArgb(180, 30, 30, 30));


            timeInput = new TextBox
            {
                Margin = new Thickness(20, 30, 20, 0),

                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Top
            };

            startButton = new Button
            {
                Content = "开始",
                Margin = new Thickness(20, 80, 20, 20),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            startButton.Click += StartButton_Click;

            grid.Children.Add(timeInput);
            grid.Children.Add(startButton);
            floatingWindow.Content = grid;

            floatingWindow.Show();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(timeInput.Text, out int seconds))
            {
                remainingTime = TimeSpan.FromSeconds(seconds);
                timer.Start();
                startButton.IsEnabled = false;
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (remainingTime > TimeSpan.Zero)
            {
                remainingTime -= TimeSpan.FromSeconds(1);
            }
            else
            {
                timer.Stop();
                startButton.IsEnabled = true;
                PlayAlarmSound();
            }
        }

        private void PlayAlarmSound()
        {
            try
            {
                string audioPath = "./music/ding.mp3";
                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
            catch (Exception ex)
            {

                MessageBox.Show("播放音频时出错: " + ex.Message);
            }
        }
    }
}