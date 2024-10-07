
namespace Quatrimo
{
    public abstract class gameState
    {
        protected stateManager manager;

        protected gameState(stateManager manager)
        {
            this.manager = manager;
        }

        public abstract void setState();
        public abstract void removeState();
        public abstract void addState();

        public void setTemporary()
        {
            foreach(var state in manager.state)
            {
                state.removeState();
            }
            manager.stateStack.Push(manager.state);
            manager.state.Clear();

            setState();
        }
        public void endTemporaryState()
        {
            removeState();
           
            foreach(var state in manager.stateStack.Peek())
            {
                state.addState();
            }
            manager.state = manager.stateStack.Pop();

        }
    }
}
