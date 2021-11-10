namespace UI.Network.Protocol
{
    public class ProtocolProvider
    {
        public PreLoginProtocol Test { get; }
        public LoggedInProtocol MainProtocol { get; }
        public ProtocolProvider()
        {
            this.Test = new PreLoginProtocol();
            this.MainProtocol = new LoggedInProtocol();
        }
    }
}
