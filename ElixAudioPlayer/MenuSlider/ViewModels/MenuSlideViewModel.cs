using CommonModule.CommonTools;
using CommonModule.BaseViewModel;
using System;
using System.ComponentModel;
using System.Windows.Input;
using CommonModule.Services.NavigatorService;
using ElixAudioPlayer.LocalAudioList.View;
using ElixAudioPlayer.VkAudioList.View;

namespace ElixAudioPlayer.MenuSlider.ViewModels
{
    public class MenuSlideViewModel : ViewModel
    {
       
        public MenuSlideViewModel()
        {
            LocalAudioListPage = new LocalAudioListPage();
            VkAudioListPage = new VkAudioListPage();
            NavigatorService = NavigatorService.Instance;
            OpenLocalAudioListCommand = new RelayCommand(OpenLocalAudioList);
            OpenVkAudioListCommand = new RelayCommand(OpenVkAudioList);
        }

        public ICommand OpenLocalAudioListCommand { get; }
        public ICommand OpenVkAudioListCommand { get; }


        public NavigatorService NavigatorService { get; private set; }
        public LocalAudioListPage LocalAudioListPage { get; set; }
        public VkAudioListPage VkAudioListPage { get; set; }

        private void OpenLocalAudioList()
        {
            NavigatorService.SetCurrentPage(LocalAudioListPage);
        }

        private void OpenVkAudioList()
        {
            NavigatorService.SetCurrentPage(VkAudioListPage);
        }
    }
}
