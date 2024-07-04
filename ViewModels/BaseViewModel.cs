using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SecondMauiAppMonkeyFinder.ViewModels
{
    public partial class BaseViewModel :  ObservableObject
    {
        public BaseViewModel() 
        {
           
        }


        [ObservableProperty]
        //[AlsoNotifyChangeFor(nameof(IsNotBusy))]
        //[AlsoNotifychangeFor(nameof(IsNotBusy))]
        bool isBusy;
        [ObservableProperty]
        string title;

        public bool IsNotBusy => !IsBusy;

    }
}
