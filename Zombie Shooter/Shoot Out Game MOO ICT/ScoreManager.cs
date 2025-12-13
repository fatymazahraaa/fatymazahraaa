namespace Zombie_Shooter
{
    public class ScoreManager
    {
        public int Score { get; private set; } = 0;

        public void AddKill()
        {
            Score += 1;
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public string GetScoreText()
        {
            return "Kills: " + Score;
        }
    }
}
