using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public struct PlayerSettings
    {
        public string name;
        public Texture2D avatar;

        public void setAvatar(string file)
        {
            if (file == null || file == "")
                avatar = Texture2D.FromStream(Program.game.GraphicsDevice, System.IO.File.OpenRead("default.png"));
            else
            {
                var tex = Texture2D.FromStream(Program.game.GraphicsDevice, System.IO.File.OpenRead(file));

                int size;
                if (tex.Height > tex.Width)
                    size = tex.Width;
                else if (tex.Height < tex.Width)
                    size = tex.Height;
                else
                {
                    avatar = tex;
                    return;
                }

                Color[] original = new Color[tex.Height * tex.Width];
                tex.GetData<Color>(original);

                Color[] cropped = new Color[size * size];
                if (tex.Height > tex.Width)
                {
                    int start = ((tex.Height - tex.Width) / 2) * tex.Width;
                    for (int i = start; i < cropped.Length + start; i++)
                        cropped[i - start] = original[i];
                }
                else
                {
                    int offset = (tex.Width - tex.Height) / 2;
                    int j = offset;
                    for (int i = 0; i < cropped.Length; i++)
                    {
                        cropped[i] = original[j];
                        if (i != 0 && i % size == 0)
                        {
                            j += offset * 2;
                            if ((tex.Width - tex.Height) % 2 == 1)
                                j++;
                        }
                        j++;
                    }
                }

                for (int i = 0; i < cropped.Length; i++)
                    cropped[i].A = 255;

                avatar = new Texture2D(Program.game.GraphicsDevice, size, size);
                avatar.SetData<Color>(cropped);
            }
        }
    }
}
