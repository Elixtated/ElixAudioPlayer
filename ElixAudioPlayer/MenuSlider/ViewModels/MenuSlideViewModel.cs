using CommonModule.CommonTools;
using CommonModule.BaseViewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;
using CommonModule.Services.NavigatorService;
using ElixAudioPlayer.LocalAudioList.View;

namespace ElixAudioPlayer.MenuSlider.ViewModels
{
    public class MenuSlideViewModel : ViewModel
    {
       
        public MenuSlideViewModel()
        {
            LocalAudioListPage = new LocalAudioListPage();
            NavigatorService = NavigatorService.Instance;
            OpenLocalAudioListCommand = new RelayCommand(OpenLocalAudioList);
        }

        public ICommand OpenLocalAudioListCommand { get; }


        public NavigatorService NavigatorService { get; private set; }
        public LocalAudioListPage LocalAudioListPage { get; set; }

        private void OpenLocalAudioList()
        {
            NavigatorService.SetCurrentPage(LocalAudioListPage);
        }
    }
}
