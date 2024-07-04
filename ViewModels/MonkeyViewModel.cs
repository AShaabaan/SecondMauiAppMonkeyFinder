using CommunityToolkit.Mvvm.Input;
using SecondMauiAppMonkeyFinder.Models;
using SecondMauiAppMonkeyFinder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SecondMauiAppMonkeyFinder.View;

namespace SecondMauiAppMonkeyFinder.ViewModels
{
    public partial class MonkeyViewModel : BaseViewModel
    {
        MonkeyService monkeyService;

        public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();
        
        public MonkeyViewModel(MonkeyService service) 
        {
            this.monkeyService = service;
            Title = "Monkey Finder";
        }

        [RelayCommand]
        public async Task GoToDetailsAsync(Monkey monkey)
        {
            if (monkey == null) 
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}",true,
                new Dictionary<string, object>()
                {
                    { "Monkey", monkey }
                });
        }

        [RelayCommand]
        async Task GetMonkeyAsync()
        {
            if(IsBusy) return;

            try
            {
                IsBusy = true;
                var monkeys = await monkeyService.GetMonkeys();
                if (Monkeys.Count != 0)
                {
                    Monkeys.Clear();
                }
                foreach (var monkey in monkeys)
                {
                    Monkeys.Add(monkey);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error ", $"Can't get Monkeys {ex.Message}","ok");

            }

            finally
            {
                IsBusy = false;


            }
        }
    }
}
