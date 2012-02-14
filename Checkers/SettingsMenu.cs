using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Checkers
{
    public class SettingsMenu : Menu
    {
        Menu previous;
        string p1Pic, p2Pic;

        public SettingsMenu(Menu p)
            : base(Font3D.getFont("arial"), Color.OrangeRed, Color.Lime)
        {
            addItem(new TextItem("P1 Name", this));
            addItem(new ActionItem("P1 Avatar", this, new Action(ChooseP1Pic)));
            addItem(new TextItem("P2 Name", this));
            addItem(new ActionItem("P2 Avatar", this, new Action(ChooseP2Pic)));
            addItem(new ActionItem("Save", this, new Action(Save)));
            addItem(new ActionItem("Cancel", this, new Action(Back)));

            previous = p;

            cam.position.Z = 300;
            cam.updateView();
        }

        public void ChooseP1Pic()
        {
            ChoosePicture(true);
        }

        public void ChooseP2Pic()
        {
            ChoosePicture(false);
        }

        public void ChoosePicture(bool p1)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Choose an Avatar for Player " + (p1 ? "1" : "2");
            dialog.Filter = "Images|*.jpg;*.png;*.bmp;*.tga";

            if (dialog.ShowDialog() == DialogResult.OK)
                if (p1)
                    p1Pic = dialog.FileName;
                else
                    p2Pic = dialog.FileName;
		}

        public void Save()
        {
            GamePlay.p1Settings.name = ((TextItem)items.First.Value).input;
            GamePlay.p1Settings.setAvatar(p1Pic);

            GamePlay.p2Settings.name = ((TextItem)items.First.Next.Next.Value).input;
            GamePlay.p2Settings.setAvatar(p2Pic);

            Back();
        }

        public void Back()
        {
            Program.game.setScreen(previous);
        }
    }
}
