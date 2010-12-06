namespace Clinky
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Text.RegularExpressions;

    public sealed class MyIO
    {

        private MyIO()
        {
        }

        public static T Clone<T>(T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, obj);
            serializationStream.Position = 0;
            obj = (T) formatter.Deserialize(serializationStream);
            serializationStream.Close();
            return obj;
        }

        public static bool Exists(string path)
        {
            return (File.Exists(path) || Directory.Exists(path));
        }

        public static string GetUniqueName(string path)
        {
            if (Exists(path))
            {
                int num;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
                Match match = (new Regex(@"(.*)_([0-9]+)\s*$")).Match(fileNameWithoutExtension);
                if (match.Success)
                {
                    fileNameWithoutExtension = match.Groups[1].Value;
                    num = int.Parse(match.Groups[2].Value) + 1;
                }
                else
                {
                    num = 2;
                }
                fileNameWithoutExtension = new StringBuilder().Append(fileNameWithoutExtension).Append("_").Append(num).ToString();
                path = Path.Combine(Path.GetDirectoryName(path), fileNameWithoutExtension + Path.GetExtension(path));
                if (Exists(path))
                {
                    path = GetUniqueName(path);
                }
            }
            return path;
        }

        public static bool IsSameFile(string path1, string path2)
        {
            path1 = path1.Replace("/", @"\");
            path2 = path2.Replace("/", @"\");
            return (path1.ToLower() == path2.ToLower());
        }
    }
}

