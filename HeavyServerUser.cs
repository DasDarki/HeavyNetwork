using HeavyNetwork.Encryption;
using HeavyNetwork.Protocol;
using K4os.Compression.LZ4;
using LiteNetwork.Server;

namespace HeavyNetwork;

/// <summary>
/// Provides a more advanced implementation of <see cref="LiteServerUser"/> that supports the HeavyNetwork protocol
/// layer.
/// </summary>
public class HeavyServerUser : LiteServerUser
{
    internal Encryptor Encryptor { get; } = new();
    
    /// <summary>
    /// Sends the given packet to the user.
    /// </summary>
    /// <param name="packet">The <see cref="Packet"/> to send.</param>
    public void SendPacket(Packet packet)
    {
        if (Context?.Options is not IHeavyNetworkOptions<HeavyServerOptions> options)
            throw new NotSupportedException("The server options are not of type HeavyServerOptions.");

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
        if (Context?.Options is not IHeavyNetworkOptions<HeavyServerOptions> options)
            throw new NotSupportedException("The server options are not of type HeavyServerOptions.");
        
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
            
            OnEncryptionHandshakeCompleted();
            return Task.CompletedTask;
        }

        return PacketHandler.ExecuteHandler(packet, this);
    }

    protected override void OnConnected()
    {
        if (Context?.Options is HeavyServerOptions {UseEncryption: true})
        {
            var clientPublicKey = Encryptor.GenerateKeyPair();
            
            SendPacket(new PacketEncryptionHandshake
            {
                Key = clientPublicKey
            });
        }
        
        base.OnConnected();
    }

    /// <summary>
    /// Gets called when the encryption key is received from the server.
    /// </summary>
    public virtual void OnEncryptionHandshakeCompleted()
    {
    }
}