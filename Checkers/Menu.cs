using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Checkers
{
    public class Menu : Screen
    {
        public Font3D font;
        public LinkedList<MenuItem> items;
        LinkedListNode<MenuItem> selection;
        Color color1, color2;

        public Vector3 position;
        public bool spin;
        public bool swing;
        bool positive;

        public Menu(Font3D f, Color c1, Color c2)
        {
            cam = new Camera(new Vector3(0, MathHelper.Pi, 0), 240, Vector3.Zero, Vector3.Up);

            font = f;
            color1 = c1;
            color2 = c2;
            items = new LinkedList<MenuItem>();
            positive = true;
        }

        public override void LoadContent(ContentManager Content)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (swing)
            {
                if (cam.goTo == null)
                {
                    if (positive)
                        cam.goTo = new Camera(new Vector3(0, 2.9f, 0), 240, Vector3.Zero, Vector3.Up);
                    else
                        cam.goTo = new Camera(new Vector3(0, 3.36f, 0), 240, Vector3.Zero, Vector3.Up);
                    positive = !positive;
                }
                cam.updateGoTo(0.8f);
            }

            foreach (MenuItem item in items)
            {
                if (spin && item == selection.Value || item.spin > 0.1f)
                    item.spin += 0.1f;
                else
                    item.spin = 0;

                if (item.spin > MathHelper.TwoPi)
                    item.spin -= MathHelper.TwoPi;
            }

            if (Input.isPressed(Keys.Down))
                if (selection == items.Last)
                    selection = items.First;
                else
                    selection = selection.Next;

            if (Input.isPressed(Keys.Up))
                if (selection == items.First)
                    selection = items.Last;
                else
                    selection = selection.Previous;

            selection.Value.Update();
        }

        public void addItem(MenuItem m)
        {
            foreach (MenuItem item in items)
                item.position.Y += 10;
            items.AddLast(m);
            if (items.Count == 1)
                selection = items.First;
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (MenuItem item in items)
                if (selection.Value == item)
                    item.Draw(color1, position);
                else
                    item.Draw(color2, position);
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch sb)
        {
        }
    }
}
