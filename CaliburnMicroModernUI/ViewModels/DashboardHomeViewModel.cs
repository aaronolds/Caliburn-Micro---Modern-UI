﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace CaliburnMicroModernUI.ViewModels
{
    [Export]
    public class DashboardHomeViewModel : Screen
    {
        public DashboardHomeViewModel()
        {

        }

        public string MessageTextBlock { get { return "Here I am - Dashboard"; } }
    }
}
