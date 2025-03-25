namespace Quatrimo
{
    public class cursedBlock : block
    { 
        short state = 0; //0 = closed, 1 = open and score buff active, 2 = open with score buff inactive
        protected override void animateScoreF(bool forceAnim = false)
        {
            base.animateScore(forceAnim);
            state = 1;
            blockSprite.tex = content.cursedopen;
        }

        protected override sprite createGFXf(block block)
        {
            regSprite sprite = (regSprite)base.createGFXf(block);
            sprite.tex = content.cursedclosed;
            return sprite;
        }

        protected override void finalizeScoringF(block block)
        {
            score(this);
        }

        protected override void scoreF(block block)
        {
            scoredAnim = false;
            removed = false;
        }

        protected override void rotateGFXf(int direction, block block)
        {
        }

        protected override void scoreRemoveGFXf(block block)
        {
        }

        protected override void tickF(block block)
        {
            if(state > 0)
            {
                scoreValue = 4;
                state--;
                return;
            }
            scoreValue = 1;
            blockSprite.tex = content.cursedclosed;
        }

    }
}
