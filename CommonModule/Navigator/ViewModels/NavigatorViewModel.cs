using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommonModule.BaseViewModel;

namespace CommonModule.Navigator.ViewModels
{
    public class NavigatorViewModel : ViewModel
    {
        private Page _currentContent;

        public Page CurrentContent
        {
            get => _currentContent;
            set
            {
                Set(ref _currentContent, value);
                PageChanged?.Invoke(this, value.DataContext);
            }
        }

        public event EventHandler<object> PageChanged;
    }
}
