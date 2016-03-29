using NMultiTool.Library.Module.Common.UI;
using NMultiTool.Library.Module.ViewModels;

namespace NMultiTool.Library.Module.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : ViewBase
    {
        public MainView(MainViewModel viewModel)
        {
            this.ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
