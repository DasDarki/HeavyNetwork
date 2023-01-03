using System.Collections.Concurrent;

namespace HeavyNetwork.Protocol;

/// <summary>
/// The packet registry manages all <see cref="Packet"/>s in the HeavyNetwork protocol.
/// </summary>
public static class PacketRegistry
{
    private static readonly ConcurrentDictionary<ulong, Type> IdToPacket = new();
    private static readonly ConcurrentDictionary<Type, ulong> PacketToId = new();
    private static ulong _nextId;

    /// <summary>
    /// Registers the given type as a packet type and assigns it a packet id.
    /// </summary>
    /// <typeparam name="T">The packet type. Must extend <see cref="Packet"/>.</typeparam>
    public static void Register<T>() where T : Packet
    {
        var id = _nextId++;
        
        if (!IdToPacket.TryAdd(id, typeof(T)))
            throw new InvalidOperationException("Packet id already registered.");
        if (!PacketToId.TryAdd(typeof(T), id))
            throw new InvalidOperationException($"Packet type {typeof(T).FullName} already registered.");
    }
    
    /// <summary>
    /// Unregisters the given type as a packet type and removes its packet id.
    /// </summary>
    /// <typeparam name="T">The packet type.</typeparam>
    public static void Unregister<T>() where T : Packet
    {
        if (!PacketToId.TryRemove(typeof(T), out var id))
            throw new InvalidOperationException($"Packet type {typeof(T).FullName} not registered.");
        if (!IdToPacket.TryRemove(id, out _))
            throw new InvalidOperationException("Packet id not registered.");
    }
    
    /// <summary>
    /// Returns the packet id for the given packet type.
    /// </summary>
    internal static ulong GetPacketId(Type packetType)
    {
        if (!PacketToId.TryGetValue(packetType, out var id))
            throw new InvalidOperationException($"Packet type {packetType.FullName} not registered.");
        return id;
    }
    
    /// <summary>
    /// Returns the packet type for the given packet id.
    /// </summary>
    internal static Type GetPacketType(ulong id)
    {
        if (!IdToPacket.TryGetValue(id, out var packetType))
            throw new InvalidOperationException($"Packet id {id} not registered.");
        return packetType;
    }
    
    /// <summary>
    /// Creates a new packet instance for the given packet id.
    /// </summary>
    internal static Packet? GetPacket(ulong id)
    {
        var packetType = GetPacketType(id);
        return Activator.CreateInstance(packetType) as Packet;
    }
}