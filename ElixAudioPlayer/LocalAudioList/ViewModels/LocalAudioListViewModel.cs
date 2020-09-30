using CommonModule.BaseViewModel;
using CommonModule.CommonTools;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using TagLib;
using System.Linq;
using System.Text;
using ElixAudioPlayer.MusicControls.ViewModels;
using CommonModule.CommonModules;
using System.Collections.Generic;

namespace ElixAudioPlayer.LocalAudioList.ViewModels
{
    public class LocalAudioListViewModel : BasePlayListViewModel
    {

        public LocalAudioListViewModel()
        {
            OpenBrowseCommand = new RelayCommand(OpenBrowse);  
        }

        public ICommand OpenBrowseCommand { get; }
        

        private void OpenBrowse()
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = true,
                DefaultExt = ".mp3",
                Filter = "Mp3 Files (.mp3|*.mp3"
            };
            bool? dialogResult = fileDialog.ShowDialog();

            if (dialogResult == true)
            {
                string[] TrackSourceArray = fileDialog.FileNames;
                string performerName;
                int TrackCount = 0;
                if(TracksOrder.Count != 0)
                {
                    TrackCount = TracksOrder.Count;
                }
                foreach (var trackSource in TrackSourceArray)
                {
                    if(Tracks.Any(x => x.FileSource == trackSource))
                    {
                        continue;
                    }
                    TagLib.File audioFile = TagLib.File.Create(trackSource);
                    if (audioFile.Tag.Performers.GetLength(0) > 0)
                    {
                        performerName = string.Join(", ", audioFile.Tag.Performers);
                    }
                    else
                    {
                        performerName = Path.GetFileName(trackSource);
                    }

                    var track = new Track()
                    {
                        FileSource = trackSource,
                        Title = toUtf8(audioFile.Tag.Title),
                        Performer = toUtf8(performerName),
                        Album = toUtf8(audioFile.Tag.Album),
                        Duration = audioFile.Properties.Duration,
                        IsLocal = true
                    };
                    Tracks.Add(track);
                    TracksOrder.Add(track.Guid, TrackCount);
                    TrackCount++;
                }
            }
        }

        public static string toUtf8(string convertedString)
        {
            if (convertedString != null)
            {
                return new string(convertedString.ToCharArray().
                Select(x => ((x + 848) >= 'А' && (x + 848) <= 'ё') ? (char)(x + 848) : x).
                ToArray());
            }
            else
            {
                return convertedString;
            }
        }
    }
}
