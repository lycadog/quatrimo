
namespace Quatrimo
{
    public abstract class animation : drawable
    {
        //need code for removing animation from a list
        public bool completed = false;

        public abstract void terminate();
    }
}