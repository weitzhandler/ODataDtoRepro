using ODataDtoRepro;
using ODataDtoRepro.Models;
using Simple.OData.Client;
using Simple.OData.Client.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Client
{
  class Program
  {
    static async Task Main(string[] args)
    {
      var settings = new ODataClientSettings(new Uri($"https://localhost:{Constants.Port}/{Constants.Api}"));
      settings.AfterResponse = (response) =>
      {
        if (response.RequestMessage.RequestUri.AbsolutePath.EndsWith("metadata"))
          return;

      };


      /*
            there should be a converter overload RegisterTypeConverter<TQuery, TResult, TConvert>() or RegisterTypeConverter<TQuery, TResult, TConvert>()
            So we can go 
            */

      settings.TypeCache.Converter.RegisterTypeConverter<Contact>(
        /*There is no information here that the result from the server was ContactDto*/ (IDictionary<string, object> d) =>
      {
        if (d.TryGetValue(nameof(ContactBase.ContactType), out var contactTypeValue) && contactTypeValue is string contactTypeStr && Enum.TryParse<ContactType>(contactTypeStr, out var contactType))
          return contactType == ContactType.Person
            ? (Contact)d.ToObject<Person>(settings.TypeCache)
            : d.ToObject<Company>(settings.TypeCache);

        throw new InvalidCastException($"Could not parse {nameof(Contact)} type from {d}.");
      });

      var client = new ODataClient(settings);

      var result = await client
        .For<Contact>()
        .FindEntriesAsync();

      /* the JSON result from server is: */
      /*
      { 
         "@odata.context":"https://localhost:54687/api/$metadata#Contacts/ODataDtoRepro.Models.ContactDto",
         "value":[ 
            { 
               "@odata.type":"#ODataDtoRepro.Models.ContactDto",
               "Id":1,
               "DisplayName":"John Doe"
            },
            { 
               "@odata.type":"#ODataDtoRepro.Models.ContactDto",
               "Id":2,
               "DisplayName":"Microsoft"
            }
         ]
      }
      */

      var materialized = result.ToList();
    }
  }
}


namespace Simple.OData.Client.Extensions
{
  static class DictionaryExtensions
  {
    static readonly MethodInfo _ToObjectGeneric;
    static readonly MethodInfo _ToObjectDynamic;
    static readonly ConcurrentDictionary<Type, MethodInfo> _Invocations = new ConcurrentDictionary<Type, MethodInfo>();
    static DictionaryExtensions()
    {
      var currentType = typeof(DictionaryExtensions);
      var type = Assembly.GetAssembly(typeof(ODataClient)).GetType(currentType.FullName);
      _ToObjectGeneric = type.GetMethod(nameof(ToObject), new[] { typeof(IDictionary<string, object>), typeof(ITypeCache), typeof(bool) });
      _ToObjectDynamic = type.GetMethod(nameof(ToObject), new[] { typeof(IDictionary<string, object>), typeof(ITypeCache), typeof(Type), typeof(bool) });
    }

    public static T ToObject<T>(this IDictionary<string, object> source, ITypeCache typeCache, bool dynamicObject = false)
        where T : class
    {
      var generic = _Invocations.GetOrAdd(typeof(T), t => _ToObjectGeneric.MakeGenericMethod(t));
      return (T)generic.Invoke(null, new object[] { source, typeCache, dynamicObject });
    }

    public static object ToObject(this IDictionary<string, object> source, ITypeCache typeCache, Type type, bool dynamicObject = false)
    {
      return _ToObjectDynamic.Invoke(null, new object[] { source, typeCache, type, dynamicObject });
    }
  }
}