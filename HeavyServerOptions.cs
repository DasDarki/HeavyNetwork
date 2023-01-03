using HeavyNetwork.Protocol;
using LiteNetwork.Server;

namespace HeavyNetwork;

/// <summary>
/// The server options for the HeavyNetwork server.
/// </summary>
public class HeavyServerOptions : LiteServerOptions, IHeavyNetworkOptions<HeavyServerOptions>
{
    /// <summary>
    /// Whether to use RSA encryption or not.
    /// </summary>
    public bool UseEncryption { get; set; } = false;

    /// <summary>
    /// Whether to use compression or not.
    /// </summary>
    public bool UseCompression { get; set; }
    
    public LinkedList<PacketPreprocessor> Preprocessors { get; } = new();
}