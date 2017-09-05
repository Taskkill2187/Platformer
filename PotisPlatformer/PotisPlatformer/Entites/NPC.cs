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
    public class NPC : Entity, ICloneable
    {
        bool DialogRunning;
        public int DialogState;
        public bool FacingRight;
        List<SoundEffect> Dialog = new List<SoundEffect>();
        int SoundCooldown;

        public NPC(int PosX, int PosY, Texture2D Tex, List<SoundEffect> Dialog, Level Parent)
        {
            this.Vel = Vector2.Zero;
            this.Texture = Tex;
            this.Rect = new Rectangle(PosX, PosY, Parent.ThisPlayer.Rect.Width, Parent.ThisPlayer.Rect.Height);
            this.DialogRunning = false;
            this.Dialog = Dialog;
            DialogState = 0;
            this.Parent = Parent;
        }

        public void StartDialog()
        {
            DialogState = 0;
            DialogRunning = true;
            Dialog[0].Play(0.2f, 0, 0);
            SoundCooldown = (int)Dialog[DialogState].Duration.Seconds * 60 + 30;
            Parent.ThisPlayer.RespawnPoint = new Vector2(this.Rect.X, this.Rect.Y);

            if (Parent.ThisPlayer.Rect.X > Rect.X)
            {
                FacingRight = true;
                Parent.ThisPlayer.Vel = Vector2.Zero;
                Parent.ThisPlayer.Rect.X = this.Rect.X + 5 + this.Rect.Width;
                Parent.ThisPlayer.FacingRight = false;
            }
            else
            {
                FacingRight = false;
                Parent.ThisPlayer.Vel = Vector2.Zero;
                Parent.ThisPlayer.Rect.X = this.Rect.X - 5 - this.Rect.Width;
                Parent.ThisPlayer.FacingRight = true;
            }
        }

        public void Update()
        {
            if (DialogRunning)
            {
                Parent.ThisPlayer.CanMove = false;
                if (SoundCooldown < 0)
                {
                    if (DialogState < Dialog.Count - 1)
                    {
                        DialogState++;
                        Dialog[DialogState].Play(0.2f, 0, 0);
                        SoundCooldown = (int)Dialog[DialogState].Duration.Seconds * 60 + 30;
                    }
                    else
                    {
                        DialogRunning = false;
                    }
                }
            }
            else
            {
                Parent.ThisPlayer.CanMove = true;
            }
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
