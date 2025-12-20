using System.ComponentModel;
using System.Runtime.CompilerServices;
using CarListApp.Maui.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CarListApp.Maui.ViewModels
{

    
        public partial class BaseViewModel : ObservableObject
        {
            [ObservableProperty]
            string title;

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(IsNotLoading))]
            bool isLoading;

            public bool IsNotLoading => !isLoading;
        
        }
}
