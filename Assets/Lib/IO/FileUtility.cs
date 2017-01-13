using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace UnityLib.IO
{
    public static class FileUtility
    {
        public static void TryWrite(string path, string s)
        {
            var content = new UTF8Encoding(true).GetBytes(s);
            TryWrite(path, content);
        }

        public static void TryWrite(string path, byte[] bytes)
        {
            try
            {
                Write(path, bytes);
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
        }

        public static void Write(string path, byte[] bytes)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (var s = File.Create(path))
            {
                s.Write(bytes, 0, bytes.Length);
            }
        }

        public static string TryReadString(string path)
        {
            var bytes = TryRead(path);
            if (bytes == null)
                return null;

            UTF8Encoding temp = new UTF8Encoding(true);
            return temp.GetString(bytes);
        }

        public static byte[] TryRead(string path)
        {
            try
            {
                return Read(path);
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }

            return null;
        }

        public static byte[] Read(string path)
        {
            var length = (int)new System.IO.FileInfo(path).Length;

            using (var fs = File.OpenRead(path))
            {
                byte[] bytes = new byte[length];

                fs.Read(bytes, 0, length);
                return bytes;
            }
        }

        public static string[] GetFileList(string directoryPath)
        {
            return Directory.GetFiles(directoryPath);
        }

        public static List<string> GetFileListRecursively(string directoryPath, List<string> files = null)
        {
            if (files == null)
                files = new List<string>();

            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(directoryPath);
            files.AddRange(fileEntries);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(directoryPath);
            foreach (string subdirectory in subdirectoryEntries)
                GetFileListRecursively(subdirectory, files);

            return files;
        }

        public static List<string> GetPathWithExtension(IEnumerable<string> paths, string extension)
        {
            var matches = new List<string>();
            foreach (var path in paths)
                if (Path.GetExtension(path) == extension)
                    matches.Add(path);

            return matches;
        }

        public static List<string> GetIntFileName(IEnumerable<string> paths)
        {
            var matches = new List<string>();
            int i = 0;
            foreach (var path in paths)
                if (int.TryParse(Path.GetFileNameWithoutExtension(path), out i))
                    matches.Add(path);

            return matches;
        }

        public static List<Texture2D> GetAllJPG(string path)
        {
            var jpgs = new List<Texture2D>();
            var files = GetFileList(path);
            foreach (var filePath in files)
                if (Path.GetExtension(filePath) == ".jpg")
                {
                    var texture = ReadJPG(filePath);
                    if (texture != null)
                        jpgs.Add(texture);
                }

            return jpgs;
        }


        public static Texture2D ReadJPG(string path)
        {
            var data = TryRead(path);
            if (data != null)
            {
                var texture = new Texture2D(1, 1);
                texture.LoadImage(data);
                return texture;
            }
            return null;
        }

        public static void WriteJPG(string path, Texture2D texture)
        {
            TryWrite(path, texture.EncodeToJPG());
        }
    }
}