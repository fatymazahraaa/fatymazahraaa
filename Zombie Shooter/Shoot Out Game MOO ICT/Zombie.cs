using System.Windows.Forms;
using System;
using System.Drawing;
namespace Zombie_Shooter
{
    public class Zombie : GameObject
    {
        private int speed;
        private Player player;
        private Random rand;
        private string currentDirection; 

        public Zombie(Player player)
        {
            this.player = player;
            this.speed = 1;
            this.rand = new Random();
            this.currentDirection = "down"; 

            Sprite = new PictureBox
            {
                Tag = "zombie",
                SizeMode = PictureBoxSizeMode.AutoSize,
                Image = Properties.Resources.zdown
            };

            Sprite.Left = rand.Next(0, 900);
            Sprite.Top = rand.Next(0, 800);
        }

        public override void Move()
        {
            if (Sprite.Left > player.Sprite.Left)
            {
                Sprite.Left -= speed;
                SetDirection("left");
            }
            else if (Sprite.Left < player.Sprite.Left)
            {
                Sprite.Left += speed;
                SetDirection("right");
            }

            if (Sprite.Top > player.Sprite.Top)
            {
                Sprite.Top -= speed;
                SetDirection("up");
            }
            else if (Sprite.Top < player.Sprite.Top)
            {
                Sprite.Top += speed;
                SetDirection("down");
            }
        }

        private void SetDirection(string direction)
        {
            if (currentDirection == direction)
                return; 

            currentDirection = direction;

            switch (direction)
            {
                case "left":
                    Sprite.Image = Properties.Resources.zleft;
                    break;
                case "right":
                    Sprite.Image = Properties.Resources.zright;
                    break;
                case "up":
                    Sprite.Image = Properties.Resources.zup;
                    break;
                case "down":
                    Sprite.Image = Properties.Resources.zdown;
                    break;
            }
        }

        public override void Update()
        {
            Move();
        }

        public static Zombie Spawn(Form form, Player player, Random rand)
        {
            Zombie newZombie = new Zombie(player);
            newZombie.AddToForm(form);

            newZombie.Sprite.Left = rand.Next(100, form.ClientSize.Width - 100);
            newZombie.Sprite.Top = rand.Next(100, form.ClientSize.Height - 100);

            return newZombie;
        }

        private void AddToForm(Form form)
        {
            form.Controls.Add(Sprite);
            Sprite.BringToFront();
        }
        public void RespawnRandomly(Size clientSize)
        {
            Random rand = new Random();
            int x = rand.Next(0, clientSize.Width - Sprite.Width);
            int y = rand.Next(0, clientSize.Height - Sprite.Height);
            Sprite.Location = new Point(x, y);
        }


    }
}

