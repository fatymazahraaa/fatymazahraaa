using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace Zombie_Shooter
{
    public partial class Form1 : Form
    {
        private Player player;
        private List<Zombie> zombies = new List<Zombie>();
        private List<Ammo> ammos = new List<Ammo>();
        private List<Health> healthItems = new List<Health>();
        private ScoreManager scoreManager = new ScoreManager();
        private Random rand = new Random();

        private Timer ammoSpawnTimer;
        private Timer zombieRespawnTimer;
        private Timer healthSpawnTimer;

        private int totalZombiesSpawned = 0;
        private int zombiesKilled = 0;
        private const int maxZombies = 10;
        private const int spawnBatchSize = 3;
        private Queue<bool> zombieSpawnQueue = new Queue<bool>();

        private GameStatus gameStatus = GameStatus.Playing;

        public Form1()
        {
            InitializeComponent();
            StartGame();
        }

        private void StartGame()

        {
          
            player = new Player
            {
                Health = 100,
                Ammo = 10,
 
            };

            Controls.Add(player.Sprite);
            player.Sprite.BringToFront();

            totalZombiesSpawned = 0;
            zombiesKilled = 0;
            zombies.Clear();
            ammos.Clear();
            healthItems.Clear();

            SpawnZombies(spawnBatchSize);
            SpawnHealthItem();

            scoreManager.ResetScore();
            UpdateUI();

            GameTimer.Start();
            gameStatus = GameStatus.Playing;

            ammoSpawnTimer = new Timer { Interval = 10000 };
            ammoSpawnTimer.Tick += AmmoSpawnTimerEvent;
            ammoSpawnTimer.Start();

            zombieRespawnTimer = new Timer { Interval = 2000 };
            zombieRespawnTimer.Tick += ZombieRespawnTimer_Tick;

            healthSpawnTimer = new Timer { Interval = 10000 };
            healthSpawnTimer.Tick += HealthSpawnTimer_Tick;
            healthSpawnTimer.Start();
        }

        private void SpawnZombies(int count)
        {
            int zombiesToSpawn = Math.Min(count, maxZombies - totalZombiesSpawned);
            for (int i = 0; i < zombiesToSpawn; i++)
            {
                Zombie zombie = Zombie.Spawn(this, player, rand);
                zombies.Add(zombie);
                totalZombiesSpawned++;
            }
        }

        private void AmmoSpawnTimerEvent(object sender, EventArgs e)
        {
            if (gameStatus != GameStatus.Playing) return;

            Ammo ammo = new Ammo();
            ammo.Spawn(this);
            ammos.Add(ammo);
        }

        private void ZombieRespawnTimer_Tick(object sender, EventArgs e)
        {
            zombieRespawnTimer.Stop();

            if (totalZombiesSpawned < maxZombies && zombieSpawnQueue.Count > 0)
            {
                zombieSpawnQueue.Dequeue();
                SpawnZombies(1);
            }

            if (zombieSpawnQueue.Count > 0 && totalZombiesSpawned < maxZombies)
            {
                zombieRespawnTimer.Start();
            }
        }

        private void HealthSpawnTimer_Tick(object sender, EventArgs e)
        {
            if (gameStatus != GameStatus.Playing) return;

            SpawnHealthItem();
        }

        private void SpawnHealthItem()
        {
            Health health = new Health(this);
            healthItems.Add(health);
        }

        private void UpdateZombies()
        {
            foreach (var zombie in zombies.ToArray())
            {
                zombie.Update();

                if (player.Sprite.Bounds.IntersectsWith(zombie.Sprite.Bounds))
                {
                    player.TakeDamage(1);
                }
            }
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (gameStatus != GameStatus.Playing) return;

            UpdateZombies();
            CheckItemCollisions();
            PreventOverlaps();
            UpdateUI();

            if (player.Status == PlayerStatus.Dead)
            {
                GameOver();
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameStatus != GameStatus.Playing) return;

            player.HandleKeyDown(e, this.ClientSize.Width, this.ClientSize.Height);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                TogglePause();
                return;
            }
 
            if (gameStatus != GameStatus.Playing) return;

            player.HandleKeyUp(e);

            if ((e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.LShiftKey || e.KeyCode == Keys.RShiftKey) && player.CanShoot())
            {
                ShootBullet(player.Facing);
            }
        }

        private void TogglePause()
        {
            if (gameStatus == GameStatus.Playing)
            {
                gameStatus = GameStatus.Paused;
                GameTimer.Stop();
               
            }
            else if (gameStatus == GameStatus.Paused)
            {
                gameStatus = GameStatus.Playing;
                GameTimer.Start();
               
            }
        }
       


        private void ShootBullet(string direction)
        {
            Point bulletPosition = new Point(player.Sprite.Left + player.Sprite.Width / 2, player.Sprite.Top + player.Sprite.Height / 2);

            Bullet bullet = new Bullet(zombies)
            {
                Direction = direction,
                BulletLeft = bulletPosition.X,
                BulletTop = bulletPosition.Y
            };

            bullet.MakeBullet(this);

            bullet.OnHitZombie += (zombieHit) =>
            {
                if (zombieHit != null)
                {
                    scoreManager.AddKill();

                    if (zombieHit.Sprite != null && zombies.Contains(zombieHit))
                    {
                        Controls.Remove(zombieHit.Sprite);
                        zombies.Remove(zombieHit);
                        zombiesKilled++;
                    }

                    bullet.DisposeBullet();

                    if (zombiesKilled >= maxZombies)
                    {
                        GameWin();
                        return;
                    }

                    if (totalZombiesSpawned < maxZombies)
                    {
                        zombieSpawnQueue.Enqueue(true);
                        if (!zombieRespawnTimer.Enabled)
                            zombieRespawnTimer.Start();
                    }
                }
            };

            player.Shoot();
            UpdateUI();
        }

        private void UpdateUI()
        {
            txtAmmo.Text = "Ammo: " + player.Ammo;
            txtScore.Text = scoreManager.GetScoreText();
            healthBar.Value = Math.Max(0, player.Health);
        }

        private void GameOver()
        {
            GameTimer.Stop();
            ammoSpawnTimer.Stop();
            zombieRespawnTimer.Stop();
            healthSpawnTimer.Stop();

            gameStatus = GameStatus.GameOver;

            player.Sprite.Image = Properties.Resources.dead;

            this.Hide();

            GameOver gameOverForm = new GameOver(this);
            gameOverForm.Show();
        }


        private void GameWin()
        {
            GameTimer.Stop();
            ammoSpawnTimer.Stop();
            zombieRespawnTimer.Stop();
            healthSpawnTimer.Stop();

            gameStatus = GameStatus.Won;

            Controls.Clear();

            PictureBox winPicture = new PictureBox
            {
                Image = Properties.Resources.win,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill
            };

            Controls.Add(winPicture);
            winPicture.BringToFront();
        }

        public  void RestartGame()
        {
            foreach (Control ctrl in Controls.OfType<PictureBox>().ToArray())
                Controls.Remove(ctrl);

            foreach (var zombie in zombies)
                Controls.Remove(zombie.Sprite);
            foreach (var ammo in ammos)
                Controls.Remove(ammo.Sprite);
            foreach (var health in healthItems)
                Controls.Remove(health.Sprite);

            zombies.Clear();
            ammos.Clear();
            healthItems.Clear();

            StartGame();
        }

        private void CheckItemCollisions()
        {
            foreach (var ammo in ammos.ToArray())
            {
                if (ammo.CheckCollision(player))
                {
                    player.Ammo += 5;
                    ammo.Destroy(this);
                    ammos.Remove(ammo);
                    break;
                }
            }

            foreach (var health in healthItems.ToArray())
            {
                if (health.CheckCollision(player))
                {
                    player.Health = Math.Min(player.Health + 20, 100);
                    health.Destroy(this);
                    healthItems.Remove(health);
                    break;
                }
            }
        }
        private void PreventOverlaps()
        {
            bool overlapsFound;

            do
            {
                overlapsFound = false;

                // Check zombies against ammo
                foreach (var zombie in zombies)
                {
                    foreach (var ammo in ammos)
                    {
                        if (zombie.Sprite.Bounds.IntersectsWith(ammo.Sprite.Bounds))
                        {
                            ammo.RespawnRandomly(this.ClientSize);
                            overlapsFound = true;
                        }
                    }
                }

                // Check zombies against health
                foreach (var zombie in zombies)
                {
                    foreach (var health in healthItems)
                    {
                        if (zombie.Sprite.Bounds.IntersectsWith(health.Sprite.Bounds))
                        {
                            health.RespawnRandomly(this.ClientSize);
                            overlapsFound = true;
                        }
                    }
                }

                // Check ammo against health
                foreach (var ammo in ammos)
                {
                    foreach (var health in healthItems)
                    {
                        if (ammo.Sprite.Bounds.IntersectsWith(health.Sprite.Bounds))
                        {
                            health.RespawnRandomly(this.ClientSize);
                            overlapsFound = true;
                        }
                    }
                }

                // avoid overlapping zombies
                for (int i = 0; i < zombies.Count; i++)
                {
                    for (int j = i + 1; j < zombies.Count; j++)
                    {
                        if (zombies[i].Sprite.Bounds.IntersectsWith(zombies[j].Sprite.Bounds))
                        {
                            zombies[j].RespawnRandomly(this.ClientSize);
                            overlapsFound = true;
                        }
                    }
                }

            } while (overlapsFound);  
        }

    }
}
