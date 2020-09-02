using CommonModule.Navigator.ViewModels;
using System;
using System.Windows.Controls;

namespace CommonModule.Services.NavigatorService
{
    public class NavigatorService
    {
        private static readonly Lazy<NavigatorService> _instance = new Lazy<NavigatorService>(() => new NavigatorService());
        private NavigatorService()
        {
            NavigatorViewModel = new NavigatorViewModel();
        }

        public NavigatorViewModel NavigatorViewModel { get; }

        public static NavigatorService Instance { get { return _instance.Value; } }

        public void SetCurrentPage(Page currentPage)
        {
            NavigatorViewModel.CurrentContent = currentPage;
        }
    }
}
