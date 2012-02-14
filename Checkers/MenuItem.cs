using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public abstract class MenuItem
    {
        public Text3D text;
        public Vector3 position;
        public float spin;

        public MenuItem(String s, Menu m)
        {
            text = new Text3D(s, m.font, m.cam);
            position = new Vector3(text.getWidth() / -2, -10 * m.items.Count, 0);
            spin = 0;
        }

        public abstract void Update();

        public void Draw(Color c, Vector3 offset)
        {
            text.Draw(Matrix.CreateTranslation(0, -6, 0) * Matrix.CreateRotationX(spin)
                * Matrix.CreateTranslation(position + offset), c);
        }
    }
}
