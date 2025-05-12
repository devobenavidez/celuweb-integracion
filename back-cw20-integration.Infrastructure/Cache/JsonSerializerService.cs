using back_cw20_integration.Application.Common.Interfaces.Cache;
using System.Text.Json;

namespace back_cw20_integration.Infrastructure.Cache
{
    public class JsonSerializerService : ISerializer
    {
        public T? Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }

        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value);
        }
    }
}
