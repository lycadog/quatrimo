
/// <summary>
/// block type class for bagPieces, used to create a new blockType when a bagpiece is converted to a boardpiece
/// </summary>
public abstract class baseBlockType
{
    /// <summary>
    /// returns associated type to use for boardPieces
    /// </summary>
    /// <returns></returns>
    public abstract blockType getType();
}