using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public abstract class Piece
    {
        public static Model model;
        static Matrix[] aTrans;
        
        public Vector3 color;
        public bool selected = false;
        public Vector[] possMoves;
        public Vector[] possAttacks;

        public Vector3 currentPos;
        public Vector3 targetPos;
        public Vector3 halfWay;
        public bool reachedHalfWay;
        public bool moving;

        public float alpha;

        public Player owner;

        public Piece(Player pl, Vector3 pos)
        {
            owner = pl;
            currentPos = pos;
            reachedHalfWay = false;
            moving = false;
            alpha = 1.0f;
        }

        public static void setModel(Model m)
        {
            model = m;
            ((BasicEffect)m.Meshes[0].Effects[0]).EnableDefaultLighting();

            aTrans = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(aTrans);
        }

        public bool isHis(Player p)
        {
            return p == owner;
        }

        public abstract Piece promote(int r);

        public void setTarget(Vector v)
        {
            moving = true;
            targetPos = new Vector3(v.x * 5, 0, v.y * 5);
            halfWay = (targetPos - currentPos) / 2f + currentPos;
        }

        public bool kill()
        {
            alpha -= 0.01f;

            if (alpha <= 0)
                return true;
            return false;
        }

        public void Update()
        {
            Vector3 delta = targetPos - currentPos;
            Vector3 halfDelta = halfWay - currentPos;

            if (moving)
            {
                targetPos.Y = currentPos.Y;

                if (delta.Length() > 0.6f)
                    currentPos += Vector3.Normalize(delta) / 2f;
                else
                    currentPos = targetPos;

                halfWay.Y = currentPos.Y;

                if (!reachedHalfWay)
                {
                    if (currentPos.Y < 4)
                        currentPos.Y += 0.2f;
                    if (halfDelta.Length() < 0.6f)
                        reachedHalfWay = true;
                }
                else
                {
                    if (currentPos.Y < 0)
                        currentPos.Y = 0;
                    else
                        currentPos.Y -= 0.2f;
                }

                if (delta.Length() < 0.6f && currentPos.Y == 0)
                {
                    moving = false;
                    reachedHalfWay = false;
                }
            }
        }

        public virtual void Draw(Camera c)
        {
            Vector3 clr;
            if (selected)
            {
                clr = new Vector3(1, 0, 0);
                selected = false;
            }
            else
                clr = color;
            ((BasicEffect)model.Meshes[0].Effects[0]).DiffuseColor = clr;
            ((BasicEffect)model.Meshes[0].Effects[0]).Alpha = alpha;
            c.render(model, Matrix.CreateTranslation(currentPos), aTrans);
        }
    }
}
