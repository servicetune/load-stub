using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Configuration
{
    public interface ICurrentConfig<T> where T : class
    {
        T? Value { get; }

        void SetConfig(T value);
    }

}