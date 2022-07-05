using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UGF.RuntimeTools.Runtime.Scenes
{
    public static class SceneExtensions
    {
        public static T GetComponent<T>(this Scene scene) where T : class
        {
            return (T)(object)GetComponent(scene, typeof(T));
        }

        public static Component GetComponent(this Scene scene, Type type)
        {
            return TryGetComponent(scene, type, out Component component) ? component : throw new ArgumentException($"Component not found in scene by the specified type: '{type}'.");
        }

        public static bool TryGetComponent<T>(this Scene scene, out T component) where T : class
        {
            if (TryGetComponent(scene, typeof(T), out Component value))
            {
                component = (T)(object)value;
                return true;
            }

            component = default;
            return false;
        }

        public static bool TryGetComponent(this Scene scene, Type type, out Component component)
        {
            if (!scene.IsValid()) throw new ArgumentException("Value should be valid.", nameof(scene));
            if (type == null) throw new ArgumentNullException(nameof(type));

            GameObject[] gameObjects = scene.GetRootGameObjects();

            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject gameObject = gameObjects[i];

                if (gameObject.TryGetComponent(type, out component))
                {
                    return true;
                }
            }

            component = default;
            return false;
        }
    }
}
