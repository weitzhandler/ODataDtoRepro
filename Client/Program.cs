using ODataDtoRepro;
using ODataDtoRepro.Models;
using Simple.OData.Client;
using System;
using System.Threading.Tasks;

namespace Client
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var settings = new ODataClientSettings(new Uri($"https://localhost:{Constants.Port}/{Constants.Api}"));
      var client = new ODataClient(settings);

      var result = await client
        .For<Contact>()
        .FindEntriesAsync();


    }
  }
}
