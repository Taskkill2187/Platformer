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

        public NPC(int PosX, int PosY, Texture2D Tex, List<SoundEffect> Dialog)
        {
            this.Vel = Vector2.Zero;
            this.Texture = Tex;
            this.Rect = new Rectangle(PosX, PosY, LevelManager.ThisPlayer.Rect.Width, LevelManager.ThisPlayer.Rect.Height);
            this.DialogRunning = false;
            this.Dialog = Dialog;
            DialogState = 0;
        }

        public void StartDialog()
        {
            DialogState = 0;
            DialogRunning = true;
            Dialog[0].Play(0.2f, 0, 0);
            SoundCooldown = (int)Dialog[DialogState].Duration.Seconds * 60 + 30;
            LevelManager.ThisPlayer.RespawnPoint = new Vector2(this.Rect.X, this.Rect.Y);

            if (LevelManager.ThisPlayer.Rect.X > Rect.X)
            {
                FacingRight = true;
                LevelManager.ThisPlayer.Vel = Vector2.Zero;
                LevelManager.ThisPlayer.Rect.X = this.Rect.X + 5 + this.Rect.Width;
                LevelManager.ThisPlayer.FacingRight = false;
            }
            else
            {
                FacingRight = false;
                LevelManager.ThisPlayer.Vel = Vector2.Zero;
                LevelManager.ThisPlayer.Rect.X = this.Rect.X - 5 - this.Rect.Width;
                LevelManager.ThisPlayer.FacingRight = true;
            }
        }

        public void Update()
        {
            if (DialogRunning)
            {
                LevelManager.ThisPlayer.CanMove = false;
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
                LevelManager.ThisPlayer.CanMove = true;
            }
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
