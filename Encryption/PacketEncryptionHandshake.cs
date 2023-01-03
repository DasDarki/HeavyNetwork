using HeavyNetwork.Protocol;

namespace HeavyNetwork.Encryption;

internal class PacketEncryptionHandshake : Packet
{
    /// <summary>
    /// The public key for the RSA encryption.
    /// </summary>
    public string Key { get; set; }
    
    public override void Write(PacketWriter writer)
    {
        writer.WriteString(Key);
    }

    public override void Read(PacketReader reader)
    {
        Key = reader.ReadString();
    }
}