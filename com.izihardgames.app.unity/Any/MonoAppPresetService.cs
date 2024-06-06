using IziHardGames.Apps.ForUnity;

namespace IziHardGames.Apps.Abstractions.ForUnity.Presets
{
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
