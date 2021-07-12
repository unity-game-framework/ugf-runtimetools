using System;
using System.IO;

namespace UGF.RuntimeTools.Runtime.Storage
{
    public static class StorageUtility
    {
        public static string GetPath(StoragePathType type, string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            string storagePath = GetPath(type);

            path = Path.Combine(storagePath, path);

            return path;
        }

        public static string GetPath(StoragePathType type)
        {
            switch (type)
            {
                case StoragePathType.Data: return UnityEngine.Application.dataPath;
                case StoragePathType.StreamingAssets: return UnityEngine.Application.streamingAssetsPath;
                case StoragePathType.PersistentData: return UnityEngine.Application.persistentDataPath;
                case StoragePathType.TemporaryCache: return UnityEngine.Application.temporaryCachePath;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
