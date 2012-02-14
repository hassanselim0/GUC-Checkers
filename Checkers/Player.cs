using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Checkers
{
    public class Player
    {
        static Model label;
        static Matrix[] labelTrans;

        public string name;
        Text3D nameTag;
        Texture2D avatar;

        public Player next;
        public Camera cam;
        public Vector selection;
        public Vector src;
        public Vector dest;

        int index;

        public Player(PlayerSettings p, int i)
	    {
            if (p.name == null)
                name = "Player " + (i + 1);
            else
                name = p.name;

            nameTag = new Text3D(name, Font3D.getFont("arial"), null);

            if (p.avatar == null)
                avatar = Texture2D.FromStream(Program.game.GraphicsDevice, System.IO.File.OpenRead("default.png"));
            else
                avatar = p.avatar;

            cam = new Camera(new Vector3(0.7f, i == 1 ? 0 : MathHelper.Pi, 0), 80, Vector3.Zero, Vector3.Up);
            selection = new Vector(0, i == 1 ? 0 : 7);
            index = i;
	    }

        public static void LoadContent(ContentManager Content)
        {
            label = Content.Load<Model>("Models\\label");
            labelTrans = new Matrix[label.Bones.Count];
            label.CopyAbsoluteBoneTransformsTo(labelTrans);

            BasicEffect be = (BasicEffect)label.Meshes[0].Effects[0];
            be.EnableDefaultLighting();
            be.DiffuseColor = Color.OrangeRed.ToVector3();
            be.DirectionalLight0.Direction = new Vector3(0, -1f, 0);
            be.DirectionalLight1.Enabled = false;
            be.DirectionalLight2.Enabled = false;
        }

        public virtual void getInput()
        {
            int delta = index == 0 ? 1 : -1;

            if (Input.isPressed(Keys.Up))
                selection.y -= delta;
            if (Input.isPressed(Keys.Down))
                selection.y += delta;
            if (Input.isPressed(Keys.Right))
                selection.x += delta;
            if (Input.isPressed(Keys.Left))
                selection.x -= delta;

            if (selection.x > 7)
                selection.x = 7;
            if (selection.x < 0)
                selection.x = 0;
            if (selection.y > 7)
                selection.y = 7;
            if (selection.y < 0)
                selection.y = 0;

            if (Input.isPressed(Keys.Enter))
            {
                if (src == null)
                {
                    GamePlay.board.validateSrcPc(selection);
                    src = selection.clone();
                }
                else
                    if (Vector.isEqual(src, selection))
                        src = null;
                    else
                    {
                        GamePlay.board.validateDestPc(selection);
                        dest = selection.clone();
                    }
            }
        }

        public void DrawLabel(Camera c)
        {
            nameTag.cam = c;

            ((BasicEffect)label.Meshes[1].Effects[0]).Texture = avatar;
            ((BasicEffect)label.Meshes[2].Effects[0]).Texture = avatar;
            c.render(label, Matrix.CreateTranslation(0, -5, 0)
                * Matrix.CreateRotationX(-c.Rotation.Y / 2 + MathHelper.PiOver4)
                * Matrix.CreateTranslation(0, 5, index == 0 ? 30 : -30), labelTrans);

            nameTag.Draw(Matrix.CreateTranslation(0, -5, 0)
                * Matrix.CreateScale(0.2f)
                * Matrix.CreateRotationY(index == 0 ? 0 : MathHelper.Pi)
                * Matrix.CreateRotationX(-c.Rotation.Y / 2 + MathHelper.PiOver4)
                * Matrix.CreateTranslation(index == 0 ? -8 : 8, 5, index == 0 ? 31 : -31), Color.Green);
            nameTag.Draw(Matrix.CreateTranslation(0, -5, 0)
                * Matrix.CreateScale(0.2f)
                * Matrix.CreateRotationY(index == 1 ? 0 : MathHelper.Pi)
                * Matrix.CreateRotationX(-c.Rotation.Y / 2 + MathHelper.PiOver4)
                * Matrix.CreateTranslation(index == 1 ? -8 : 8, 5, index == 0 ? 29 : -29), Color.Green);
        }
    }
}
