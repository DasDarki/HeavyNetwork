using System.Security.Cryptography;
using System.Xml.Serialization;

namespace HeavyNetwork.Encryption;

/// <summary>
/// The encryptor takes care of encrypting and decrypting by the given RSA keys.
/// </summary>
internal class Encryptor
{
    internal string PublicKey { get; set; } = null!;

    internal string PrivateKey { get; private set; } = null!;
    
    internal bool CanEncrypt => !string.IsNullOrEmpty(PublicKey);
    
    internal bool CanDecrypt => !string.IsNullOrEmpty(PrivateKey);
    
    internal byte[] Encrypt(byte[] data)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        rsa.FromXmlString(PublicKey);
        
        return rsa.Encrypt(data, true);
    }
    
    internal byte[] Decrypt(byte[] data)
    {
        using var rsa = new RSACryptoServiceProvider(2048);
        rsa.FromXmlString(PrivateKey);
        
        return rsa.Decrypt(data, true);
    }

    internal string GenerateKeyPair()
    {
        using var generator = new RSACryptoServiceProvider(2048);
        PrivateKey = KeyToString(generator.ExportParameters(true));
        return KeyToString(generator.ExportParameters(false));
    }

    private string KeyToString(RSAParameters param)
    {
        using var stringWriter = new StringWriter();
        var xmlSerializer = new XmlSerializer(typeof(RSAParameters));
        xmlSerializer.Serialize(stringWriter, param);
        return stringWriter.ToString();
    }
}