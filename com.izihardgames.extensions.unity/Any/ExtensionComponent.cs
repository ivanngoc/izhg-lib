using IziHardGames.Buffers;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UnityEngine
{

    public static class IterateComponents
    {
        /// <summary>
        /// Foreach component can be cated to T. For Active AndInactive Gameobjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static int Foreach<T>(Scene scene, Action<T> action)
        {
            var roots = scene.GetRootGameObjects();
            int count = default;
            for (int i = 0; i < roots.Length; i++)
            {
                count += Foreach(roots[i], action);
            }
            return count;
        }
        public static int Foreach<T>(GameObject gameObject, Action<T> action)
        {
            var components = gameObject.GetComponents<T>();
            int count = 0;

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is T item)
                {
                    action(item);
                    count++;
                }
            }
            int childs = gameObject.transform.childCount;
            for (int i = 0; i < childs; i++)
            {
                var childTransform = gameObject.transform.GetChild(i);
                count += Foreach(childTransform.gameObject, action);
            }
            return count;
        }
    }

    /// <summary>
    /// Find
    /// </summary>
    public static class ExtensionComponent
    {
        /// <summary>
        /// Active <see cref="GameObject"/> only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T FindComponentActiveOfTypeAtScene<T>(this Component self) where T : Component
        {
            T[] finds = Object.FindObjectsOfType<T>();

            for (int i = 0; i < finds.Length; i++)
            {
                if (finds[i].gameObject.scene == self.gameObject.scene)
                {
                    return finds[i] as T;
                }
            }
            return default;
        }

        /// <summary>
        /// Active <see cref="GameObject"/> only
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="toAdd"></param>
        /// <returns></returns>
        public static bool FindActiveObjectsOfTypeAtScene<T>(this Component self, List<T> toAdd) where T : Component
        {
            T[] finds = Object.FindObjectsOfType<T>();

            bool isMatched = false;

            for (int i = 0; i < finds.Length; i++)
            {
                if (finds[i].gameObject.scene == self.gameObject.scene)
                {
                    toAdd.Add(finds[i]);

                    isMatched = true;
                }
            }
            return isMatched;
        }


        public static bool TryGetComponentTillRoot<T>(this Component component, out T result) where T : Component
        {
            if (component.TryGetComponent<T>(out result))
            {
                return true;
            }
            else
            {
                if (TryGetComponentInParent(component, out result))
                {
                    return true;
                }
            }
            result = default;

            return false;
        }
        /// <summary>
        /// Не ищет сначала у своего гейобжекта а сразу начинает от парента
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetComponentInParent<T>(this Component component, out T result) where T : Component
        {
            Transform parent = component.transform.parent;

            while (parent != null)
            {
                if (parent.TryGetComponent(out result))
                {
                    return true;
                }

                parent = parent.parent;
            }
            result = default;

            return false;
        }

        /// <summary>
        /// Self Excluded
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetComponentsInHierarchy<T>(this Component self, List<T> result) where T : Component
        {
            Transform transform = self.transform;

            int childCount = transform.childCount;

            bool isMatch = default;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);

                var comp = child.GetComponents<T>();

                if (comp != null)
                {
                    result.AddRange(comp);

                    child.TryGetComponentsInHierarchy<T>(result);

                    isMatch = true;
                }
            }

            return isMatch;
        }
        /// <summary>
        /// Найти компоненты указанного типа ниже по иерархии (не ищет у самого себя)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryGetComponentsInHierarchy(this Component self, List<Component> result, Type type)
        {
            Transform transform = self.transform;

            int childCount = transform.childCount;

            bool isMatch = default;

            for (int i = 0; i < childCount; i++)
            {
                var comp = transform.GetChild(i).GetComponent(type);

                if (comp)
                {
                    result.Add(comp);

                    comp.TryGetComponentsInHierarchy(result, type);

                    isMatch = true;
                }
            }

            return isMatch;
        }
        /// <summary>
        /// Active and Inactive <see cref="GameObject"/> 
        /// </summary>
        /// <returns></returns>
        public static bool TryGetAllComponentsOnScene<T>(this T self, List<T> result) where T : Component
        {
            var buffer = BufferList<GameObject>.Shared.RentCapacityMax();

            buffer.Clear();

            self.gameObject.scene.GetRootGameObjects(buffer);

            bool isMatch = default;

            for (int i = 0; i < buffer.Count; i++)
            {
                T[] comp = buffer[i].GetComponents<T>();

                if (comp != null)
                {
                    result.AddRange(comp);

                    isMatch = true;
                }

                if (buffer[i].GetComponent<Transform>().TryGetComponentsInHierarchy(result))
                {
                    isMatch = true;
                }
            }

            BufferList<GameObject>.Shared.Return(buffer);

            return isMatch;
        }
        /// <summary>
        /// Active and Inactive <see cref="GameObject"/> 
        /// </summary>
        /// <returns></returns>
        public static bool TryGetAllComponentsOnScene<T>(this Component self, List<T> result) where T : Component
        {
            var buffer = BufferList<GameObject>.Shared.RentCapacityMax();

            buffer.Clear();

            self.gameObject.scene.GetRootGameObjects(buffer);

            bool isMatch = default;

            for (int i = 0; i < buffer.Count; i++)
            {
                result.AddRange(buffer[i].GetComponents<T>());

                if (buffer[i].GetComponent<Transform>().TryGetComponentsInHierarchy<T>(result))
                {
                    isMatch = true;
                }
            }

            BufferList<GameObject>.Shared.Return(buffer);

            return isMatch;
        }
        public static bool TryGetAllComponentsOnScene(this Component self, List<Component> result, Type type)
        {
            var buffer = BufferList<GameObject>.Shared.RentCapacityMax();

            buffer.Clear();

            self.gameObject.scene.GetRootGameObjects(buffer);

            bool isMatch = default;

            for (int i = 0; i < buffer.Count; i++)
            {
                result.AddRange(buffer[i].GetComponents(type));

                if (buffer[i].GetComponent<Transform>().TryGetComponentsInHierarchy(result, type))
                {
                    isMatch = true;
                }
            }

            BufferList<GameObject>.Shared.Return(buffer);

            return isMatch;
        }



        public static T[] GetAllComponentsOnScene<T>(this Component self) where T : Component
        {
            List<T> ts = new List<T>();

            self.TryGetAllComponentsOnScene<T>(ts);

            return ts.ToArray();
        }


        public static int GetComponentsBeneath<T>(this Component component, List<T> result) where T : Component
        {
            int resultCount = result.Count;

            component.TryGetComponentsInHierarchy(result);

            return result.Count - resultCount;
        }

        /// <summary>
        /// По функционалу тоже похож на <see cref="Component.GetComponentInParent{T}"/>
        /// но создан для того чтобы искать неактивные объекты???
        /// ТОЖЕ возвращает компонент или на текущем гейм обжекте либо вверх по иерархиию
        /// Поиск выполняется включая себя
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public static T GetComponentTillRoot<T>(this Component component) where T : Component
        {
            T result = default;

            if (component.TryGetComponent<T>(out result))
            {
                return result;
            }
            else
            {
                if (TryGetComponentInParent(component, out result))
                {
                    return result;
                }
            }

            return result;
        }

        public static T GetComponentAbove<T>(this Component component) where T : Component
        {
            T result = default;

            component.TryGetComponentInParent(out result);

            return result;
        }


    }
    public static class ExtensionString
    {
        public static string ColorRed(this string s) => $"<color=red>{s}</color>";
        public static string ColorWhite(this string s) => $"<color=white>{s}</color>";
        public static string ColorGreen(this string s) => $"<color=green>{s}</color>";
        public static string ColorLime(this string s) => $"<color=lime>{s}</color>";
        public static string ColorYellow(this string s) => $"<color=yellow>{s}</color>";
    }

}// namespace