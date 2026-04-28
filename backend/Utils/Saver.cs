using System.Text.Json;

namespace trial.utils
{
    public class Saver
    {
        private string filename;

        public Saver(string filename)
        {
            this.filename = filename;
        }

        public void Save(string jsonData)
        {
            File.WriteAllText(this.filename, jsonData);
            Console.WriteLine("Saved data to file");
        }

        public string Load()
        {
            try{
                string jsonData = File.ReadAllText(this.filename);
                return jsonData;
            } catch(System.IO.FileNotFoundException)
            {
                Console.WriteLine("Cannot open file!");
            }
            return "";
        }
    }
}