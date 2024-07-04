using SecondMauiAppMonkeyFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SecondMauiAppMonkeyFinder.Services
{
    public class MonkeyService
    {
        HttpClient httpClient;
        public MonkeyService()
        {
            httpClient = new HttpClient();
        }
        List<Monkey> monkeyList = new List<Monkey>();
        public async Task <List<Monkey>> GetMonkeys()
        {
            if (monkeyList?.Count > 0)
                return monkeyList;

            var url = "https://montemagno.com/monkeys.json";

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
            }


            #region if want to read from the app 

            //using var stream = FileSystem.OpenAppPackageFileAsync("monkeydata.json");
            //using var reader = new StreamReader(stream);
            //var contents = await reader.ReadToEndAsync();
            //monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
            #endregion

            return monkeyList;
        }
    }
}
