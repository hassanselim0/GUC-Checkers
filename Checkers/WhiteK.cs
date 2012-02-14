using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Checkers
{
    public class WhiteK : King
    {
        public WhiteK(Player pl, Vector3 pos)
            : base(pl, pos)
        {
            color = new Vector3(1, 1, 0.9f);
        }
    }
}
