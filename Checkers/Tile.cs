using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Checkers
{
    public class Tile
    {
        public Piece p;
        public bool killPc;

        public Tile(Piece pc)
        {
            p = pc;
            killPc = false;
        }

        public void Update()
        {
            if (p != null)
                p.Update();

            if (killPc)
                if (p.kill())
                {
                    p = null;
                    killPc = false;
                }
        }

        public void Draw(Camera c)
        {
            if (p != null)
                p.Draw(c);
        }
    }
}
