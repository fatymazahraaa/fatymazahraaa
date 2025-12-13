using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System;
using Zombie_Shooter;

public class Bullet
{
    public string Direction { get; set; }
    public int BulletLeft { get; set; }
    public int BulletTop { get; set; }

    private int speed = 20;
    private PictureBox bullet;
    private Timer bulletTimer;
    public event Action<Zombie> OnHitZombie;
    private List<Zombie> zombies;

    public Bullet(List<Zombie> zombies)
    {
        this.zombies = zombies ?? new List<Zombie>();
    }

    public void MakeBullet(Form form)
    {
        bullet = new PictureBox
        {
            BackColor = Color.White,
            Size = new Size(5, 5),
            Tag = "bullet",
            Left = BulletLeft,
            Top = BulletTop
        };
        bullet.BringToFront();
        form.Controls.Add(bullet);

        bulletTimer = new Timer
        {
            Interval = speed
        };
        bulletTimer.Tick += BulletTimerEvent;
        bulletTimer.Start();
    }

    private void BulletTimerEvent(object sender, EventArgs e)
    { 
        if (bullet == null)
            return;
 
        if (Direction == "left")
        {
            bullet.Left -= speed;
        }
        else if (Direction == "right")
        {
            bullet.Left += speed;
        }
        else if (Direction == "up")
        {
            bullet.Top -= speed;
        }
        else if (Direction == "down")
        {
            bullet.Top += speed;
        }

        CheckCollision();

       
    }
 
    public void CheckCollision()
    {
        if (bullet == null) return;  

      
        foreach (var zombie in zombies.ToArray())   
        {
            if (bullet != null && bullet.Bounds.IntersectsWith(zombie.Sprite.Bounds))
            {
                OnHitZombie?.Invoke(zombie); 
                DisposeBullet();  
                break;   
            }
        }
    }

    public void DisposeBullet()
    { 
        if (bullet == null) return;

        bulletTimer.Stop();
        bulletTimer.Dispose();
        bullet.Dispose();
        bulletTimer = null;
        bullet = null;   
    }
}
