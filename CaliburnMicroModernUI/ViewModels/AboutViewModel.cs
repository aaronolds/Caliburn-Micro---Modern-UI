using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace CaliburnMicroModernUI.ViewModels
{
    [Export]
    public class AboutViewModel : Screen
    {
        public AboutViewModel()
        {
            int i = 1;
        }
        public List<string> Items { get { return new List<string> {"Modern UI", "Caliburn.Micro"}; } }
    }
}
