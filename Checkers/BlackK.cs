using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Checkers
{
    public class BlackK : King
    {
        public BlackK(Player pl, Vector3 pos)
            : base(pl, pos)
        {
            color = new Vector3(0.2f);
        }
    }
}
