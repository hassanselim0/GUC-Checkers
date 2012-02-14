using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public abstract class Screen
    {
        public Camera cam;

        public abstract void LoadContent(ContentManager Content);

        public abstract void Update(GameTime gameTime);

        public virtual void DrawBackground(GameTime gameTime, SpriteBatch sb)
        {
        }

        public abstract void Draw(GameTime gameTime);

        public abstract void DrawSprites(GameTime gameTime, SpriteBatch sb);
    }
}
