using CommonModule.BaseViewModel;
using CommonModule.CommonModules;
using CommonModule.CommonTools;
using CommonModule.Services.NavigatorService;
using ElixAudioPlayer.VkAudioList.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using VkNet;
using VkNet.Abstractions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace ElixAudioPlayer.VkAudioList.ViewModels
{
    public class VkAudioListViewModel : BasePlayListViewModel
    {
        private static IVkApi _api;
        private string _login;
        private string _password;
        private string _twoFactorAuthorization;
        private bool _isLoading;
        public VkAudioListViewModel()
        {
            AutorisationCommand = new RelayCommand(Autorisation);
            GetAudioCommand = new RelayCommand(ShowAudio);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddAudioBypass();
            _api = new VkApi(serviceCollection);

        }
        public ICommand AutorisationCommand { get; }
        public ICommand GetAudioCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public string Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string TwoFactorAuthorization
        {
            get => _twoFactorAuthorization;
            set => Set(ref _twoFactorAuthorization, value);
        }

        private void Autorisation()
        {

            while (_api.IsAuthorized == false)
            {
                _api.Authorize(new ApiAuthParams
                {
                    Login = "",
                    Password = ""

                });
            }
        }

        private async void ShowAudio()
        {

            var audios = Enumerable.Empty<VkNet.Model.Attachments.Audio>();


            var loadedAudios = await Task.Factory.StartNew(async () =>
            {
                try
                {
                    IsLoading = true;
                    return await _api.Audio.GetAsync(new AudioGetParams { Count = 2 });
                }
                finally
                {
                    IsLoading = false;
                }
            });
            audios = await loadedAudios;

            //кружок зарузки с флагами видимости
            int trackNumber = 0;
            foreach (var audio in audios)
            {
                if (audio.Url != null & audio.Title != null & audio.Artist != null & audio.Album != null)
                {
                    var track = new Track()
                    {
                        FileSource = M3u8ToMp3(audio.Url.ToString()),
                        Title = audio.Title,
                        Performer = audio.Artist,
                        Album = audio.Album.Title,
                        Duration = TimeSpan.FromSeconds(audio.Duration),
                        IsLocal = false
                    };
                    Tracks.Add(track);
                    TracksOrder.Add(track.Guid, trackNumber);
                    trackNumber++;
                }
            }
        }
        private string M3u8ToMp3(string url)
        {
            var replaced = Regex.Replace(url, @"(.*vkuseraudio\.net\/.*?\/).*?\/(.*?)\/index\.m3u8(.*)", "$1$2.mp3$3");
            return replaced;
        }

    }
}
