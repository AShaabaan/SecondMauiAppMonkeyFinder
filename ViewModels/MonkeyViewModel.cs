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
        //it will give us a specifif api that is calling underlying api
        IConnectivity  connectivity;
        private readonly IGeolocation geolocation;

        public ObservableCollection<Monkey> Monkeys { get; } = new ObservableCollection<Monkey>();
        
        
        public MonkeyViewModel(MonkeyService service ,IConnectivity connectivity , IGeolocation  geolocation) 
        {
            this.monkeyService = service;
            Title = "Monkey Finder";
            this.connectivity = connectivity;
            this.geolocation = geolocation;
        }

        [RelayCommand]
        public async Task GetCloestMonkeyAsync()
        {
            if (IsBusy || Monkeys.Count == 0)
                return;
            Monkeys.Clear();
            try
            {
                var location = await geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await geolocation.GetLocationAsync(
                        new GeolocationRequest
                        {
                            DesiredAccuracy = GeolocationAccuracy.Medium,
                            Timeout = TimeSpan.FromSeconds(30),

                        });
                }
                if (location == null)
                {
                    return;
                }
                var first = Monkeys.OrderBy(x => location.CalculateDistance(x.Latitude, x.Longitude, DistanceUnits.Miles)).FirstOrDefault();
                if (first == null)
                    return;

                await Shell.Current.DisplayAlert("Cloesest Monkey", $"{first.Name} in {first.Location}","ok");
            }
            catch (Exception ex)
            {
                {
                    Debug.WriteLine(ex);
                    await Shell.Current.DisplayAlert("Error ", $"Can't get Cloest Monkey {ex.Message}", "ok");
                }
            }
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
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                { 
                    await Shell.Current.DisplayAlert("Error", "Check Internet Connection", "Ok");
                    return;
                }

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
