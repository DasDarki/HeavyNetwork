namespace HeavyNetwork.Protocol;

/// <summary>
/// Marks a property as mapped packet field.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class Field : Attribute
{
    /// <summary>
    /// The index in the byte buffer of that field.
    /// </summary>
    public int Index { get; }

    public Field(int index)
    {
        Index = index;
    }
}