using LIT.Smabu.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LIT.Smabu.Infrastructure.Persistence
{
    public static class AggregateJsonConverter
    {
        static readonly JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new StringEnumConverter() }
        };

        static AggregateJsonConverter()
        {

        }

        public static string ConvertToJson(IAggregateRoot aggregate)
        {
            var result = JsonConvert.SerializeObject(aggregate, settings);
            return result;
        }

        public static TAggregate ConvertFromJson<TAggregate>(string json) where TAggregate : class, IAggregateRoot
        {
            var result = ConvertFromJson(typeof(TAggregate), json) as TAggregate;
            return result ?? throw new InvalidCastException($"Aggregate has to be type of {typeof(TAggregate)}");
        }

        public static IAggregateRoot ConvertFromJson(Type aggregateType, string json)
        {
            var result = JsonConvert.DeserializeObject(json, aggregateType, settings) as IAggregateRoot;
            return result ?? throw new InvalidCastException($"Aggregate has to be type of {aggregateType}");
        }
    }
}
