using Articles.Models.Feature.Login;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Articles.Client;
public static class ObjectExtension
{
    private readonly static JsonSerializerSettings _deserializeSettings = new() { ObjectCreationHandling = ObjectCreationHandling.Replace };

    public static T CloneJson<T>(this T source) => 
        source is null
            ? default!
            : JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), _deserializeSettings)!;
}
