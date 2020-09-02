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
        public BasePlayListViewModel()
        {
            Tracks = new Dictionary<int, Track>();
        }

        public Dictionary<int,Track> Tracks { get; set; }
        public Track SelectedTrack { get; set; }
    }
}
