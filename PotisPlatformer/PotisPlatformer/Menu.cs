using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{
    public enum MenuButtonLayout { MiddleVert, MiddleHorz, TopHorz }

    public class Menu
    {
        public List<ControlElement> ControlElementList = new List<ControlElement>();

        public Menu()
        {
            List<Button> ButtonList = new List<Button>();
        }

        public void ArrangeButtons(MenuButtonLayout Layout)
        {
            switch (Layout)
            {
                case MenuButtonLayout.MiddleVert:
                    for (int i = 0; i < ControlElementList.Count; i++)
                    {
                        if (ControlElementList[i].GetType() == typeof(Button))
                            ((Button)ControlElementList[i]).Center = new Vector2(Values.WindowSize.X / 2, Values.WindowSize.Y / 2 + (i - ControlElementList.Count / 2) * 100);
                    }
                    break;

                case MenuButtonLayout.MiddleHorz:
                    for (int i = 0; i < ControlElementList.Count; i++)
                    {
                        if (ControlElementList[i].GetType() == typeof(Button))
                            ((Button)ControlElementList[i]).Center = new Vector2(Values.WindowSize.X / 2 + (i - ControlElementList.Count / 2) * 100, Values.WindowSize.Y / 2);
                    }
                    break;

                case MenuButtonLayout.TopHorz:
                    for (int i = 0; i < ControlElementList.Count; i++)
                    {
                        if (ControlElementList[i].GetType() == typeof(Button))
                            ((Button)ControlElementList[i]).Center = new Vector2(Values.WindowSize.X / 2 + (i - ControlElementList.Count / 2) * 100, ControlElementList[i].Rect.Height / 2 + 12);
                    }
                    break;
            }
            for (int i = 0; i < ControlElementList.Count; i++)
            {
                if (ControlElementList[i].GetType() == typeof(Button))
                    ((Button)ControlElementList[i]).FitRectangleToCenter();
            }
        }

        public void Update()
        {
            if (Controls.WasLMBJustPressed())
            {
                MenuManager.CurrentSelectedElement = null;
            }

            for (int i = 0; i < ControlElementList.Count; i++)
            {
                ControlElementList[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ControlElementList.Count; i++)
            {
                ControlElementList[i].Draw(spriteBatch);
            }
        }
    }
}
