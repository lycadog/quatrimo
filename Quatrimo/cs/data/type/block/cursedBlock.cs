namespace Quatrimo
{
    public class cursedBlock : block
    { 
        short state = 0; //0 = closed, 1 = open and score buff active, 2 = open with score buff inactive

        public override void animateScore(encounter encounter, animation anim, int index = -1, bool forceAnim = false)
        {
            base.animateScore(encounter, anim, index, forceAnim);
            state = 1;
            element.tex = Game1.cursedopen;
        }

        protected override void graphicsInit(block block)
        {
            base.graphicsInit(block);
            element.tex = Game1.cursedclosed;
        }

        protected override void scoreF(block block)
        {
            base.scoreF(block);
            scoredAnim = false;
            markedForRemoval = false;
        }

        protected override void rotateGFXf(int direction, block block)
        {
        }

        protected override void scoreRemoveGFXf(block block)
        {
        }

        protected override void tickF(block block)
        {
            scoredAnim = false;
            scored = false;

            if(state > 0)
            {
                scoreValue = 4;
                state--;
                return;
            }
            scoreValue = 1;
            element.tex = Game1.cursedclosed;
        }
    }
}
