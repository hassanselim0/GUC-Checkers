using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class Font3D
    {
        static Dictionary<string,Font3D> fonts;

        public string name;
        Dictionary<char, Letter> letters;

        public static void Initialize(ContentManager Content)
        {
            fonts = new Dictionary<string, Font3D>();

            string[] sa = Directory.GetFiles("Content\\Text3D");
            for (int i = 0; i < sa.Length; i++)
            {
                string s = sa[i].Substring(15, sa[i].Length - 19);
                Font3D f = new Font3D();
                f.LoadContent(Content, s);
                fonts.Add(s.ToLower(), f);
            }
        }

        public static Font3D getFont(string name)
        {
            name = name.ToLower();
            return fonts[name];
        }

        private void LoadContent(ContentManager Content, string font)
        {
            name = font;
            letters = new Dictionary<char, Letter>(52);

            Model m = Content.Load<Model>("Text3D\\" + font);
            foreach (ModelMesh mesh in m.Meshes)
                letters.Add(mesh.Name[0], new Letter(mesh));
        }

        public Letter getLetter(char c)
        {
            Letter l;
            letters.TryGetValue(c, out l);
            return l;
        }
    }
}
