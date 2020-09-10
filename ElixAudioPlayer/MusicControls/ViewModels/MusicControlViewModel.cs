using CommonModule.BaseViewModel;
using CommonModule.CommonTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ElixAudioPlayer.MusicControls.ViewModels
{
    public class MusicControlViewModel : ViewModel
    {
        private double _volumeValue = 1.0;
        private double _maxDurationValue;
        private bool _isPlaying;
        private double _positionValue;
        private TimeSpan _trackTimeNow;
        private string _timeNowText;
        private string _totalDurationText;


        public MusicControlViewModel()
        {
            MediaPlayer = new MediaPlayer();
            PlayCommand = new RelayCommand(Play);
            SelectAndPlayTrackCommand = new RelayCommand(SelectAndPlayTrack);
            SetVolumeCommand = new RelayCommand(SetVolume);
            SetPositionCommand = new RelayCommand(SetPosition);
            MediaPlayer.MediaEnded += PlayNextTrack;
        }

        public BasePlayListViewModel CurrentPlayListViewModel { get; set; }
        MediaPlayer MediaPlayer { get; set; }


        public ICommand PlayCommand { get; }
        public ICommand SetVolumeCommand { get; }
        public ICommand SetPositionCommand { get; }
        public ICommand SelectAndPlayTrackCommand { get; }



        #region Propertyes

        public double VolumeValue
        {
            get => _volumeValue;
            set => Set(ref _volumeValue, value);
        }
        public double MaxDurationValue
        {
            get => _maxDurationValue;
            set => Set(ref _maxDurationValue, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }

        public double PositionValue
        {
            get => _positionValue;
            set
            {
                Set(ref _positionValue, value);
            }
        }

        public TimeSpan TrackTimeNow
        {
            get => MediaPlayer.Position;
            set => Set(ref _trackTimeNow, value);

        }

        public string TimeNowText
        {
            get => _timeNowText;
            set => Set(ref _timeNowText, value);
        }

        public string TotalDurationText
        {
            get => _totalDurationText;
            set => Set(ref _totalDurationText, value);
        }


        #endregion

        private void PlayNextTrack(object sender, EventArgs e)
        {

            var currentTrack = CurrentPlayListViewModel.TracksOrder.FirstOrDefault(x => x.Key == CurrentPlayListViewModel.SelectedTrack.Guid);
            int nextTrackIndex;
            if (!currentTrack.Equals(default(KeyValuePair<Guid, int>)))
            {
                nextTrackIndex =/*isRepeat ? currentTrack.Value:*/currentTrack.Value + 1;
                

                if (CurrentPlayListViewModel.Tracks.Count > nextTrackIndex)
                {
                    CurrentPlayListViewModel.SelectedTrack = CurrentPlayListViewModel.Tracks[nextTrackIndex];
                    StartTrack(new Uri(CurrentPlayListViewModel.Tracks[nextTrackIndex].FileSource));
                }
            }
        }



        public void Play()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                if (CurrentPlayListViewModel.SelectedTrack != null && CurrentPlayListViewModel != null)
                {
                    if (!MediaPlayer.HasAudio)
                    {
                        StartTrack(new Uri(CurrentPlayListViewModel.SelectedTrack.FileSource));
                    }
                    else if (MediaPlayer.HasAudio)
                    {
                        MediaPlayer.Play();
                        Timer();
                    }
                }
            }
            else
            {
                MediaPlayer.Pause();
            }
            
        }

        public void SelectAndPlayTrack()
        {
            StartTrack(new Uri(CurrentPlayListViewModel.SelectedTrack.FileSource));
            IsPlaying = true;
        }


        private void StartTrack(Uri path)
        {
            MediaPlayer.Open(path);
            var trackTotalDuration = CurrentPlayListViewModel.SelectedTrack.Duration;
            TotalDurationText = String.Format("{0:00}:{1:00}:{2:00}", trackTotalDuration.Hours, trackTotalDuration.Minutes, trackTotalDuration.Seconds);

            MaxDurationValue = trackTotalDuration.TotalSeconds;

            PositionValue = 0;

            MediaPlayer.Play();
            Timer();
        }

        private void Timer()
        {
            var timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += new EventHandler(TimerTick);
            timerVideoTime.Start();
            
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                if (MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
                {
                    if (TrackTimeNow.TotalSeconds > 0)
                    {
                        TrackTimeNow = MediaPlayer.Position;
                        TimeNowText = String.Format("{0:00}:{1:00}:{2:00}", TrackTimeNow.Hours, TrackTimeNow.Minutes, TrackTimeNow.Seconds);
                        PositionValue = TrackTimeNow.TotalSeconds;
                    }
                }
            }
        }

        public void SetVolume()
        {
            MediaPlayer.Volume = VolumeValue;
        }

        public void SetPosition()
        {
            MediaPlayer.Position = TimeSpan.FromSeconds(PositionValue);
        }
    }
}
