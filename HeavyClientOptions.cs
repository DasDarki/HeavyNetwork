using HeavyNetwork.Protocol;
using LiteNetwork.Client;

namespace HeavyNetwork;

/// <summary>
/// The client options for the HeavyNetwork client.
/// </summary>
public class HeavyClientOptions : LiteClientOptions, IHeavyNetworkOptions<HeavyClientOptions>
{
    /// <summary>
    /// Whether to use compression or not.
    /// </summary>
    public bool UseCompression { get; set; }
    
    public LinkedList<PacketPreprocessor> Preprocessors { get; } = new();
}