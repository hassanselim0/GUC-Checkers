using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class Letter
    {
        public int spacing;

        Matrix absTrans;
        ModelMesh mesh;

        public Letter(ModelMesh m)
        {
            spacing = 14;

            ((BasicEffect)m.Effects[0]).EnableDefaultLighting();
            absTrans = Matrix.CreateTranslation(0, m.ParentBone.Transform.Translation.Y, 0);
            mesh = m;
        }

        public void Draw(Matrix transfM, Camera cam, Color clr)
        {
            ((BasicEffect)mesh.Effects[0]).World = absTrans * transfM;
            ((BasicEffect)mesh.Effects[0]).Projection = cam.projection;
            ((BasicEffect)mesh.Effects[0]).View = cam.view;
            ((BasicEffect)mesh.Effects[0]).DiffuseColor = clr.ToVector3();
            mesh.Draw();
        }
    }
}
