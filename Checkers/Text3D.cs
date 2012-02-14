using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class Text3D
    {
        public Camera cam;
        public string str;
        Font3D font;

        public Text3D(string s ,Font3D f, Camera c)
        {
            str = s;
            font = f;
            cam = c;
        }

        public float getWidth()
        {
            return 14 * str.Length;
        }

        public void Draw(Matrix transfM, Color clr)
        {
            Vector3 startTrans = transfM.Translation;

            for (int i = 0; i < str.Length; i++)
            {
                Letter l = font.getLetter(str[i]);
                if (l != null)
                {
                    l.Draw(transfM, cam, clr);
                    transfM.Translation += transfM.Right * l.spacing;
                }
                else
                {
                    if (str[i] == '\n')
                    {
                        startTrans += transfM.Down * 20;
                        transfM.Translation = startTrans;
                    }
                    else
                        transfM.Translation += transfM.Right * 14;
                }
            }
        }
    }
}
