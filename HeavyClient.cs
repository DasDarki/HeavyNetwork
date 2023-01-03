using HeavyNetwork.Encryption;
using HeavyNetwork.Protocol;
using K4os.Compression.LZ4;
using LiteNetwork.Client;

namespace HeavyNetwork;

/// <summary>
/// Provides a more advanced implementation of <see cref="LiteClient"/> that supports the HeavyNetwork protocol
/// layer.
/// </summary>
public class HeavyClient : LiteClient
{
    internal Encryptor Encryptor { get; } = new();
    
    public HeavyClient(HeavyClientOptions options, IServiceProvider? serviceProvider = null) : base(options, serviceProvider)
    {
    }

    /// <summary>
    /// Sends the given packet to the server.
    /// </summary>
    /// <param name="packet">The <see cref="Packet"/> to send.</param>
    public void SendPacket(Packet packet)
    {
        if (Options is not IHeavyNetworkOptions<HeavyClientOptions> options)
            throw new NotSupportedException("The client options are not of type HeavyClientOptions.");

        var data = options.ConvertPacketToBytes(packet);

        if (Encryptor.CanEncrypt)
        {
            data = Encryptor.Encrypt(data);
        }

        if (options.UseCompression)
        {
            data = LZ4Pickler.Pickle(data);
        }
        
        Send(data);
    }

    /// <summary>
    /// Handles the HeavyNetwork protocol.
    /// </summary>
    public override Task HandleMessageAsync(byte[] packetBuffer)
    {
        if (Options is not IHeavyNetworkOptions<HeavyClientOptions> options)
            throw new NotSupportedException("The client options are not of type HeavyClientOptions.");
        
        if (options.UseCompression)
        {
            packetBuffer = LZ4Pickler.Unpickle(packetBuffer);
        }

        if (Encryptor.CanDecrypt)
        {
            packetBuffer = Encryptor.Decrypt(packetBuffer);
        }
        
        var packet = options.ConvertBytesToPacket(packetBuffer);

        if (packet is PacketEncryptionHandshake handshake)
        {
            Encryptor.PublicKey = handshake.Key;
            
            var serverPublicKey = Encryptor.GenerateKeyPair();
            
            SendPacket(new PacketEncryptionHandshake
            {
                Key = serverPublicKey
            });

            OnEncryptionHandshakeCompleted();
            return Task.CompletedTask;
        }
        
        return PacketHandler.ExecuteHandler(packet);
    }

    /// <summary>
    /// Gets called when the encryption key is received from the server.
    /// </summary>
    public virtual void OnEncryptionHandshakeCompleted()
    {
    }
}