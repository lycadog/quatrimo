namespace Quatrimo
{
    public class emptyScoreOperation : scoreOperation
    {
        public override void execute(encounter encounter)
        {
        }

        public override bool interrupt(encounter encounter)
        {
            return false;
        }
    }
}