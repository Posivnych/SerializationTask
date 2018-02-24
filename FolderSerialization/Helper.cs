using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace FolderSerialization
{
    public static class Helper
    {
        public static void Unpack(string destination,
            (string[] directories, (string name, byte[] data)[] files) data)
        {
            foreach (var directory in data.directories)
            {
                Directory.CreateDirectory(Path.Combine(destination, directory));
            }

            foreach (var file in data.files)
            {
                File.WriteAllBytes(Path.Combine(destination, file.name), file.data);
            }
        }

        public static (string[] directories, (string name, byte[] data)[] files) Deserialize(string path)
        {
            var formatter = new BinaryFormatter();
            using (var stream = File.OpenRead(path))
            {
                return ((string[], (string, byte[])[]))formatter.Deserialize(stream);
            }
        }

        public static void Serialize(string pathFrom, string pathTo)
        {
            var pack = (PackFolders(pathFrom), PackFiles(pathFrom));
            var formatter = new BinaryFormatter();
            using (var stream = File.Create(pathTo))
            {
                formatter.Serialize(stream, pack);
            }
        }

        private static (string, byte[])[] PackFiles(string path)
        {
            var files = Directory
                .GetFiles(path, "*", SearchOption.AllDirectories);

            return files.Select(x => x.Replace($@"{path}\", "")).Zip
                (files.Select(x => File.ReadAllBytes(x)),
                (name, data) => (name, data)).ToArray();
        }

        private static string[] PackFolders(string path)
        {
            return Directory
                .GetDirectories(path, "*", SearchOption.AllDirectories)
                .Select(x => x.Replace($@"{path}\", ""))
                .ToArray();
        }
    }
}
