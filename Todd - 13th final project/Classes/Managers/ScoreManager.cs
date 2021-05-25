using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Todd.Moduls;

namespace Todd.Managers
{
    public class ScoreManager
    {
        //file in bin that the data would saved to
        private static string scoresFileName = "scores.xml";

        public List<Score>  highscore { get; private set; }
        public List<Score>  scores { get; private set; }

        #region constructors
        public ScoreManager():this(new List<Score>()){}//empty constractor that wntwe new list to the constractor
        public ScoreManager(List<Score> scores_)
        {
            scores = scores_;
        }
        #endregion


        #region handle highscore 
        public void updateHighScore(string criterion)
        {
            if (criterion == "date")
                scores = scores.OrderBy(c => c.date).ToList();
            else if (criterion == "gold coins")
                scores = scores.OrderBy(c => c.coinsAndHearts.goldCoins).ToList();
            else if (criterion == "silver coins")
                scores = scores.OrderBy(c => c.coinsAndHearts.silverCoins).ToList();
            else if (criterion == "bronze coins")
                scores = scores.OrderBy(c => c.coinsAndHearts.bronzeCoins).ToList();
            else if (criterion == "hearth")
                scores = scores.OrderBy(c => c.coinsAndHearts.hearts).ToList();
            else if (criterion == "time")
                scores = scores.OrderBy(c => c.timeInSeconds).ToList();

            highscore = scores.Take(3).ToList();
        }//in all levels
        public void updateHighScore(string criterion,string level)
        {
            if (level == "All")
            {
                updateHighScore(criterion);
                return;
            }
            List<Score> scoresInLevel = new List<Score>();
            foreach (Score sc in scores)
                if (sc.level == level)
                    scoresInLevel.Add(sc);

            if (level == "Survival")
            {
                if (criterion == "date")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.date).ToList();
                else if (criterion == "gold coins")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.coinsAndHearts.goldCoins).ToList();
                else if (criterion == "silver coins")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.coinsAndHearts.silverCoins).ToList();
                else if (criterion == "bronze coins")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.coinsAndHearts.bronzeCoins).ToList();
                else if (criterion == "hearth")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.coinsAndHearts.hearts).ToList();
                else if (criterion == "time")
                    scoresInLevel = scoresInLevel.OrderByDescending(c => c.timeInSeconds).ToList();
                return;
            }

            if (criterion == "date")
                scoresInLevel = scoresInLevel.OrderBy(c => c.date).ToList();
            else if (criterion == "gold coins")
                scoresInLevel = scoresInLevel.OrderBy(c => c.coinsAndHearts.goldCoins).ToList();
            else if (criterion == "silver coins")
                scoresInLevel = scoresInLevel.OrderBy(c => c.coinsAndHearts.silverCoins).ToList();
            else if (criterion == "bronze coins")
                scoresInLevel = scoresInLevel.OrderBy(c => c.coinsAndHearts.bronzeCoins).ToList();
            else if (criterion == "hearth")
                scoresInLevel = scoresInLevel.OrderBy(c => c.coinsAndHearts.hearts).ToList();
            else if (criterion == "time")
                scoresInLevel = scoresInLevel.OrderBy(c => c.timeInSeconds).ToList();

            highscore = scoresInLevel.Take(3).ToList();
        }//in certain level
        #endregion


        #region load and save 

        public static ScoreManager Load()
        {
            if (!File.Exists(scoresFileName))
                return new ScoreManager();
            using (var reader = new StreamReader(new FileStream(scoresFileName, FileMode.Open)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));
                var scores_ = (List<Score>)serilizer.Deserialize(reader);
                return new ScoreManager(scores_);
            }

        }//return the scores from the xml file

        public static void Save(ScoreManager scoreManager)
        {
            using (var writer = new StreamWriter(new FileStream(scoresFileName, FileMode.Create)))
            {
                var serilizer = new XmlSerializer(typeof(List<Score>));
                serilizer.Serialize(writer, scoreManager.scores);
            }

        }//save the list of scores to the xml file

        #endregion
    }
}
