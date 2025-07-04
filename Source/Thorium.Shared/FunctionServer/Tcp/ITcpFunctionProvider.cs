namespace Thorium.Shared.FunctionServer.Tcp
{
    public interface ITcpFunctionProvider
    {
        string FunctionName {  get; }
        object Execute(FunctionServerTcpClient client, object[] args);
    }
}
