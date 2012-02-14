using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Checkers
{
    public class MainMenu : Menu
    {
        Texture2D logo;
        Vector2 logoPos;

        public MainMenu()
            : base(Font3D.getFont("arial"), Color.OrangeRed, Color.Lime)
        {
            addItem(new ActionItem("Start", this, new Action(Start)));
            addItem(new ActionItem("Settings", this, new Action(Settings)));
            addItem(new ActionItem("Exit", this, new Action(Exit)));

            position = new Vector3(0, -20, 0);

            swing = true;
            spin = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            logo = Content.Load<Texture2D>("Images\\Logo");
            logoPos = new Vector2((Main.size.X - logo.Width) / 2, 10);

            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            if (Input.isPressed(Keys.Escape))
                Program.game.Exit();

            base.Update(gameTime);
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(logo, logoPos, Color.White);

            base.DrawSprites(gameTime, sb);
        }

        public void Start()
        {
            Program.game.setScreen(new GamePlay());
        }

        public void Settings()
        {
            Program.game.setScreen(new SettingsMenu(new MainMenu()));
        }

        public void Exit()
        {
            Program.game.Exit();
        }
    }
}
