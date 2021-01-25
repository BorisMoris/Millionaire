﻿using System;
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
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Millionaire");
            }
        }
        private string defaultQSetsPath = @"..\..\DefaultQSets";

        public FileManager()
        {
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
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

        /// <summary>
        /// Loads all question sets from a program's folder
        /// </summary>
        /// <param name="exceptions">Out param., returns list of potential exceptions</param>
        /// <returns>List of question sets</returns>
        public List<QSet> LoadQuestionSets(out List<Exception> exceptions)
        {
            List<QSet> qSets = new List<QSet>();
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
        public QSet LoadQSetFromFile(string path)
        {
            QSet qSet = new QSet();
            int counter = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string s = sr.ReadLine();
                if (s == null) //checks wheter the first line is not null
                {
                    throw new ArgumentException(path + " First line is null, can't load");
                  //  return null;
                }
                qSet.Name = s.TrimEnd(';');

                sr.ReadLine();

                Difficulty difficulty = Difficulty.easy;
                s = sr.ReadLine();
                while(s != null)
                {
                    if (s.StartsWith("*")) //checks wheter higher difficulty section was reached
                    {
                        if (counter < 5) //checks wheter enough questions of previous difficulty was loaded
                        {
                            throw new ArgumentException(path + " Not enough questions of the same difficulty, can't load");
                        }

                        if (difficulty < Difficulty.hard) //checks wheter highest difficulty level was reached
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

    }
}
