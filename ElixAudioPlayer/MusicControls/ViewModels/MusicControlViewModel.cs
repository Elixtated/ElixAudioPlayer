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
        private TimeSpan _trackTotalDuration;


        public MusicControlViewModel()
        {
            MediaPlayer = new MediaPlayer();
            PlayCommand = new RelayCommand(PlayPause);
            SetVolumeCommand = new RelayCommand(SetVolume);
            SetPositionCommand = new RelayCommand(SetPosition);
            MediaPlayer.MediaEnded += PlayNextTrack;
        }

        public BasePlayListViewModel CurrentPlayListViewModel { get; set; }
        MediaPlayer MediaPlayer { get; set; }


        public ICommand PlayCommand { get; }
        public ICommand SetVolumeCommand { get; }
        public ICommand SetPositionCommand { get; }

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

        public TimeSpan TrackTotalDuration
        {
            get => _trackTotalDuration;
            set => Set(ref _trackTotalDuration,value);
        }
        #endregion

        private void PlayNextTrack(object sender, EventArgs e)
        {

            var currentTrack = CurrentPlayListViewModel.TracksOrder.FirstOrDefault(x => x.Key == CurrentPlayListViewModel.SelectedTrack.Guid);
            int nextTrackIndex;
            if (!currentTrack.Equals(default(KeyValuePair<Guid, int>)))
            {
                nextTrackIndex = currentTrack.Value + 1;

                if (CurrentPlayListViewModel.Tracks.Count > nextTrackIndex)
                {
                    CurrentPlayListViewModel.SelectedTrack = CurrentPlayListViewModel.Tracks[nextTrackIndex];
                    MediaPlayer.Open(new Uri(CurrentPlayListViewModel.Tracks[nextTrackIndex].FileSource));
                    
                    TrackTotalDuration = CurrentPlayListViewModel.SelectedTrack.Duration;
                    MaxDurationValue = TrackTotalDuration.TotalSeconds;
                    PositionValue = 0;
                    MediaPlayer.Play();
                    Timer();
                }
            }
        }


        
        public void PlayPause()
        {
            if (!IsPlaying)
            {
                if (CurrentPlayListViewModel.SelectedTrack != null && CurrentPlayListViewModel != null)
                {
                    MediaPlayer.Open(new Uri(CurrentPlayListViewModel.SelectedTrack.FileSource));
                    TrackTotalDuration = CurrentPlayListViewModel.SelectedTrack.Duration;
                    MaxDurationValue = TrackTotalDuration.TotalSeconds;
                    PositionValue = 0;
                    MediaPlayer.Play();
                    IsPlaying = true;
                    Timer();
                }
            }
            else if (IsPlaying)
            {
                MediaPlayer.Pause();
                IsPlaying = false;
            }
        }


        private void Timer()
        {
            var timerVideoTime = new DispatcherTimer();
            timerVideoTime.Interval = TimeSpan.FromSeconds(1);
            timerVideoTime.Tick += new EventHandler(timerTick);
            timerVideoTime.Start();
        }

        void timerTick(object sender, EventArgs e)
        {
            if (MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds > 0)
            {
                if (TrackTimeNow.TotalSeconds > 0)
                {
                    TrackTimeNow = MediaPlayer.Position;
                    PositionValue = TrackTimeNow.TotalSeconds;
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
