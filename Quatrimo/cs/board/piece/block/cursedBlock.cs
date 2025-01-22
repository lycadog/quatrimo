namespace Quatrimo
{
    public class cursedBlock : block
    { 
        short state = 0; //0 = closed, 1 = open and score buff active, 2 = open with score buff inactive
        public override void animateScore(drawable anim, bool forceAnim = false)
        {
            base.animateScore(anim, forceAnim);
            state = 1;
            blockSprite.setRegTexture(content.cursedopen);
        }

        protected override void createGFXf(block block)
        {
            base.createGFXf(block);
            blockSprite.setRegTexture(content.cursedclosed);
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
            blockSprite.setRegTexture(content.cursedclosed);
        }

    }
}
