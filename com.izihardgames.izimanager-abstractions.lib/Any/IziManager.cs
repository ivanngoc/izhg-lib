using System;

namespace IziHardGames.IziManager.Abstractions.Lib
{
    /// <summary>
    /// Менеджер может добавлять любой объект а также присваивать ему ID. Таким образом при разработке типов не нужно будет в разрабатываемых типах создавать поле ID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class IziManager<T>
    {
        public static ReadOnlyManager<T> ReadOnly = new ReadOnlyManager<T>();
        public static object Update;
        public static object Delete;
        public static object ReadOnlyQuery;
        /// <summary>
        /// apply some action foreach
        /// </summary>
        public static object Batch;
        public static object Add;
        /// <summary>
        /// Acess Unit's components. Use in case Logical Unit is splitted into objects with various types
        /// </summary>
        public static object Componets;
        /// <summary>
        /// Ассоциации объектов. Удобно когда есть явные связи. Создает/удаляет ассоциации/связи
        /// </summary>
        public static object Associations;

#if DEBUG
#pragma warning disable
        private static void Test()
        {
            int id = 500;
            ref var item = ref IziManager<string>.ReadOnly[500];
        }
#pragma warning restore
#endif
        /// <summary>
        /// 
        /// </summary>
        /// <returns>token to use for unlock</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public static int Lock()
        {
            throw new System.NotImplementedException();
        }
        public static int Unlock(int token)
        {
            throw new System.NotImplementedException();
        }
        public static void ProtectFromRead()
        {
            throw new System.NotImplementedException();
        }
        public static void ProtectFromWrite()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ReadOnlyManager<T>
    {
        private CollectionSourceAdapter<T> source;
        public ref T this[int id]
        {
            get => ref source.GetItemByIdRef();
        }

        public int IdOf(T item)
        {
            throw new System.NotImplementedException();
        }
    }

    public abstract class CollectionSourceAdapter<T>
    {

    }

    public static class ExtensionsForCollectionSourceAdapter
    {
        public static T GetItemById<T>(this CollectionSourceAdapter<T> source)
        {
            throw new System.NotImplementedException();
        }
        public static ref T GetItemByIdRef<T>(this CollectionSourceAdapter<T> source)
        {
            throw new System.NotImplementedException();
        }
        public static T GetItemByIndex<T>(this CollectionSourceAdapter<T> source)
        {
            throw new System.NotImplementedException();
        }
    }
}
