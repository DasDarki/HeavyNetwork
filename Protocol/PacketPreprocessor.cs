namespace HeavyNetwork.Protocol;

/// <summary>
/// The packet preprocessor allows you to modify the packet before it is sent or received.
/// </summary>
public abstract class PacketPreprocessor
{
    /// <summary>
    /// Gets called when a packet is being sent.
    /// </summary>
    /// <param name="packet">The packet itself.</param>
    public virtual void PreprocessOutgoingPacket(Packet packet)
    {
    }
    
    /// <summary>
    /// Gets called when a packet was received and is about to be processed.
    /// </summary>
    /// <param name="packet">The packet itself.</param>
    public virtual void PreprocessIncomingPacket(Packet packet)
    {
    }

    /// <summary>
    /// Gets called when a packet was transformed to its byte data but before it is sent.
    /// </summary>
    /// <param name="packet">The original packet.</param>
    /// <param name="data">The transformed packet data.</param>
    public virtual void PreprocessOutgoingPacketData(Packet packet, ref byte[] data)
    {
    }
    
    /// <summary>
    /// Gets called when a packet was received and is about to be transformed to its object representation.
    /// </summary>
    /// <param name="data">The received packet data.</param>
    public virtual void PreprocessIncomingPacketData(ref byte[] data)
    {
    }
}