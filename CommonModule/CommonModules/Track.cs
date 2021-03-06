﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonModule.CommonModules
{
    public class Track : INotifyPropertyChanged
    {
        private string _fileSource;
        private string _performer;
        private string _title;
        private string _album;
        private TimeSpan _duration;
        private bool _isLocal;
        

        public Track()
        {
            Guid = Guid.NewGuid();
            PlaybackState = States.Stopped;
        }


        public Guid Guid { get; set; }

        public bool IsLocal
        {
            get => _isLocal;
            set
            {
                _isLocal = value;
                OnPropertyChanged();
            }
        }

        public string FileSource
        {
            get => _fileSource;
            set
            {
                _fileSource = value;
                OnPropertyChanged();
            }
        }

        public string Performer
        {
            get => _performer;
            set
            {
                _performer = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Album
        {
            get => _album;
            set
            {
                _album = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public enum States
        {
            Playing,
            Paused,
            Stopped
        }
        public States PlaybackState { get; set; }
        




        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
