using System.Windows.Forms;
using System.Drawing;
using System;
namespace Zombie_Shooter
{
    public class Player : GameObject
    {
        public int Health { get; set; }
        public int Ammo { get; set; }
        public string Facing { get; set; }
        public int MoveSpeed { get; set; } = 10;
        public bool IsAlive => Health > 0;

        public PlayerStatus Status
        {
            get => IsAlive ? PlayerStatus.Alive : PlayerStatus.Dead;
        }

        public Player()
        {
            Sprite = new PictureBox();
            Sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            Sprite.Image = Properties.Resources.up;
            Sprite.Tag = "player";
            Sprite.Left = 100;
            Sprite.Top = 100;
            Health = 100;
            Ammo = 10;
            Facing = "up";
        }

        public override void Move()
        {
            if (Facing == "left")
            {
                MoveLeft();
            }
            else if (Facing == "right")
            {
                MoveRight(800);
            }
            else if (Facing == "up")
            {
                MoveUp();
            }
            else if (Facing == "down")
            {
                MoveDown(600);
            }
        }

        public override void Update()
        {
            if (Health <= 0)
            {
                Console.WriteLine("Player is dead!");
            }
        }

        public void MoveLeft()
        {
            if (Sprite.Left > 0)
            {
                Sprite.Left -= MoveSpeed;
                Facing = "left";
                Sprite.Image = Properties.Resources.left;
            }
        }

        public void MoveRight(int formWidth)
        {
            if (Sprite.Right < formWidth)
            {
                Sprite.Left += MoveSpeed;
                Facing = "right";
                Sprite.Image = Properties.Resources.right;
            }
        }

        public void MoveUp()
        {
            if (Sprite.Top > 45)
            {
                Sprite.Top -= MoveSpeed;
                Facing = "up";
                Sprite.Image = Properties.Resources.up;
            }
        }

        public void MoveDown(int formHeight)
        {
            if (Sprite.Bottom < formHeight)
            {
                Sprite.Top += MoveSpeed;
                Facing = "down";
                Sprite.Image = Properties.Resources.down;
            }
        }
        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
                Health = 0;
        }



        public void CollectAmmo(int amount)
        {
            Ammo += amount;
        }

        public bool CanShoot()
        {
            return Ammo > 0;
        }

        public void Shoot()
        {
            if (Ammo > 0)
                Ammo--;
        }

        public void HandleKeyDown(KeyEventArgs e, int formWidth, int formHeight)
        {
            if (e.KeyCode == Keys.Left) MoveLeft();
            if (e.KeyCode == Keys.Right) MoveRight(formWidth);
            if (e.KeyCode == Keys.Up) MoveUp();
            if (e.KeyCode == Keys.Down) MoveDown(formHeight);
        }

        public void HandleKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && CanShoot())
            {
                Shoot();
            }
        }
    }
}
