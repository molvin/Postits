using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PostIts
{
    public static class SaveManager
    {
        private static readonly string RootPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Postits";

        public static void Save(int id, string rtf, int x, int y, int width, int height)
        {
            SaveData data = new SaveData { Id = id, RichText = rtf, X = x, Y = y, Width = width, Height = height };
            string json = JsonConvert.SerializeObject(data);

            string path = Path.Combine(RootPath, $"Postit{id}.json");

            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);

            if (!File.Exists(path))
            {
                FileStream file = File.Create(path);
                file.Close();
            }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
        }
        public static List<SaveData> LoadFiles()
        {
            if (!Directory.Exists(RootPath))
                return null;

            List<SaveData> files = new List<SaveData>();

            string[] paths = Directory.GetFiles(RootPath);

            //TODO: add safety if other files exsists in directory
            foreach(string path in paths)
            {
                using(StreamReader reader = new StreamReader(path))
                {
                    string json = reader.ReadToEnd();
                    SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
                    files.Add(data);
                }
            }
            return files;
        }

        //TODO: save open state
        [Serializable]
        public struct SaveData
        {
            public int Id;
            public string RichText;
            public int X;
            public int Y;
            public int Width;
            public int Height;
        }
    }
}
