using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
        /// <param name="errors">Returns list of potential errors</param>
        /// <returns>List of question sets</returns>
        public static ObservableCollection<QSet> LoadQuestionSets(out List<string> errors)
        {
            ObservableCollection<QSet> qSets = new ObservableCollection<QSet>();

            QSet qSet = null;

            errors = new List<string>();
            
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
                    errors.Add(ex.Message);
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
                int line = 3;
                while(s != null)
                {
                    if (s.StartsWith("*")) //checks wheter higher difficulty section was reached
                    {                        
                        if (difficulty < Difficulty.Hard) //checks wheter highest difficulty level was reached
                        {
                            difficulty++;
                            s = sr.ReadLine();
                        }
                        else
                        {
                            throw new ArgumentException(path + " Špatný počet značek pro změnu obtížnosti, nelze načíst.");
                        }                        
                    }

                    string[] split = s.Split(';');
                    if (split.Length != 5) //checks wheter the line contains exactly 5 strings
                    {
                        throw new ArgumentException(path + $" Špatný počet řetězců na řádku {line}, nelze načíst.");
                    }
                    qSet.AddQuestion(difficulty, split[0], split[1], split[2], split[3], split[4]);
                    line++;

                    s = sr.ReadLine();
                }
            }

            if (qSet.EasyQuestions.Count() >= 5 && qSet.MediumQuestions.Count() >= 5 && qSet.HardQuestions.Count() >= 5)
            {
                return qSet;
            }
            else
            {
                throw new ArgumentException(path + " Nedostatek otázek stejné obtížnosti, nelze načíst.");
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

        /// <summary>
        /// Delete file containing question set
        /// </summary>
        /// <param name="path">Path to file to delete</param>
        public static void DeleteQSet(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            File.Delete(path);
        }

        /// <summary>
        /// Copy file from one destination to another
        /// </summary>
        /// <param name="initialPath">Current path to file</param>
        /// <param name="targetPath">Path to new file</param>
        public static void ExportQSet(string initialPath, string targetPath)
        {
            File.Copy(initialPath, targetPath);
        }

        /// <summary>
        /// Import file with question set
        /// </summary>
        /// <param name="initialPath">Path to imported file</param>
        /// <returns>Path to copied file</returns>
        public static string ImportQSet(string initialPath, string finalPath)
        {
            File.Copy(initialPath, finalPath);
            return finalPath;
        }

        /// <summary>
        /// Save given qSet to destination in his Path property
        /// </summary>
        /// <param name="qSet"></param>
        public static void SaveQSet(QSet qSet)
        {
            if (qSet.Path == null)
            {
                qSet.Path = GenerateFilePath(qSet.Name);
            }

            using (StreamWriter writer = new StreamWriter(qSet.Path, false, Encoding.UTF8))
            {
                writer.WriteLine(qSet.Name);
                writer.WriteLine("*easyQuestions");
                List<Question> questions = qSet.EasyQuestions;
                string[] parts = new string[5];
                Difficulty difficulty = Difficulty.Easy;
                bool loop = true;
                while (loop)
                {
                    foreach (Question question in questions)
                    {
                        parts[0] = question.QuestionSentence;
                        parts[1] = question.RightAnswer;
                        parts[2] = question.WrongAnswer1;
                        parts[3] = question.WrongAnswer2;
                        parts[4] = question.WrongAnswer3;

                        writer.WriteLine(string.Join(";", parts));
                    }

                    difficulty++;
                    if (difficulty == Difficulty.Medium)
                    {
                        writer.WriteLine("*mediumQuestions");
                        questions = qSet.MediumQuestions;
                    }
                    else if (difficulty == Difficulty.Hard)
                    {
                        writer.WriteLine("*hardQuestions");
                        questions = qSet.HardQuestions;
                    }
                    else
                    {
                        loop = false;
                    }
                }
            }            
        }

        /// <summary>
        /// Generate path from given name of question set 
        /// </summary>
        /// <remarks>
        /// Source of diacritics removing algorythm: http://archives.miloush.net/michkap/archive/2007/05/14/2629747.html
        /// </remarks>
        /// <param name="qSetName"></param>
        /// <returns>Path</returns>
        public static string GenerateFilePath (string qSetName)
        {
            if (ContainsInvalidChars(qSetName))
            {
                throw new Exception("Název sady obsahuje nepovolené znaky");
            }

            qSetName = qSetName.ToLower();
            string normalized = qSetName.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            stringBuilder.Replace(' ', '_');
            stringBuilder.Append(".csv");
            
            return Path.Combine(dataDir, stringBuilder.ToString());
        }

        /// <summary>
        /// Checks if given string contains characters invalid for file name
        /// </summary>
        /// <param name="str"></param>
        /// <returns>True if string contains invalid characters, otherwise false</returns>
        public static bool ContainsInvalidChars(string str)
        {
            foreach(char c in str)
            {
                foreach (char h in Path.GetInvalidFileNameChars())
                {
                    if (c == h)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
