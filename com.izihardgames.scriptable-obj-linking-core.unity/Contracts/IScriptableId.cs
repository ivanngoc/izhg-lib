using System;
using IziHardGames.Apps.Abstractions.ForUnity.Presets;

namespace IziHardGames.Apps.ForUnity
{
    /// <summary>
    /// При загрузке добавлении впервые в систему <see cref="ProjectPresets"/>производится сканирование <see cref="ProjectPresets.All"/>.
    /// Каждая реализация <see cref="AppPreset"/> которая имеет этот интерфейс будет вызывать <see cref="IziHandler"/> для каждого объекта-пресета внутри по ключу Type интерфейса
    /// </summary>
	public interface IAutoRegPresetItem
    {
        public Type Type { get; }
    }

    public interface IScriptableId
    {
        public ScriptableId Id { get; set; }
    }
}