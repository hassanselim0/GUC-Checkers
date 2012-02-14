using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Checkers
{
    public class BlackPc : Piece
    {
        public BlackPc(Player pl, Vector3 pos)
            : base(pl, pos)
        {
            color = new Vector3(0.2f);
            possMoves = new Vector[] { new Vector(-1, 1), new Vector(1, 1) };
            possAttacks = new Vector[] { new Vector(-2, 2), new Vector(2, 2) };
        }

        public override Piece promote(int r)
        {
            if (r == 7)
            {
                BlackK k = new BlackK(owner, currentPos);
                k.halfWay = halfWay;
                k.moving = moving;
                k.targetPos = targetPos;
                return k;
            }
            else
                return this;
        }
    }
}
