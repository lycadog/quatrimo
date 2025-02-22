using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace Quatrimo
{
    public class drawable
    {
        //add proper code for global objects with no parent
        //add elementPos conversion when setting localPos

        public bool stale;
        public bool hidden;

        protected drawable _parent = stateManager.baseParent;
        public drawable parent { get; }

        protected List<drawable> children;

        drawable staleParent;

        protected Vector2I _globalPos;
        public Vector2I globalPos { get => _globalPos; }

        protected Vector2 _localPos; //local position relative to parent
        public Vector2 localPos { get => _localPos; set { _localPos = value;
                //ADD _elementPos converson HERE
                syncWithParent(); updateChildren(); } }
        
        protected Vector2I _elementPos; //local element position
        public Vector2I elementPos { get => _elementPos; set { _elementPos = value; 
                localPos = new Vector2I(elementPos.x * 20 + 10, elementPos.y * 20 + 10); } }

        public Vector2I size = new Vector2I(20, 20);
        public Vector2I origin = new Vector2I(10, 10);
        public float rot = 0;


        /// <summary>
        /// Draw self and all children
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            drawChildren(spriteBatch, gameTime);
        }

        protected void drawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var child in children)
            {
                child.draw(spriteBatch, gameTime);
            }
        }

        /// <summary>
        /// Remove the drawable
        /// </summary>
        public void dispose()
        {
            stale = true;
            parent.children.Remove(this);
        }
        
        /// <summary>
        /// Makes the provided object the caller's parent
        /// </summary>
        /// <param name="parent"></param>
        public virtual void setParent(drawable parent)
        {
            _parent = parent;
            parent.children.Add(this);
            syncWithParent();
        }

        /// <summary>
        /// Remove the parent of the object, linking it to the worldRoot
        /// </summary>
        /// <param name="parent"></param>
        public virtual void removeParent() //rework
        {
            parent.children.Remove(this);
            setParent(stateManager.baseParent);
        }

        /// <summary>
        /// Syncs a child's global position with its parent
        /// </summary>
        public virtual void syncWithParent()
        {
            _globalPos = (Vector2I)(localPos + parent.globalPos);
        }
        
        /// <summary>
        /// Updates all the children of an object
        /// </summary>
        public virtual void updateChildren()
        {
            foreach(var child in children)
            {
                child.syncWithParent();
            }
        }

        public virtual void hide()
        {
            if(hidden == true)
            {
                Debug.WriteLine("Attempted to hide hidden sprite");
                return;
            }

            staleParent = parent;
            removeParent();
            _parent = stateManager.hiddenParent;
            hidden = true;
        }

        public virtual void unhide()
        {
            if(hidden == false)
            {
                Debug.WriteLine("Attempted to hide non-hidden sprite");
                return;
            }

            setParent(staleParent);
            hidden = false;
        }

    }
}