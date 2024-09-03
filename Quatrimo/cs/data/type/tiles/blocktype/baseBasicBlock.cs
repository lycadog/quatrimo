
public class baseBasicBlock : baseBlockType
{
    public override blockType getType(board board)
    {
        return new basicBlock(board);
    }
}