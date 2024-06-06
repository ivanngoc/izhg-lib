using System;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;


namespace IziHardGames.AppConstructor
{
    public abstract class IziModuleBind : ScriptableObject, IGuidAsString
    {
        [SerializeField] protected string guid = string.Empty;

        private int resolveTries = 100;
        public readonly IziModuleStatus status = new IziModuleStatus();

        public string GuidAsString { get => guid; protected set => guid = value; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - Module Load Completes<br/>
        /// </returns>
        public abstract void LoadModuleBegin(IziAppModuled app);
        public abstract void LoadModuleEnd();
        public abstract bool IsLoadCompleted();
        /// <summary>
        /// Itterate for progressing module's loading
        /// </summary>
        public abstract void ItterateLoading();
        public abstract void CleanupStaticFields();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// <see langword="true"/> - dependecies resolved <br/>
        /// <see langword="false"/> - dependecies net resolved;
        /// </returns>
        public abstract bool ResolveDependecies();
#if UNITY_EDITOR
        [ContextMenu("Base Izi validate")]
        protected virtual void OnValidate()
        {
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(GetType());
            var atr = attrs.FirstOrDefault(x => x is GuidAttribute) as GuidAttribute;
            if (atr != null)
            {
                guid = atr.Value;
            }
        }
#endif
        public void PutSelfToApp(IziAppModuled app)
        {
            if (!string.IsNullOrEmpty(guid))
            {
                app.PutItem(guid, this);
            }
            app.PutItem(GetType().AssemblyQualifiedName, this);
        }

        internal void AntiDeadLock()
        {
            resolveTries--;
            if (resolveTries < 0) throw new Exception($"Deadlock? {GetType().FullName}");
        }
    }
}
