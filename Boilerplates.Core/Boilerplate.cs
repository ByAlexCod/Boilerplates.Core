using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Boilerplates.Core
{
    public class Boilerplate
    {
        private readonly string _projectPath;
        private List<string> spliters = new List<string>() { " ", "(", ")", ",", ";", "{", "}", "\n", "\r", "." };

        public Boilerplate(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                throw new FileNotFoundException("Cannot find the folder.", projectPath);
            }

            _projectPath = Path.GetFullPath(projectPath);
        }

        /// <summary>
        /// Find all bpl variables in project files
        /// </summary>
        /// <param name="projectPath"></param>
        public List<string> GetBPLPlaceholders(string projectPath = null, bool rootAnnalyze = true)
        {
            List<string> currentRootVariables = new List<string>();

            if (projectPath == null && rootAnnalyze)
            {
                projectPath = _projectPath;
            }

            if (projectPath != null)
            {
                foreach (var filePath in Directory.GetFiles(projectPath))
                {
                    if(Path.GetFileName(filePath).ToLower().Contains("bpl")) currentRootVariables.Add(Path.GetFileName(filePath));
                    currentRootVariables.AddRange(GetFilesBPPlaceholders(filePath));
                }

                foreach (var directoryPath in Directory.GetDirectories(projectPath))
                {
                    if (Path.GetFileName(directoryPath).ToLower().Contains("bpl")) currentRootVariables.Add(Path.GetFileName(directoryPath));
                    currentRootVariables.AddRange(GetBPLPlaceholders(directoryPath, false));
                }

                
            }

            return currentRootVariables;
        }

        private List<string> split(List<string> text, int decomposerIndex)
        {
            if (decomposerIndex < spliters.Count)
            {
                List<string> currentSplitted = new List<string>();

                foreach (var splitted in text)
                {
                    currentSplitted.AddRange(splitted.Split(spliters[decomposerIndex]));
                }

                return split(currentSplitted, decomposerIndex + 1);
            }
            return text;
        }

        private List<string> GetFilesBPPlaceholders(string filePath)
        {
            string fileText = File.ReadAllText(filePath);
            List<string> decomposed = split(new List<string>() {fileText}, 0);
            HashSet<string> variableNoBpl = new HashSet<string>();
            foreach (var variable in decomposed)
            {
                if (variable.ToLower().Contains("bpl"))
                {
                    variableNoBpl.Add(variable);
                }
            }

            return variableNoBpl.ToList();
        }

        public string StringReplaceOneOfList(string s, Dictionary<string, string> l)
        {
            string final = s;
            foreach (var bp in l)
            {
                final = final.Replace(bp.Key, bp.Value);
            }

            return final;
        }

        public void Boil (string outputPath, Dictionary<string, string> bplReplacements, string currentRootPath = null)
        {
            //if (!Directory.Exists(outputPath)) Directory.CreateDirectory(outputPath);
            if (currentRootPath == null)
            {
                currentRootPath = _projectPath;
            }
            foreach (var filePath in Directory.GetFiles(currentRootPath))
            {

                string newFilePath = StringReplaceOneOfList(Path.GetFileName(filePath), bplReplacements);

                newFilePath = Path.Combine(outputPath, newFilePath);


                string newContent = StringReplaceOneOfList( File.ReadAllText(filePath), bplReplacements );
                File.Create(newFilePath).Close();
                File.WriteAllText(newFilePath, newContent);
            }

            foreach (var directoryPath in Directory.GetDirectories(currentRootPath))
            {
                string newFolderPath = StringReplaceOneOfList(Path.GetFileName(directoryPath), bplReplacements);
                newFolderPath = Path.Combine(outputPath, newFolderPath);
                Directory.CreateDirectory(newFolderPath);
                Boil(newFolderPath, bplReplacements, directoryPath);
            }
        }

        //private string GetVariableWithoutBPL(string baseVariable)
        //{
        //    return baseVariable
        //        .Replace("_bpl", "")
        //        .Replace("bpl", "")
        //        .Replace("Bpl", "")
        //        .Replace("_BPL", "")
        //        .Replace("BPL", "");
        //}
    }
}
