namespace Quatrimo
{
    public class scoreRow : scoreOperation
    {
        int y;
        Vector2I animBounds; //the left and right X positions, to start the decay animation at

        public scoreRow(int y, Vector2I animBounds)
        {
            this.y = y;
            this.animBounds = animBounds;
        }

        public void execute(encounter encounter)
        {
            throw new System.NotImplementedException();
        }

    }
}