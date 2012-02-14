using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class Camera
    {
        Vector3 rotation;
        public Vector2 rot2;
        public Vector3 position;
        public Vector3 target;
        Vector3 up;
        public float zoom;
        public Matrix view;
        public Matrix projection;

        public Camera goTo;

        public Vector3 Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                if (rotation.X > 1.5f)
                    rotation.X = 1.5f;
                if (rotation.X < -1.5f)
                    rotation.X = 1.5f;
                position = (Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y)).Forward * zoom
                    + target;
                up = Vector3.Transform(Vector3.Up, Matrix.CreateRotationZ(rotation.Z));
                updateView();
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                updateView();
            }
        }

        public Camera()
        {
        }

        public Camera(Vector3 pos, Vector3 tar, Vector3 u)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 800f / 480f, 1, 1000);
            up = u;
            target = tar;
            Position = pos;
        }

        public Camera(Vector3 rot, float z, Vector3 tar, Vector3 u)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 800f / 480f, 1, 1000);
            up = u;
            target = tar;
            zoom = z;
            Rotation = rot;
        }

        public Camera clone()
        {
            Camera r = new Camera();
            r.goTo = goTo;
            r.position = new Vector3(position.X,position.Y,position.Z);
            r.projection = projection;
            r.rot2 = new Vector2(rot2.X, rot2.Y);
            r.rotation = new Vector3(rotation.X, rotation.Y, rotation.Z);
            r.target = new Vector3(target.X, target.Y, target.Z);
            r.up = new Vector3(up.X, up.Y, up.Z);
            r.view = view;
            r.zoom = zoom;
            return r;
        }

        public void updateView()
        {
            view = Matrix.CreateLookAt(position, target, up);
        }

        public void rotate3rdPerson(Vector3 center, Vector3 delta)
        {
            target = center;
            Rotation += new Vector3(delta.X, delta.Y, delta.Z);
        }

        public void zoomWithMouse()
        {
            zoom += Input.mouseDeltaScroll() / 4f;
        }

        public void rotate1stPerson()
        {
            Vector2 delta = Input.mouseDeltaMovement();
            if (delta != Vector2.Zero)
            {
                rot2.X += delta.X;
                rot2.Y += delta.Y;
                if (rot2.Y > 80)
                    rot2.Y = 80;
                if (rot2.Y < -80)
                    rot2.Y = -80;
            }
            target.Y = 1 * (float)Math.Sin(MathHelper.ToRadians(rot2.Y)) + position.Y;
            target.Z = 1 * (float)Math.Cos(MathHelper.ToRadians(rot2.Y))
                * (float)Math.Cos(MathHelper.ToRadians(rot2.X)) + position.Z;
            target.X = 1 * (float)Math.Cos(MathHelper.ToRadians(rot2.Y))
                * (float)Math.Sin(MathHelper.ToRadians(rot2.X)) + position.X;

            updateView();
        }

        public void move1stPerson(float delta)
        {
            position += Vector3.Normalize(target - position) * delta;
        }

        public void starf1stPerson(float delta)
        {
            position += Vector3.Normalize(Vector3.Cross(target - position, Vector3.Up)) * delta;
        }

        public void updateGoTo(float speed)
        {
            if (goTo != null)
            {
                if (rotation == Vector3.Zero && goToTarget(goTo.target, speed)
                    && goToPosition(goTo.position, speed)
                    || rotation != Vector3.Zero && goToTarget(goTo.target, speed)
                    && goToRotation(goTo.rotation, goTo.zoom, speed))
                    goTo = null;
            }
        }

        public bool goToTarget(Vector3 dest, float speed)
        {
            if (Math.Abs(target.X - dest.X) < 0.4 && Math.Abs(target.Y - dest.Y) < 0.4
                && Math.Abs(target.Z - dest.Z) < 0.4)
            {
                target.X = dest.X;
                target.Y = dest.Y;
                target.Z = dest.Z;
                updateView();
                return true;
            }

            if (Math.Abs(target.X - dest.X) < 10 && Math.Abs(target.X - dest.X) > 0.4)
                target.X += (target.X - dest.X > 0 ? -0.1f : 0.1f) * speed;
            else
                target.X -= (target.X - dest.X) / 100 * speed;

            if (Math.Abs(target.Y - dest.Y) < 10 && Math.Abs(target.Y - dest.Y) > 0.4)
                target.Y += (target.Y - dest.Y > 0 ? -0.1f : 0.1f) * speed;
            else
                target.Y -= (target.Y - dest.Y) / 100 * speed;

            if (Math.Abs(target.Z - dest.Z) < 10 && Math.Abs(target.Z - dest.Z) > 0.4)
                target.Z += (target.Z - dest.Z > 0 ? -0.1f : 0.1f) * speed;
            else
                target.Z -= (target.Z - dest.Z) / 100 * speed;

            updateView();

            return false;
        }

        public bool goToPosition(Vector3 dest, float speed)
        {
            if (Math.Abs(position.X - dest.X) < 0.4 && Math.Abs(position.Y - dest.Y) < 0.4
                && Math.Abs(position.Z - dest.Z) < 0.4)
            {
                position.X = dest.X;
                position.Y = dest.Y;
                position.Z = dest.Z;
                updateView();
                return true;
            }

            if (Math.Abs(position.X - dest.X) < 10 && Math.Abs(position.X - dest.X) > 0.4)
                position.X += (position.X - dest.X > 0 ? -0.1f : 0.1f) * speed;
            else
                position.X -= (position.X - dest.X) / 100 * speed;

            if (Math.Abs(position.Y - dest.Y) < 10 && Math.Abs(position.Y - dest.Y) > 0.4)
                position.Y += (position.Y - dest.Y > 0 ? -0.1f : 0.1f) * speed;
            else
                position.Y -= (position.Y - dest.Y) / 100 * speed;

            if (Math.Abs(position.Z - dest.Z) < 10 && Math.Abs(position.Z - dest.Z) > 0.4)
                position.Z += (position.Z - dest.Z > 0 ? -0.1f : 0.1f) * speed;
            else
                position.Z -= (position.Z - dest.Z) / 100 * speed;

            updateView();

            return false;
        }

        public bool goToRotation(Vector3 dest, float z, float speed)
        {
            if (Math.Abs(rotation.X - dest.X) < 0.04 && Math.Abs(rotation.Y - dest.Y) < 0.04
                && Math.Abs(rotation.Z - dest.Z) < 0.04 && Math.Abs(zoom - z) < 0.4)
            {
                rotation.X = dest.X;
                rotation.Y = dest.Y;
                rotation.Z = dest.Z;
                Rotation = rotation;
                return true;
            }

            if (Math.Abs(rotation.X - dest.X) < 0.6f && Math.Abs(rotation.X - dest.X) > 0.04)
                rotation.X += (rotation.X - dest.X > 0 ? -0.01f : 0.01f) * speed;
            else
                rotation.X -= (rotation.X - dest.X) / 60 * speed;

            if (Math.Abs(rotation.Y - dest.Y) < 0.6f && Math.Abs(rotation.Y - dest.Y) > 0.04)
                rotation.Y += (rotation.Y - dest.Y > 0 ? -0.01f : 0.01f) * speed;
            else
                rotation.Y -= (rotation.Y - dest.Y) / 60 * speed;

            if (Math.Abs(rotation.Z - dest.Z) < 0.6f && Math.Abs(rotation.Z - dest.Z) > 0.04)
                rotation.Z += (rotation.Z - dest.Z > 0 ? -0.01f : 0.01f) * speed;
            else
                rotation.Z -= (rotation.Z - dest.Z) / 60 * speed;

            if (Math.Abs(zoom - z) < 10 && Math.Abs(zoom- z) > 0.4)
                zoom += (zoom - z > 0 ? -0.1f : 0.1f) * speed;
            else
                zoom -= (zoom - z) / 100 * speed;

            Rotation = rotation;

            return false;
        }

        public void render(Model model, Matrix modelTransform, Matrix[] absoluteTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = absoluteTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
    }
}
