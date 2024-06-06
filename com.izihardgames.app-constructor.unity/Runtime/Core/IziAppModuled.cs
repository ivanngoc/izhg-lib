using System.Collections.Generic;
//using IziHardGames.Apps.Abstractions.Lib;


namespace IziHardGames.AppConstructor
{
    public class IziAppModuled //: IIziApp
    {
        private Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        public T GetItemAs<T>(string key) where T : class => keyValuePairs[key] as T;
        public void PutItem(string key, object item) => keyValuePairs[key] = item;
        public bool IsModuleLoaded(string guid) => GetItemAs<IziModuleBind>(guid).status.Loaded;
    }
}
