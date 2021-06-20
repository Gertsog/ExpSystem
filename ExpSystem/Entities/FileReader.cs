using System.Collections.Generic;
using System.Linq;

namespace ExpSystem
{
    public static class FileReader
    {
        /// <summary>
        /// Считывание информации до пустой строки
        /// </summary>
        public static List<string> ReadFile(System.IO.StreamReader stream)
        {
            string line;
            List<string> lines = new List<string>();
            while (!string.IsNullOrEmpty(line = stream.ReadLine()))
            {
                lines.Add(line);
            }
            lines.Remove(lines.First());
            return lines;
        }
    }
}
