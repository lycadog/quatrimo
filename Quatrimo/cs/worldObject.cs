using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Quatrimo
{
    public class worldObject
    {
        //NEW SYSTEM FOR SPRITEMANAGERS: a sprite draws all of their children when drawn. this way sprite groups can easily be
        //added and removed from the drawlist simply. this will rework spriteManagers to being a drawable worldobject that simply draw
        //their children only

        //EDIT LATER so children is private, ALSO sync _elementPos directly on localPos update
        protected worldObject _parent = stateManager.worldRoot;
        public worldObject parent { get => _parent; set => _parent = value; } //DO NOT EVER SET except for GLOBAL objects

        protected List<worldObject> children;

        worldObject staleParent;

        protected Vector2I _globalPos;
        public Vector2I globalPos { get => _globalPos; }

        protected Vector2 _localPos; //local screen position
        public Vector2 localPos { get => _localPos; set { _localPos = value; syncWithParent(); updateChildren(); } }
        //add conversion to elementPos during localPos set later

        protected Vector2I _elementPos; //local element position
        public Vector2I elementPos { get => _elementPos; set { _elementPos = value;
                localPos = new Vector2I(elementPos.x * 20 + 10, elementPos.y * 20 + 10); } }

        public Vector2I size = new Vector2I(20, 20);
        public Vector2I origin = new Vector2I(10, 10);
        public float rot = 0;
        
        /// <summary>
        /// Makes the provided object the caller's parent
        /// </summary>
        /// <param name="parent"></param>
        public virtual void setParent(worldObject parent)
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
            _parent = stateManager.worldRoot;
            syncWithParent();
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
            staleParent = parent;
            removeParent();
            _parent = 
        }

        public virtual void unhide()
        {

        }

    }
}