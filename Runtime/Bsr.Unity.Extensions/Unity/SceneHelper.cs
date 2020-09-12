using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bsr.Unity.Extensions.Unity
{
    public static class SceneHelper
    {
        public static bool IsName(this Scene scene, string name)
        {
            return scene.name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsActiveScene(string name)
        {
            var scene = SceneManager.GetActiveScene();
            return scene.name.Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsSceneLoaded(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
                throw new ArgumentException($"{nameof(sceneName)} can not be empty");

            switch (SceneManager.sceneCount)
            {
                case 0:
                    return false;
                case 1:
                    return SceneManager.GetActiveScene().name.Equals(sceneName, StringComparison.OrdinalIgnoreCase);
                default:
                {
                    for (var i = 0; i < SceneManager.sceneCount; i++)
                    {
                        if (SceneManager.GetSceneAt(i).name.Equals(sceneName, StringComparison.OrdinalIgnoreCase))
                            return true;
                    }
                }
                    return false;
            }
        }

        public static T[] FindObjectsOfTypeInAllOpenedScenes<T>() where T : Component
        {
            var objects = new List<T>();
            var scenes = GetOpenedScenes();
            for (var i = 0; i < scenes.Length; i++)
            {
                objects.AddRange(scenes[i].FindObjectsOfType<T>());
            }

            return objects.ToArray();
        }

        public static Scene[] GetOpenedScenes()
        {
            var count = SceneManager.sceneCount;
            var scenes = new List<Scene>(count);

            for (var i = 0; i < count; i++)
            {
                var scene = SceneManager.GetSceneAt(i);

                if (scene.isLoaded)
                    scenes.Add(scene);
            }

            return scenes.ToArray();
        }

        public static T[] FindObjectsOfType<T>(this Scene scene) where T : Component
        {
            var activeScene = SceneManager.GetActiveScene();
            var sameActive = activeScene == scene;
            if (!sameActive)
            {
                if (!SceneManager.SetActiveScene(scene))
                    throw new UnityException($"Failed to set active scene {scene.name}");
            }

            var objects = Resources.FindObjectsOfTypeAll<T>();

            if (!sameActive && !SceneManager.SetActiveScene(activeScene))
                throw new UnityException($"Failed to set active scene back to {activeScene.name}");

            return objects;
        }

        public static IEnumerable<T> FindObjectsOfTypeInScene<T>(string sceneName) where T : Component
        {
            var activeScene = SceneManager.GetActiveScene();
            var sameActive = activeScene.name.Equals(sceneName, StringComparison.Ordinal);
            if (!sameActive)
            {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (!SceneManager.SetActiveScene(scene))
                    throw new UnityException($"Failed to set active scene {sceneName}");
            }

            var objects = Resources.FindObjectsOfTypeAll<T>();

            if (!sameActive && !SceneManager.SetActiveScene(activeScene))
                throw new UnityException($"Failed to set active scene back to {activeScene.name}");

            return objects;
        }
    }
}