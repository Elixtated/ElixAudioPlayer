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
        private bool _isOpen;
        private bool _isClose = true;
        public MenuSlideViewModel()
        {
            LocalAudioListPage = new LocalAudioListPage();
            NavigatorService = NavigatorService.Instance;
            OpenMenuCommand = new RelayCommand(OpenMenu);
            CloseMenuCommand = new RelayCommand(CloseMenu);
            OpenLocalAudioListCommand = new RelayCommand(OpenLocalAudioList);
        }



        public ICommand OpenMenuCommand { get; }
        public ICommand CloseMenuCommand { get; }
        public ICommand OpenLocalAudioListCommand { get; }



        public NavigatorService NavigatorService { get; private set; }
        public LocalAudioListPage LocalAudioListPage { get; set; }


        public bool IsOpen
        {
            get => _isOpen;
            set => Set(ref _isOpen, value);
        }

        public bool IsClose
        {
            get => _isClose;
            set => Set(ref _isClose, value);

        }

        private void OpenMenu()
        {
            IsOpen = true;
            IsClose = false;
        }

        private void CloseMenu()
        {
            IsOpen = false;
            IsClose = true;
        }

        private void OpenLocalAudioList()
        {
            NavigatorService.SetCurrentPage(LocalAudioListPage);
        }
    }
}
