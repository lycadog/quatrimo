

namespace Quatrimo
{
    public abstract class scoreOperation
    {

        public abstract void execute(encounter encounter);
        /// <summary>
        /// Returns whether or not the block should interrupt encounter state to wait for animations upon scoring
        /// </summary>
        /// <param name="encounter"></param>
        /// <returns></returns>
        public virtual bool interrupt(encounter encounter)
        {
            return true;
        }
    }
}
