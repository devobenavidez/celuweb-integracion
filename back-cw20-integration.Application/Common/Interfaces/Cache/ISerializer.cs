namespace back_cw20_integration.Application.Common.Interfaces.Cache
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T value);
        T? Deserialize<T>(byte[] bytes);
    }
}
