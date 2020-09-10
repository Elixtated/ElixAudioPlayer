using CommonModule.BaseViewModel;
using CommonModule.Services.NavigatorService;
using ElixAudioPlayer.LocalAudioList.View;
using ElixAudioPlayer.MusicControls.ViewModels;
using System;

namespace ElixAudioPlayer.MainWindow.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            NavigatorService = NavigatorService.Instance;
            NavigatorService.NavigatorViewModel.PageChanged += OnPageChanged;
            MusicControlViewModel = new MusicControlViewModel();
        }
        public NavigatorService NavigatorService { get; set; }
        public MusicControlViewModel MusicControlViewModel { get; }

        private void OnPageChanged(object sender, object viewModel)
        {
            if (viewModel is BasePlayListViewModel)
            {
                var castedViewModel = (BasePlayListViewModel)viewModel;
                castedViewModel.SetPlayByClickCommand(MusicControlViewModel.SelectAndPlayTrack);
                MusicControlViewModel.CurrentPlayListViewModel = castedViewModel;
            }
        }
    }
}
