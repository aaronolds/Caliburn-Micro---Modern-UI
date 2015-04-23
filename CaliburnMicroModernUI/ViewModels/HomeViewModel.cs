using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace CaliburnMicroModernUI.ViewModels
{
    [Export]
    public sealed class HomeViewModel : Screen
    {
        public string MessageTextBlock { get { return "Here I am."; } }
    }
}
