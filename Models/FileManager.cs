using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Millionaire.Models
{
    class FileManager
    {
        private string dataDir
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
        private string defaultQSetsPath = @"..\..\DefaultQSets";

        public FileManager()
        {         
        }

        /// <summary>
        /// Copy all default question sets to program's folder
        /// </summary>
        public void CopyDefault()
        {
            
            DirectoryInfo defaultDirInfo = new DirectoryInfo(defaultQSetsPath);
            FileInfo[] defaultFiles = defaultDirInfo.GetFiles("*.csv");

            DirectoryInfo dataDirInfo = new DirectoryInfo(dataDir);
            FileInfo[] dataFiles = dataDirInfo.GetFiles("*.csv");
            
            List<string> dataFileNames = new List<string>(); //generate list of names of files in program's folder
            foreach (FileInfo file in dataFiles)
            {
                dataFileNames.Add(file.Name);
            }

            foreach (FileInfo defaultFile in defaultFiles) //comparing files by names
            {
                if (!dataFileNames.Contains(defaultFile.Name))
                {
                    File.Copy(defaultFile.FullName, Path.Combine(dataDir, defaultFile.Name));
                }
            }
        }

        public List<QSet> LoadQuestionSets()
        {
            List<QSet> qSets = new List<QSet>();
            QSet qSet;
            
            string[] files = Directory.GetFiles(dataDir, "*.csv");
            foreach (string file in files)
            {
                qSet = LoadQSetFromFile(file);
                if(qSet != null)
                {
                    qSets.Add(qSet);
                }                                
            }

            Debug.WriteLine(qSets.Count());

            return qSets;
        }

        public QSet LoadQSetFromFile(string path)
        {
            QSet qSet = new QSet();

            int counter = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string s = sr.ReadLine();
                if (s == null)
                {
                    return null;
                }

                qSet.Name = s.TrimEnd(';');

                sr.ReadLine();

                s = sr.ReadLine();
                while (s != null && !s.StartsWith("*"))
                {
                    string[] split = s.Split(';');
                    if (split.Length != 5)
                    {
                        return null;
                    }
                    qSet.AddEasyQ(split[0], split[1], split[2], split[3], split[4]);
                    counter++;

                    s = sr.ReadLine();
                }

                if (counter < 5)
                {
                    return null;
                }
                counter = 0;

                s = sr.ReadLine();
                while (s != null && !s.StartsWith("*"))
                {
                    string[] split = s.Split(';');
                    if (split.Length != 5)
                    {
                        return null;
                    }
                    qSet.AddMediumQ(split[0], split[1], split[2], split[3], split[4]);
                    counter++;

                    s = sr.ReadLine();
                }

                if (counter < 5)
                {
                    return null;
                }
                counter = 0;

                s = sr.ReadLine();
                while (s != null && !s.StartsWith("*"))
                {
                    string[] split = s.Split(';');
                    if (split.Length != 5)
                    {
                        return null;
                    }
                    qSet.AddHardQ(split[0], split[1], split[2], split[3], split[4]);
                    counter++;

                    s = sr.ReadLine();
                }

                if (counter < 5)
                {
                    return null;
                }               

            }
            Debug.WriteLine("vrátil jsem hodnoty");
            return qSet;
        }

    }
}
