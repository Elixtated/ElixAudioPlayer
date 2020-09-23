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
        private bool _isShuffleOn;
        private bool _isRepeatOn;
        private bool _isSelectionStarted;


        public MusicControlViewModel()
        {
            MediaPlayer = new MediaPlayer();
            PlayCommand = new RelayCommand(Play);
            SelectAndPlayTrackCommand = new RelayCommand(SelectAndPlayTrack);
            SetVolumeCommand = new RelayCommand(SetVolume);
            SetPositionCommand = new RelayCommand(SetPosition);
            SwitchNextTrackCommand = new RelayCommand(SwitchNextTrack);
            SwitchPreviousTrackCommand = new RelayCommand(SwitchPreviousTrack);
            ShuffleTracksCommand = new RelayCommand(ShuffleTracks);
            RepeatTrackCommand = new RelayCommand(RepeatTrack);
            DragCommand = new RelayCommand(Drag);
            MediaPlayer.MediaEnded += PlayNextTrack;
        }

        public BasePlayListViewModel CurrentPlayListViewModel { get; set; }
        MediaPlayer MediaPlayer { get; set; }


        public ICommand PlayCommand { get; }
        public ICommand DragCommand { get; }

        public ICommand SwitchPreviousTrackCommand { get; }

        public ICommand SwitchNextTrackCommand { get; }

        public ICommand ShuffleTracksCommand { get; }

        public ICommand RepeatTrackCommand { get; }

        public ICommand SetVolumeCommand { get; }

        public ICommand SetPositionCommand { get; }

        public ICommand SelectAndPlayTrackCommand { get; }



        #region Propertyes

        public bool IsSuffleOn
        {
            get => _isShuffleOn;
            set => Set(ref _isShuffleOn, value);
        }

        public bool SelectionStarted
        {
            get => _isSelectionStarted;
            set => Set(ref _isSelectionStarted, value);
        }

        public bool IsRepeatOn
        {
            get => _isRepeatOn;
            set => Set(ref _isRepeatOn, value);
        }

        public KeyValuePair<Guid, int> CurrentTrack
        {
            get => CurrentPlayListViewModel.TracksOrder.FirstOrDefault(x => x.Key == CurrentPlayListViewModel.SelectedTrack.Guid);
        }

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
            SwitchNextTrack();
        }



        private void Play()
        {
            IsPlaying = !IsPlaying;
            if (IsPlaying)
            {
                if (CurrentPlayListViewModel.SelectedTrack != null && CurrentPlayListViewModel != null)
                {
                    if (!MediaPlayer.HasAudio)
                    {
                        StartTrack(null);
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
            StartTrack(null);
            IsPlaying = true;
        }
        /*isRepeat ? currentTrack.Value:*/
        private void SwitchNextTrack()
        {

            int nextTrackIndex;
            if (!CurrentTrack.Equals(default(KeyValuePair<Guid, int>)))
            {

                if (IsRepeatOn)
                {
                    nextTrackIndex = CurrentTrack.Value;
                }
                else if (IsSuffleOn)
                {
                    Random random = new Random();
                    nextTrackIndex = random.Next(0, CurrentPlayListViewModel.Tracks.Count);
                    while (CurrentPlayListViewModel.Tracks[nextTrackIndex] == CurrentPlayListViewModel.SelectedTrack)
                    {
                        nextTrackIndex = random.Next(0, CurrentPlayListViewModel.Tracks.Count);
                    }
                }
                else
                {
                    nextTrackIndex = CurrentTrack.Value + 1;
                }
                if (CurrentPlayListViewModel.Tracks.Count > nextTrackIndex)
                {

                    StartTrack(nextTrackIndex);
                }
            }
        }

        private void SwitchPreviousTrack()
        {

            int nextTrackIndex;
            if (!CurrentTrack.Equals(default(KeyValuePair<Guid, int>)))
            {
                nextTrackIndex = CurrentTrack.Value - 1;

                if (CurrentPlayListViewModel.Tracks.Count > nextTrackIndex & nextTrackIndex >= 0)
                {

                    StartTrack(nextTrackIndex);
                }
            }
        }


        private void StartTrack(int? index = null)
        {

            if (index != null)
            {
                CurrentPlayListViewModel.SelectedTrack = CurrentPlayListViewModel.Tracks[index.Value];
            }
            var path = CurrentPlayListViewModel.SelectedTrack.FileSource;

            MediaPlayer.Open(new Uri(path));
            var trackTotalDuration = CurrentPlayListViewModel.SelectedTrack.Duration;
            TotalDurationText = String.Format("{0:00}:{1:00}:{2:00}", trackTotalDuration.Hours, trackTotalDuration.Minutes, trackTotalDuration.Seconds);

            MaxDurationValue = trackTotalDuration.TotalSeconds;

            PositionValue = 0;

            MediaPlayer.Play();
            Timer();
        }

        private void Timer()
        {
            var timerTime = new DispatcherTimer();
            timerTime.Interval = TimeSpan.FromSeconds(1);
            timerTime.Tick += new EventHandler(TimerTick);
            timerTime.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (SelectionStarted == false)
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

        }
        // проверка таймера на флаг перетаскивания туду епта selectionstarted

        private void Drag()
        {
            SelectionStarted = !SelectionStarted;
        }
        private void SetVolume()
        {
            MediaPlayer.Volume = VolumeValue;
        }

        private void SetPosition()
        {
            MediaPlayer.Position = TimeSpan.FromSeconds(PositionValue);
            SelectionStarted = !SelectionStarted;
        }

        private void ShuffleTracks()
        {
            IsSuffleOn = !IsSuffleOn;
        }
        private void RepeatTrack()
        {
            IsRepeatOn = !IsRepeatOn;
        }
    }
}
