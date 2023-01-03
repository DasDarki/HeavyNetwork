using System.Reflection;
using HeavyNetwork.Protocol;

namespace HeavyNetwork;

/// <summary>
/// Marks a method as packet handler which gets called when the packet of the given type is received.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PacketHandler : Attribute
{
    private static readonly Dictionary<Type, PacketHandlerExecutor> RegisteredHandlers = new();
    
    internal Type Type { get; }
    
    public PacketHandler(Type type)
    {
        Type = type;
    }
    
    /// <summary>
    /// Scans through the given type and registers all packet handlers which are static.<br/><br/>
    /// 
    /// <b>On the client side:</b><br/>
    /// The first argument and only argument must be the packet.<br/><br/>
    ///
    /// <b>On the server side:</b><br/>
    /// The first argument must be the client.
    /// The second and last argument must be the packet.
    ///
    /// <remarks>
    /// The method can be async and can return a Task.
    /// </remarks>
    /// </summary>
    public static void Scan<T>()
    {
        foreach (var method in typeof(T).GetRuntimeMethods())
        {
            if (!method.IsStatic) continue;
            var attribute = method.GetCustomAttribute<PacketHandler>();
            if (attribute == null) continue;
            if (RegisteredHandlers.TryGetValue(attribute.Type, out PacketHandlerExecutor? value))
                throw new Exception(
                    $"A packet handler for the type {attribute.Type.FullName} is already registered (existing: {value.Name})");
            RegisteredHandlers.Add(attribute.Type, new PacketHandlerExecutor(method));
        }
    }
    
    /// <summary>
    /// Scans through the given instances and registers all packet handlers which are not static.<br/><br/>
    ///
    /// <b>On the client side:</b><br/>
    /// The first argument and only argument must be the packet.<br/><br/>
    ///
    /// <b>On the server side:</b><br/>
    /// The first argument must be the client. If the handler is in the user class itself, it must be omitted.
    /// The second and last argument must be the packet.
    ///
    /// <remarks>
    /// The method can be async and can return a Task.
    /// </remarks>
    /// </summary>
    public static void Scan(object obj)
    {
        foreach (var method in obj.GetType().GetRuntimeMethods())
        {
            if (method.IsStatic) continue;
            var attribute = method.GetCustomAttribute<PacketHandler>();
            if (attribute == null) continue;
            if (RegisteredHandlers.TryGetValue(attribute.Type, out PacketHandlerExecutor? value))
                throw new Exception(
                    $"A packet handler for the type {attribute.Type.FullName} is already registered (existing: {value.Name})");
            RegisteredHandlers.Add(attribute.Type, new PacketHandlerExecutor(method, obj)
            {
                OmitClient = obj is HeavyServerUser
            });
        }
    }

    internal static Task ExecuteHandler(Packet packet, HeavyServerUser? user = null)
    {
        if (!RegisteredHandlers.TryGetValue(packet.GetType(), out PacketHandlerExecutor? executor))
            throw new Exception($"No packet handler for the type {packet.GetType().FullName} is registered");

        object[] args = user == null || executor.OmitClient ? new object[] {packet} : new object[] {user, packet};
        object? rval = executor.Execute(args);
        
        if (rval is Task task)
            return task;
        
        return Task.CompletedTask;
    }
    
    internal class PacketHandlerExecutor
    {
        internal string Name => _method.Name;
        
        internal bool OmitClient { get; set; }
    
        private readonly object? _owner;
        private readonly MethodInfo _method;

        internal PacketHandlerExecutor(MethodInfo method, object? owner = null)
        {
            _method = method;
            _owner = owner;
        }

        internal object? Execute(params object[] args)
        {
            return _method.Invoke(_owner, args);
        }
    }
}