

namespace Quatrimo
{
    public abstract class scoreOperation
    {

        public abstract void execute(encounter encounter);

        /// <summary>
        /// Interrupts the current board state, instead sets it to wait for animations to end - returns if the block tick loop should end or not
        /// </summary>
        /// <param name="encounter"></param>
        /// <returns></returns>
        public virtual bool interrupt(encounter encounter)
        {
            encounter.state = new animSuspendState(encounter, encounter.state);
            encounter.animHandler.animState = encounter.animHandler.waitForAnimations;
            return true;
        }
    }
}
