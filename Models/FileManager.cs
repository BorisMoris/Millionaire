using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Millionaire.Models
{
    static class FileManager
    {
        private static string dataDir
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Millionaire");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }

        private static string scoresFile = "scores.xml";
        public static bool safeToSaveScores = true;

        /// <summary>
        /// Loads all question sets from a program's folder
        /// </summary>
        /// <param name="exceptions">Returns list of potential exceptions</param>
        /// <returns>List of question sets</returns>
        public static ObservableCollection<QSet> LoadQuestionSets(out List<Exception> exceptions)
        {
            ObservableCollection<QSet> qSets = new ObservableCollection<QSet>();

            QSet qSet = null;

            exceptions = new List<Exception>();
            
            string[] files = Directory.GetFiles(dataDir, "*.csv");
            foreach (string file in files)
            {
                try
                {
                    qSet = LoadQSetFromFile(file);
                    if (qSet != null)
                    {
                        qSets.Add(qSet);
                    }
                }
                catch(Exception ex)
                {
                    exceptions.Add(ex);
                }                           
            }
            
            return qSets;
        }


        /// <summary>
        /// Loads one question set from a file
        /// </summary>
        /// <param name="path">File location</param>
        /// <returns>Question set or null if file's content is invalid</returns>
        public static QSet LoadQSetFromFile(string path)
        {
            QSet qSet = new QSet();
            qSet.Path = path;

            int counter = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string s = sr.ReadLine();
                if (s == null) //checks wheter the first line is not null
                {
                    throw new ArgumentException(path + " First line is null, can't load");                  
                }
                qSet.Name = s.TrimEnd(';');

                sr.ReadLine();

                Difficulty difficulty = Difficulty.Easy;
                s = sr.ReadLine();
                while(s != null)
                {
                    if (s.StartsWith("*")) //checks wheter higher difficulty section was reached
                    {
                        if (counter < 5) //checks wheter enough questions of previous difficulty was loaded
                        {
                            throw new ArgumentException(path + " Not enough questions of the same difficulty, can't load");
                        }

                        if (difficulty < Difficulty.Hard) //checks wheter highest difficulty level was reached
                        {
                            difficulty++;
                            s = sr.ReadLine();
                        }
                        else
                        {
                            throw new ArgumentException(path + " Invalid number of difficulty signs, can't load");
                        }                        
                    }

                    string[] split = s.Split(';');
                    if (split.Length != 5) //checks wheter the line contains exactly 5 strings
                    {
                        throw new ArgumentException(path + " Wrong count of strings on a line, can't load");
                    }
                    qSet.AddQuestion(difficulty, split[0], split[1], split[2], split[3], split[4]);
                    counter++;

                    s = sr.ReadLine();
                }
            }

            if (qSet.EasyQuestions.Count() >= 5 && qSet.MediumQuestions.Count() >= 5 && qSet.HardQuestions.Count() >= 5)
            {
                return qSet;
            }
            else
            {
                throw new ArgumentException(path + " Not enough questions of the same difficulty, can't load");
            }            
        }

        /// <summary>
        /// Save list of scores to scores.xml
        /// </summary>
        /// <param name="scores"></param>
        public static void SaveScores(List<Score> scores)
        {  
            XmlSerializer serializer = new XmlSerializer(scores.GetType());

            using(StreamWriter sw=new StreamWriter(Path.Combine(dataDir, scoresFile)))
            {
                if (!safeToSaveScores)
                {
                    throw new Exception("Kvůli dřívější nepřístupnosti souboru není bezpečné ukládat skóre. Restartujte aplikaci.");
                }

                serializer.Serialize(sw, scores);
            }
        }

        /// <summary>
        /// Load list of scores from scores.xml
        /// </summary>
        /// <returns></returns>
        public static List<Score> LoadScores()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
            if (File.Exists(Path.Combine(dataDir, scoresFile)))
            {
                using(StreamReader sr=new StreamReader(Path.Combine(dataDir, scoresFile)))
                {
                    return (List<Score>)serializer.Deserialize(sr);
                }
            }
            else
            {
                return new List<Score>();
            }
        }

        public static void DeleteQSet(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }                
        }

    }
}
