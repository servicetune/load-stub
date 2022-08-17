using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Configuration
{
    public class AppStubOptions
    {
        public OperationScope? BackgroundLoop { get; set; }

        public Dictionary<string, OperationScope>? Endpoints { get; set; }

    }
}
