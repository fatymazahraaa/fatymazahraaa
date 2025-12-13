using System;
using System.Drawing;
using System.Security.Policy;
using System.Windows.Forms;
namespace Zombie_Shooter
{
    public class Ammo : GameObject
    {
        private Random rand = new Random();

        public Ammo()
        {
            Sprite.Image = Properties.Resources.ammo_Image;
            Sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            Sprite.Tag = "ammo";
            Speed = 0;
        }

        public void RespawnRandomly(Size clientSize)
        {
            Random rand = new Random();
            int x = rand.Next(0, clientSize.Width - Sprite.Width);
            int y = rand.Next(0, clientSize.Height - Sprite.Height);
            Sprite.Location = new Point(x, y);
        }

        public void Spawn(Form form)
        {
            Sprite.Left = rand.Next(10, form.ClientSize.Width - Sprite.Width);
            Sprite.Top = rand.Next(60, form.ClientSize.Height - Sprite.Height);
            AddToForm(form);
        }

        public void Destroy(Form form)
        {
            RemoveFromForm(form);
        }

        // Collision check with player
        public bool CheckCollision(Player player)
        {
            return player.Sprite.Bounds.IntersectsWith(Sprite.Bounds);
        }

        public override void Move()
        {
            // No movement
        }

        public override void Update()
        {
            // No update needed
        }
    }
}
