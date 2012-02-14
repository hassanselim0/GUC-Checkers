using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Checkers
{
    public class TextItem : MenuItem
    {
        public string name, input;

        public TextItem(String s, Menu m)
            : base(s + ":", m)
        {
            name = s;
            input = "";
        }

        public override void Update()
        {
            string delta = Input.getText();
            if (delta != "" || Input.isPressed(Keys.Back))
            {
                if (delta != "")
                    input += delta;
                if (Input.isPressed(Keys.Back))
                    input = input.Substring(0, input.Length - 1);
                text.str = name + ":" + input;
                position.X = text.getWidth() / -2;
            }
        }
    }
}
