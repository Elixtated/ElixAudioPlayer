using CommonModule;
using CommonModule.BaseViewModel;
using CommonModule.CommonTools;
using ElixAudioPlayer.LocalAudioList.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ElixAudioPlayer.MusicControls.ViewModels
{
    public class MusicControlViewModel : ViewModel
    {
        private double _volumeValue = 1.0;
        private double _maxDurationValue;
        private bool _isPlaying;
        private int _positionValue;


        public MusicControlViewModel()
        {
            MediaPlayer = new MediaPlayer();
            PlayCommand = new RelayCommand(PlayPause);
            SetVolumeCommand = new RelayCommand(SetVolume);
            SetPositionCommand = new RelayCommand(SetPosition);

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
        {   get => _maxDurationValue;
            set => Set(ref _maxDurationValue, value);
        }

        public bool IsPlaying
        {
            get => _isPlaying;
            set => Set(ref _isPlaying, value);
        }

        public int PositionValue
        {
            get => _positionValue;
            set => Set(ref _positionValue, value);
        }
        #endregion



        public void PlayPause()
        {
            if (!IsPlaying)
            {
                if (CurrentPlayListViewModel.SelectedTrack != null && CurrentPlayListViewModel != null)
                {
                    MediaPlayer.Open(new Uri(CurrentPlayListViewModel.SelectedTrack.FileSourse));
                    MaxDurationValue = CurrentPlayListViewModel.SelectedTrack.Duration.TotalSeconds;
                    MediaPlayer.Play();
                    IsPlaying = true;
 
                }
            }
            else if(IsPlaying)
            {
                MediaPlayer.Pause();
                IsPlaying = false;
            }
            
        }

        public void SetVolume()
        {
            MediaPlayer.Volume = VolumeValue; 
        }  

        public void SetPosition()
        {
                MediaPlayer.Pause();
                TimeSpan time = new TimeSpan(0, 0, Convert.ToInt32(Math.Round((double)PositionValue)));
                MediaPlayer.Position = time;
                MediaPlayer.Play();  
        }
    }
}
