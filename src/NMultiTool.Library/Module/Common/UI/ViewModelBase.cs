using System.Windows;
using NMultiTool.Library.Module.Views;

namespace NMultiTool.Library.Module.Common.UI
{
    public abstract class ViewModelBase : DependencyObject
    {
        public MainWindow MainWindow { get; set; }
    }
}
