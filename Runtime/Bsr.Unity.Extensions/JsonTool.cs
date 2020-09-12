using System.IO;
using System.Text;
using UnityEngine;

namespace Bsr.Unity.Extensions
{
    public static class JsonTool
    {
        public static T FromFile<T>(string path)
        {
            return !File.Exists(path) ? default : JsonUtility.FromJson<T>( File.ReadAllText(path));
        }
        
        public static void ToFile(object obj, string path)
        {
            var json = JsonUtility.ToJson(obj, true);
            File.AppendAllText(path, json, Encoding.UTF8);
        }
    }
}