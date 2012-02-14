using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Checkers
{
    public abstract class King : Piece
    {
        public King(Player pl, Vector3 pos)
            : base(pl, pos)
        {
            possAttacks = new Vector[24];
            possMoves = new Vector[28];

            int m = 0;
            int i = 7;
            while (m < 28 && i > 0)
            {
                if (i != 1 && m < 24)
                {
                    possAttacks[m] = new Vector(-i, -i);
                    possAttacks[m + 1] = new Vector(i, i);
                    possAttacks[m + 2] = new Vector(-i, i);
                    possAttacks[m + 3] = new Vector(i, -i);
                }

                possMoves[m] = new Vector(-i, -i);
                possMoves[m + 1] = new Vector(i, i);
                possMoves[m + 2] = new Vector(-i, i);
                possMoves[m + 3] = new Vector(i, -i);

                m = m + 4;
                i--;
            }
        }

        public override Piece promote(int r)
        {
            return this;
        }

        public override void Draw(Camera c)
        {
            bool tmp = base.selected;
            base.Draw(c);
            currentPos.Y += 1;
            base.selected = tmp;
            base.Draw(c);
            currentPos.Y -= 1;
        }
    }
}
