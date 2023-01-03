using System.Reflection;

namespace HeavyNetwork.Protocol;

/// <summary>
/// An extension to the normal <see cref="Packet"/> without the need to specify the write and read method.
/// Instead it uses the marked <see cref="Field"/> properties to write and read the data.
/// </summary>
public abstract class MappedPacket : Packet
{
    public override void Write(PacketWriter writer)
    {
        foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     .Where(f => f.GetCustomAttribute<Field>() != null)
                     .OrderBy(f => f.GetCustomAttribute<Field>()!.Index))
        {
            var value = field.GetValue(this);
            
            if (value is null)
                throw new NullReferenceException($"Field {field.Name} is null.");
            
            writer.WriteObject(value);
        }
    }
    
    public override void Read(PacketReader reader)
    {
        foreach (var field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                     .Where(f => f.GetCustomAttribute<Field>() != null)
                     .OrderBy(f => f.GetCustomAttribute<Field>()!.Index))
        {
            var value = reader.ReadObject(field.FieldType);
            
            if (value is null)
                throw new NullReferenceException($"Field {field.Name} is null.");
            
            field.SetValue(this, value);
        }
    }
}