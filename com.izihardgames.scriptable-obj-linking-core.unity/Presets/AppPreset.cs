using System;
using System.Collections.Generic;
using IziHardGames.Apps.ForUnity;
using UnityEngine;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    public abstract class AppPreset : ScriptableObject, IScriptableId
    {
        public ScriptableId? idPreset;
        public virtual IEnumerable<ScriptableObject> AllAsScriptables { get => throw new System.NotImplementedException($"Not Overrided for {GetType().FullName}"); }
        public T As<T>() where T : AppPreset => (this as T) ?? throw new InvalidCastException();
        public ScriptableId Id { get => idPreset ?? throw new NullReferenceException(); set => idPreset = value; }
    }
}
