using System.Drawing;
using System.Numerics;

namespace HeavyNetwork.Protocol;

/// <summary>
/// The packet reader takes care of reading the data from its belonging byte buffer.
/// </summary>
public class PacketReader
{
    private readonly BinaryReader _buffer;

    /// <summary>
    /// Creates a new packet reader instance from the given byte data.
    /// </summary>
    /// <param name="data">The data which should be read.</param>
    public PacketReader(byte[] data)
    {
        var stream = new MemoryStream(data);
        stream.Seek(0, SeekOrigin.Begin);
        
        _buffer = new BinaryReader(stream);
    }
    
    /// <summary>
    /// Reads the packet ID from the buffer. If the stream is not at the beginning, an exception will be thrown.
    /// </summary>
    public ulong ReadPacketID()
    {
        if (_buffer.BaseStream.Position != 0)
            throw new Exception("The packet id can only be read at the beginning of the stream.");
        
        return _buffer.ReadUInt64();
    }
    
    public int ReadInt32()
    {
        return _buffer.ReadInt32();
    }
    
    public uint ReadUInt32()
    {
        return _buffer.ReadUInt32();
    }
    
    public short ReadInt16()
    {
        return _buffer.ReadInt16();
    }
    
    public ushort ReadUInt16()
    {
        return _buffer.ReadUInt16();
    }
    
    public byte ReadByte()
    {
        return _buffer.ReadByte();
    }
    
    public sbyte ReadSByte()
    {
        return _buffer.ReadSByte();
    }
    
    public bool ReadBoolean()
    {
        return _buffer.ReadBoolean();
    }
    
    public string ReadString()
    {
        return _buffer.ReadString();
    }
    
    public byte[] ReadBytes(int count)
    {
        return _buffer.ReadBytes(count);
    }
    
    public float ReadFloat()
    {
        return _buffer.ReadSingle();
    }
    
    public double ReadDouble()
    {
        return _buffer.ReadDouble();
    }
    
    public long ReadInt64()
    {
        return _buffer.ReadInt64();
    }
    
    public ulong ReadUInt64()
    {
        return _buffer.ReadUInt64();
    }
    
    public char ReadChar()
    {
        return _buffer.ReadChar();
    }
    
    public decimal ReadDecimal()
    {
        return _buffer.ReadDecimal();
    }
    
    public DateTime ReadDateTime()
    {
        return DateTime.FromBinary(_buffer.ReadInt64());
    }
    
    public Guid ReadGuid()
    {
        return new Guid(_buffer.ReadBytes(16));
    }
    
    public Vector2 ReadVector2()
    {
        return new Vector2(ReadFloat(), ReadFloat());
    }
    
    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }
    
    public Vector4 ReadVector4()
    {
        return new Vector4(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }
    
    public Quaternion ReadQuaternion()
    {
        return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
    }
    
    public Matrix4x4 ReadMatrix4X4()
    {
        return new Matrix4x4(
            ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat(),
            ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat(),
            ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat(),
            ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat()
        );
    }
    
    public Color ReadColor()
    {
        return Color.FromArgb(ReadInt32());
    }
    
    public T ReadEnum<T>() where T : Enum
    {
        return (T) Enum.ToObject(typeof(T), ReadInt32());
    }
    
    public object ReadEnum(Type type)
    {
        return Enum.ToObject(type, ReadInt32());
    }
    
    public IList<T> ReadList<T>(Func<T> readFunc)
    {
        var count = ReadInt32();
        var list = new List<T>(count);
        
        for (var i = 0; i < count; i++)
        {
            list.Add(readFunc());
        }
        
        return list;
    }

    public object ReadObject(Type type)
    {
        if (type == typeof(int)) return ReadInt32();
        if (type == typeof(uint)) return ReadUInt32();
        if (type == typeof(short)) return ReadInt16();
        if (type == typeof(ushort)) return ReadUInt16();
        if (type == typeof(byte)) return ReadByte();
        if (type == typeof(sbyte)) return ReadSByte();
        if (type == typeof(bool)) return ReadBoolean();
        if (type == typeof(string)) return ReadString();
        if (type == typeof(float)) return ReadFloat();
        if (type == typeof(double)) return ReadDouble();
        if (type == typeof(long)) return ReadInt64();
        if (type == typeof(ulong)) return ReadUInt64();
        if (type == typeof(char)) return ReadChar();
        if (type == typeof(decimal)) return ReadDecimal();
        if (type == typeof(DateTime)) return ReadDateTime();
        if (type == typeof(Guid)) return ReadGuid();
        if (type == typeof(Vector2)) return ReadVector2();
        if (type == typeof(Vector3)) return ReadVector3();
        if (type == typeof(Vector4)) return ReadVector4();
        if (type == typeof(Quaternion)) return ReadQuaternion();
        if (type == typeof(Matrix4x4)) return ReadMatrix4X4();
        if (type == typeof(Color)) return ReadColor();

        if (type.IsEnum)
        {
            return ReadEnum(type);
        }
        
        throw new Exception($"The type {type} is not supported.");
    }
}