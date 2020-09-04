using CommonModule.CommonModules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModule.BaseViewModel
{
    public class BasePlayListViewModel : ViewModel
    {
        private Track _selectedTrack;
        public BasePlayListViewModel()
        {
            Tracks = new ObservableCollection<Track>();
            TracksOrder = new Dictionary<Guid, int>();
        }

        public ObservableCollection<Track> Tracks { get; set; }
        public Track SelectedTrack
        {
            get => _selectedTrack;
            set=> Set(ref _selectedTrack, value);
            
        }

        public Dictionary<Guid, int> TracksOrder { get; set; }
    }
}
