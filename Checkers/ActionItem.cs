using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Checkers
{
    public class ActionItem : MenuItem
    {
        public Action action;

        public ActionItem(String s, Menu m, Action a)
            : base(s, m)
        {
            action = a;
        }

        public override void Update()
        {
            if (action != null && Input.isPressed(Keys.Enter))
                action();
        }
    }
}
