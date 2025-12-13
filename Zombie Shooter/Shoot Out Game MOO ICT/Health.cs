using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Zombie_Shooter
{
    public class Health
    {
        public PictureBox Sprite { get; private set; }
        private Random rand;
        private Form1 game;

        public void RespawnRandomly(Size clientSize)
        {
            Random rand = new Random();
            int x = rand.Next(0, clientSize.Width - Sprite.Width);
            int y = rand.Next(0, clientSize.Height - Sprite.Height);
            Sprite.Location = new Point(x, y);
        }

        public Health(Form1 game)
        {
            this.game = game;
            rand = new Random();

            Sprite = new PictureBox
            {
                Image = Properties.Resources.healthItem,
                Size = new Size(30, 30),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            Sprite.Location = new Point(rand.Next(50, game.ClientSize.Width - 50), rand.Next(50, game.ClientSize.Height - 50));
            game.Controls.Add(Sprite);
        }

        public void Destroy(Form1 game)
        {
            game.Controls.Remove(Sprite);
        }

        // Collision check with player and apply effect
        public bool CheckCollision(Player player)
        {
            if (player.Sprite.Bounds.IntersectsWith(Sprite.Bounds))
            {
                player.Health = Math.Min(player.Health + 20, 100);
                Destroy(game);
                return true;
            }
            return false;
        }
    }
}
