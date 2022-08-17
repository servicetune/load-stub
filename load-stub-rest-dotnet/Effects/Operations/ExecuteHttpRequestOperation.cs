using Load.Stub.Rest.dotNet.Model;

namespace Load.Stub.Rest.dotNet.Effects.Operations
{
    public class ExecuteHttpRequestOperation : OperationRuntimeBase<object>
    {
        public IHttpClientFactory HttpClientFactory { get; }

        public ExecuteHttpRequestOperation(IHttpClientFactory httpClientFactory,Serilog.ILogger logger) :base(OperationType.ExecuteHttpRequest,logger.ForContext<ExecuteHttpRequestOperation>())
        {
            HttpClientFactory = httpClientFactory;
        }

        public override Task Executetion(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
