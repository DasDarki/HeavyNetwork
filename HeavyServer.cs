using LiteNetwork.Server;

namespace HeavyNetwork;

/// <summary>
/// Provides a more advanced implementation of <see cref="LiteServer{TUser}"/> that supports the HeavyNetwork protocol.
/// </summary>
/// <typeparam name="T"></typeparam>
public class HeavyServer<T> : LiteServer<T> where T : HeavyServerUser, new()
{
    public HeavyServer(HeavyServerOptions options, IServiceProvider? serviceProvider = null) : base(options, serviceProvider)
    {
    }
}