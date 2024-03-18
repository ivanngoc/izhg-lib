using System.Collections;
using System.Collections.Generic;
using IziHardGames.Apps.Abstractions.NetStd21;
using IziHardGames.Apps.ForUnity;
using IziHardGames.Apps.Abstractions.Lib;
using UnityEngine;
using System;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
    public abstract class AppPreset : ScriptableObject, IScriptableId
    {
        public ScriptableId? idPreset;
        public virtual IEnumerable<ScriptableObject> AllAsScriptables { get => throw new System.NotImplementedException($"Not Overrided for {GetType().FullName}"); }
        public T As<T>() where T : AppPreset => (this as T) ?? throw new InvalidCastException();
        public ScriptableId Id { get => idPreset ?? throw new NullReferenceException(); set => idPreset = value; }
    }

    public class MonoAppPresetService : IIziService
    {
        private readonly ProjectPresets presets;

        public MonoAppPresetService(ProjectPresets presets)
        {
            this.presets = presets;
        }

        public void Start()
        {
            foreach (var aggregator in presets.All)
            {
                if (aggregator is IAutoRegPresetItem autreg)
                {
                    var type = autreg.Type;
                    var handler = IziHandler.selector[type];
                    var items = aggregator.AllAsScriptables;
                    foreach (var item in items)
                    {
                        handler.Invoke(item);
                    }
                }
            }
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
