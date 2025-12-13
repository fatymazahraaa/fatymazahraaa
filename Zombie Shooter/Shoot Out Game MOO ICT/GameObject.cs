using System.Windows.Forms;
namespace Zombie_Shooter
{
    public abstract class GameObject
    {
        public PictureBox Sprite { get; set; }
        public int Speed { get; set; }
         
        public GameObject()
        {
            Sprite = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.AutoSize
            };
        }
        public abstract void Move(); 
        public abstract void Update();  
 
        public void AddToForm(Form form)
        {
            form.Controls.Add(Sprite);
            Sprite.BringToFront();
        }
 
        public void RemoveFromForm(Form form)
        {
            form.Controls.Remove(Sprite);
            Sprite.Dispose();
        }
    }
}
