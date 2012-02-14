using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class GamePlay : Screen
    {
        public static Player currPl;

        public static Board board;

        public static PlayerSettings p1Settings, p2Settings;

        Model selector;
        Matrix[] selectorTrans;

        Texture2D background;
        Rectangle destRect;
        Rectangle srcRect;

        public GamePlay()
        {
            Player p1 = new Player(p1Settings, 0);
            Player p2 = new Player(p2Settings, 1);
            p1.next = p2;
            p2.next = p1;
            currPl = p1;

            board = new Board();

            cam = p1.cam.clone();
        }

        public override void LoadContent(ContentManager Content)
        {
            Board.setModel(Content.Load<Model>("Models\\board"));
            Piece.setModel(Content.Load<Model>("Models\\piece"));

            selector = Content.Load<Model>("Models\\selector");
            selectorTrans = new Matrix[selector.Bones.Count];
            selector.CopyAbsoluteBoneTransformsTo(selectorTrans);
            ((BasicEffect)selector.Meshes[0].Effects[0]).Alpha = 0.6f;
            ((BasicEffect)selector.Meshes[0].Effects[0]).DiffuseColor = Vector3.UnitX;

            Player.LoadContent(Content);

            background = Content.Load<Texture2D>("Images\\background");

            Viewport v = Program.game.GraphicsDevice.Viewport;
            destRect = new Rectangle(0, 0, v.Width, v.Height);
            int x, y, w, h;
            if (background.Width / background.Height > destRect.Width / destRect.Height)
            {
                h = background.Height;
                w = background.Height * destRect.Width / destRect.Height;
                y = 0;
                x = (background.Width - w) / 2;
            }
            else
            {
                h = background.Width * destRect.Height / destRect.Width;
                w = background.Width;
                y = (background.Height - h) / 2;
                x = 0;
            }
            srcRect = new Rectangle(x, y, w, h);
        }

        public override void Update(GameTime gameTime)
        {
            cam.updateGoTo(1.6f);

            board.Update();

            if (isGameOver())
                return;

            if (board.attackAgain != null)
                currPl.src = board.attackAgain;
            currPl.getInput();
            if (currPl.src != null)
            {
                board.getPc(currPl.src).selected = true;
                if (currPl.dest != null)
                {
                    board.applyAction(currPl.src, currPl.dest);
                    currPl.src = null;
                    currPl.dest = null;
                    if (isGameOver())
                        return;
                    if (board.attackAgain == null)
                    {
                        currPl = currPl.next;
                        if (!(currPl is AI1))
                            cam.goTo = currPl.cam;
                    }
                }
            }
        }

        private bool isGameOver()
        {
            bool b = board.isStuck();
            if (b)
            {
                System.Windows.Forms.MessageBox.Show(currPl.next.name + " WINS !!!");
                System.Threading.Thread.Sleep(1000);
                Program.game.setScreen(new MainMenu());
            }
            return b;
        }

        public override void DrawBackground(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(background, destRect, srcRect, Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            Program.game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            board.Draw(cam);

            if (currPl.selection != null)
                cam.render(selector, Matrix.CreateTranslation(currPl.selection.x * 5, 0, currPl.selection.y * 5),
                    selectorTrans);

            currPl.DrawLabel(cam);
            currPl.next.DrawLabel(cam);
        }

        public override void DrawSprites(GameTime gameTime, SpriteBatch sb)
        {
        }
    }
}
