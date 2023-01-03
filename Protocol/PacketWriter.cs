using System.Drawing;
using System.Numerics;

namespace HeavyNetwork.Protocol;

/// <summary>
/// The packet writer takes care of writing data to the packets byte buffer.
/// </summary>
public class PacketWriter
{
    private readonly BinaryWriter _buffer;

    /// <summary>
    /// Creates a new instance of the <see cref="PacketWriter"/> class with an empty buffer and appends the packet id.
    /// </summary>
    public PacketWriter(ulong packetId)
    {
        _buffer = new BinaryWriter(new MemoryStream());

        WriteUInt64(packetId);
    }

    public byte[] ToArray()
    {
        return ((MemoryStream) _buffer.BaseStream).ToArray();
    }

    public void WriteInt16(short value)
    {
        _buffer.Write(value);
    }

    public void WriteUInt16(ushort value)
    {
        _buffer.Write(value);
    }

    public void WriteInt32(int value)
    {
        _buffer.Write(value);
    }

    public void WriteUInt32(uint value)
    {
        _buffer.Write(value);
    }

    public void WriteInt64(long value)
    {
        _buffer.Write(value);
    }

    public void WriteUInt64(ulong value)
    {
        _buffer.Write(value);
    }

    public void WriteFloat(float value)
    {
        _buffer.Write(value);
    }

    public void WriteDouble(double value)
    {
        _buffer.Write(value);
    }

    public void WriteBoolean(bool value)
    {
        _buffer.Write(value);
    }

    public void WriteString(string value)
    {
        _buffer.Write(value);
    }

    public void WriteBytes(byte[] value)
    {
        _buffer.Write(value);
    }

    public void WriteBytes(byte[] value, int offset, int count)
    {
        _buffer.Write(value, offset, count);
    }

    public void WriteBytes(byte[] value, int offset, int count, bool reverse)
    {
        if (reverse)
        {
            Array.Reverse(value, offset, count);
        }

        _buffer.Write(value, offset, count);
    }

    public void WriteBytes(byte[] value, bool reverse)
    {
        if (reverse)
        {
            Array.Reverse(value);
        }

        _buffer.Write(value);
    }

    public void WriteBytes(byte[] value, int count, bool reverse)
    {
        if (reverse)
        {
            Array.Reverse(value, 0, count);
        }

        _buffer.Write(value, 0, count);
    }

    public void WriteBytes(byte[] value, int count)
    {
        _buffer.Write(value, 0, count);
    }

    public void WriteDecimal(decimal value)
    {
        _buffer.Write(value);
    }

    public void WriteChar(char value)
    {
        _buffer.Write(value);
    }

    public void WriteDateTime(DateTime value)
    {
        _buffer.Write(value.ToBinary());
    }

    public void WriteGuid(Guid value)
    {
        _buffer.Write(value.ToByteArray());
    }

    public void WriteVector2(Vector2 value)
    {
        _buffer.Write(value.X);
        _buffer.Write(value.Y);
    }

    public void WriteVector3(Vector3 value)
    {
        _buffer.Write(value.X);
        _buffer.Write(value.Y);
        _buffer.Write(value.Z);
    }

    public void WriteVector4(Vector4 value)
    {
        _buffer.Write(value.X);
        _buffer.Write(value.Y);
        _buffer.Write(value.Z);
        _buffer.Write(value.W);
    }

    public void WriteQuaternion(Quaternion value)
    {
        _buffer.Write(value.X);
        _buffer.Write(value.Y);
        _buffer.Write(value.Z);
        _buffer.Write(value.W);
    }

    public void WriteMatrix4X4(Matrix4x4 value)
    {
        _buffer.Write(value.M11);
        _buffer.Write(value.M12);
        _buffer.Write(value.M13);
        _buffer.Write(value.M14);
        _buffer.Write(value.M21);
        _buffer.Write(value.M22);
        _buffer.Write(value.M23);
        _buffer.Write(value.M24);
        _buffer.Write(value.M31);
        _buffer.Write(value.M32);
        _buffer.Write(value.M33);
        _buffer.Write(value.M34);
        _buffer.Write(value.M41);
        _buffer.Write(value.M42);
        _buffer.Write(value.M43);
        _buffer.Write(value.M44);
    }

    public void WriteColor(Color value)
    {
        _buffer.Write(value.ToArgb());
    }

    public void WriteEnum<T>(T value)
    {
        _buffer.Write(Convert.ToInt32(value));
    }

    public void WriteList<T>(List<T> value, Action<T> writeAction)
    {
        _buffer.Write(value.Count);

        foreach (var item in value)
        {
            writeAction(item);
        }
    }

    public void WriteObject(object obj)
    {
        switch (obj)
        {
            case short o1:
                WriteInt16(o1);
                break;
            case ushort o2:
                WriteUInt16(o2);
                break;
            case int o3:
                WriteInt32(o3);
                break;
            case uint o4:
                WriteUInt32(o4);
                break;
            case long o5:
                WriteInt64(o5);
                break;
            case ulong o6:
                WriteUInt64(o6);
                break;
            case float o7:
                WriteFloat(o7);
                break;
            case double o8:
                WriteDouble(o8);
                break;
            case bool o9:
                WriteBoolean(o9);
                break;
            case string o10:
                WriteString(o10);
                break;
            case byte[] o11:
                WriteBytes(o11);
                break;
            case decimal o12:
                WriteDecimal(o12);
                break;
            case char o13:
                WriteChar(o13);
                break;
            case DateTime o14:
                WriteDateTime(o14);
                break;
            case Guid o15:
                WriteGuid(o15);
                break;
            case Vector2 o16:
                WriteVector2(o16);
                break;
            case Vector3 o17:
                WriteVector3(o17);
                break;
            case Vector4 o18:
                WriteVector4(o18);
                break;
            case Quaternion o19:
                WriteQuaternion(o19);
                break;
            case Matrix4x4 o20:
                WriteMatrix4X4(o20);
                break;
            case Color o21:
                WriteColor(o21);
                break;
            case Enum o22:
                WriteEnum(o22);
                break;
        }
    }
}