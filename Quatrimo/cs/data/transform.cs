using Microsoft.Xna.Framework;

namespace Quatrimo
{
    public class transform
    {
        public Vector2I worldPos;
        public Vector2I worldDimensions;
        public Vector2I worldOrigin;
        public float globalRotation;

        public Vector2 localPos;
        public Vector2I elementPos;
        public Vector2I localDimensions;
        public Vector2I localOrigin;
        public float localRotation;

        public void updateChildTransform(transform parent)
        {
            worldPos = (Vector2I)(localPos += parent.localPos);
        }
    }
}