using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using System;

namespace Quatrimo
{
    public class blockSpecification
    {   //change int mod to a new type that can hold a single mod, or multiple mods picked at random
        public int mod;
        public Color color;
        public Texture2DRegion tex;

        public Action<bagBlock> overwrite;

        public blockSpecification(int mod, Color color, Texture2DRegion tex)
        {
            this.mod = mod;
            this.color = color;
            this.tex = tex;
            overwrite += (bagBlock block) => block.mod = mod;
            overwrite += (bagBlock block) => block.color = color;
            overwrite += (bagBlock block) => block.tex = tex;
        }

        public blockSpecification(int mod, Color color)
        {
            this.mod = mod;
            this.color = color;
            overwrite += (bagBlock block) => block.mod = mod;
            overwrite += (bagBlock block) => block.color = color;
        }

        public blockSpecification(int mod)
        {
            this.mod = mod;
            overwrite += (bagBlock block) => block.mod = mod;
        }

        public blockSpecification(int mod, Texture2DRegion tex)
        {
            this.mod = mod;
            this.tex = tex;
            overwrite += (bagBlock block) => block.mod = mod;
            overwrite += (bagBlock block) => block.tex = tex;
        }

        public blockSpecification(Color color, Texture2DRegion tex)
        {
            this.color = color;
            this.tex = tex;
            overwrite += (bagBlock block) => block.color = color;
            overwrite += (bagBlock block) => block.tex = tex;
        }

        public blockSpecification(Texture2DRegion tex)
        {
            this.tex = tex;
            overwrite += (bagBlock block) => block.mod = mod;
            overwrite += (bagBlock block) => block.color = color;
            overwrite += (bagBlock block) => block.tex = tex;
        }
    }
}