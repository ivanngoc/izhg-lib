namespace IziHardGames.Ticking.Lib
{
	public class UpdateOptions
	{
		/// <summary>
		/// определяет <see cref="UpdateStep.priority"/>. Чем выше значение тем раньше выполнение и меньше <see cref="UpdateStep.priority"/>.
		/// При колизиях значений значение <see cref="UpdateStep.priority"/> очередь задается по правилу первый пришел - раньше выполнился.
		/// </summary>
		public int priority;
		public int type;
		public int frameRate;
		public float time;
		public bool isMainTh;
		public bool isScaledDeltaTime;
		private bool isMainThreadOnly;

		public EUpdateChannel eUpdateChannel;
		public EUpdateInclusive eUpdateInclusive;
		public EUpdateLifespan eUpdateLifespan;
		public EUpdateComposition eUpdateComposition;
		public EUpdateFiltering eUpdateFiltering;
		public EUpdateJobType eUpdateJobType;

		public readonly static UpdateOptions Default = new UpdateOptions(
			type: default,
			priority: 0,
			frameRate: default,
			time: default,
			isMainThreadOnly: true,
			isScaledDeltaTime: true,
			eUpdateType: EUpdateChannel.Default,
			eUpdateInclusive: EUpdateInclusive.None,
			eUpdateLifespan: EUpdateLifespan.Persistent,
			eUpdateComposition: EUpdateComposition.Action,
			eUpdateFiltering: EUpdateFiltering.FlagEnabled,
			eUpdateJobType: EUpdateJobType.Sync);

		public UpdateOptions(int type,
					   int priority,
					   int frameRate,
					   float time,
					   bool isMainThreadOnly,
					   bool isScaledDeltaTime,
					   EUpdateChannel eUpdateType,
					   EUpdateInclusive eUpdateInclusive,
					   EUpdateLifespan eUpdateLifespan,
					   EUpdateComposition eUpdateComposition,
					   EUpdateFiltering eUpdateFiltering,
					   EUpdateJobType eUpdateJobType)
		{
			this.priority = priority;
			this.type = type;
			this.frameRate = frameRate;
			this.time = time;
			this.isMainThreadOnly = isMainThreadOnly;
			this.isScaledDeltaTime = isScaledDeltaTime;
			eUpdateChannel = eUpdateType;
			this.eUpdateInclusive = eUpdateInclusive;
			this.eUpdateLifespan = eUpdateLifespan;
			this.eUpdateComposition = eUpdateComposition;
			this.eUpdateFiltering = eUpdateFiltering;
			this.eUpdateJobType = eUpdateJobType;
		}

	}
}