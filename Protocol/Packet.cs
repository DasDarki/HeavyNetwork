namespace HeavyNetwork.Protocol;

/// <summary>
/// The packet describes a structure which contains data to send over the network. The packet itself describes
/// the structure of the packet, while an instance contains the actual data.
/// </summary>
public abstract class Packet
{
    /// <summary>
    /// Gets called when the packet should be written to the byte buffer before sending.
    /// </summary>
    /// <param name="writer">The <see cref="PacketWriter"/> for this packet.</param>
    public abstract void Write(PacketWriter writer);
    
    /// <summary>
    /// Gets called when the packet should be read from the byte buffer after receiving.
    /// </summary>
    /// <param name="reader">The <see cref="PacketReader"/> for this packet.</param>
    public abstract void Read(PacketReader reader);
}