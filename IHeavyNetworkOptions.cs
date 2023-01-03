using HeavyNetwork.Protocol;

namespace HeavyNetwork;

public interface IHeavyNetworkOptions<out T> where T : IHeavyNetworkOptions<T>
{
    /// <summary>
    /// Whether to use compression or not.
    /// </summary>
    public bool UseCompression { get; set; }
    
    protected LinkedList<PacketPreprocessor> Preprocessors { get; }
    
    /// <summary>
    /// Adds a packet preprocessor to the pipeline.
    /// </summary>
    public T AddPreprocessor(PacketPreprocessor preprocessor)
    {
        Preprocessors.AddLast(preprocessor);
        return (T) this;
    }

    internal byte[] ConvertPacketToBytes(Packet packet)
    {
        foreach (var preprocessor in Preprocessors)
        {
            preprocessor.PreprocessOutgoingPacket(packet);
        }

        var writer = new PacketWriter(PacketRegistry.GetPacketId(packet.GetType()));
        packet.Write(writer);

        var bytes = writer.ToArray();
        
        foreach (var preprocessor in Preprocessors)
        {
            preprocessor.PreprocessOutgoingPacketData(packet, ref bytes);
        }
        
        return bytes;
    }
    
    internal Packet ConvertBytesToPacket(byte[] bytes)
    {
        foreach (var preprocessor in Preprocessors)
        {
            preprocessor.PreprocessIncomingPacketData(ref bytes);
        }

        var reader = new PacketReader(bytes);
        var packet = PacketRegistry.GetPacket(reader.ReadPacketID());
        
        if (packet == null)
        {
            throw new Exception("Packet with ID " + reader.ReadPacketID() + " is not registered or an instance could not be created.");
        }
        
        packet.Read(reader);

        foreach (var preprocessor in Preprocessors)
        {
            preprocessor.PreprocessIncomingPacket(packet);
        }

        return packet;
    }
}