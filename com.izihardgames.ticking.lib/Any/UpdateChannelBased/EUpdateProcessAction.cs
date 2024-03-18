using System;

namespace IziHardGames.Ticking.Lib
{
	public enum EUpdateJobType
	{
		None,
		Sync,
		AsyncTask,
		AsyncEnumerator,
		Poll,
		TaskParallel,
	}
	public enum EUpdateGroups
	{
		None,
		System,
		UserControl,
		Data,
		View,
		Clean,
	}

	public enum EUpdatePhase
	{
		None = 0,
		#region System
		System,
		Clock,
		#endregion

		#region UserControl
		CameraCollect,
		/// <summary>
		/// Collect input data from engine provider
		/// </summary>
		InputCollect,
		/// <summary>
		/// Call methods for each control set that has been activated
		/// </summary>
		InputTrigger,
		/// <summary>
		/// Collect user environemnt's datas with raycasters, context's datas that been modified by user actions through input system
		/// </summary>
		EnvironmentCollectWithRaycast,

		EnvironmentCollectRest,
		/// <summary>
		/// Calculate datas based on original datas from previous loop
		/// </summary>
		EnvironmentCalculateInternals,
		/// <summary>
		/// Share internal calculation results. Push notifications.
		/// </summary>
		EnvironmentShareCalculateInternals,

		/// <summary>
		/// Decide wich User Modes or User Actions matched user environemnt current state.
		/// </summary>
		EnvironmentFilter,
		/// <summary>
		/// Resolve collisions between triggers of same controls sets
		/// </summary>
		UserControlResolve,
		/// <summary>
		/// Execute user actions
		/// </summary>
		UserControlExecution,
		#endregion

		#region Data
		/// <summary>
		/// AI deciding what to do before any calculation. 
		/// Also apply changes from previous <see cref="GameMechanic"/> Loop like after complete one action need to start doing another action
		/// </summary>
		DecisionMaking,
		/// <summary>
		/// Filter/Query/Select and etc
		/// </summary>
		PrepareDataLoop,
		/// <summary>
		/// Execute calculations (Suppose to be most highloaded for CPU)
		/// </summary>
		DataLoop,

		CalculateRotation,
		CalculateDirection,
		CalculateMove,
		/// <summary>
		/// [By default] Основная логика игры.
		/// </summary>
		GameMechanic,
		#endregion
		/// <summary>
		/// Процессы которые происходят раз в несколько фреймов
		/// </summary>
		ProcessWithCertainFramePeriod,
		/// <summary>
		/// Процессы которые происходят раз в несколько секунд
		/// </summary>
		ProcessWithCertainTimePeriod,


		#region View
		PrepareViewLoop,
		ViewLoop,
		EndViewLoop,
		#endregion

		#region Reset/Refrash/Clean
		/// <summary>
		/// Reset to defaults values for next frame calculation
		/// </summary>
		Clean,
		#endregion
	}



	[Flags]
	public enum EUpdateChannel
	{
		All = -1,
		None = 0,
		/// <summary>
		/// Unity Update. Before Animation and coroutines.
		/// </summary>
		Default = 1 << 0,
		/// <summary>
		/// Unity Update In the end of pipeline. After Animation Loop
		/// </summary>
		DefaultLate = 1 << 1,

		Fixed = 1 << 2,
		/// <summary>
		/// <see cref="System.Threading.Timer"/>
		/// </summary>
		PreciseThreadTimer = 1 << 3,
	}

	public enum EUpdateInclusive
	{
		None,
		Inclusive,
		Exclusive,
	}
	/// <summary>
	/// Determine when <see cref="UpdateStep"/> will stop
	/// </summary>
	public enum EUpdateLifespan
	{
		None,
		/// <summary>
		/// Update until get command to stop update
		/// </summary>
		Persistent,
		/// <summary>
		/// Update Every tick while <see cref="UpdateStep.funcCondition"/> is <see langword="true"/>
		/// </summary>
		WhileFunc,
		/// <summary>
		/// Update until <see cref="UpdateStep.funcCondition"/> is <see langword="true"/>
		/// </summary>
		UntilFunc,
		/// <summary>
		/// <see cref="UpdateStep"/> provide method to call to stop update process
		/// </summary>
		UntilCallback,
		/// <summary>
		/// Update exact <see cref="UpdateStep.countFrameTarget"/> times
		/// </summary>
		FixedFrameCount,
		/// <summary>
		/// Update for <see cref="UpdateStep.timeTarget"/> seconds
		/// </summary>
		FixedTime,
	}
	/// <summary>
	/// Из чего состоит Update процесс. (действие и условие действия и их комбинации)
	/// </summary>
	public enum EUpdateComposition
	{
		None,
		Action,
		ActionJoinedCondition,
		ActionSeparateCondition,
	}

	/// <summary>
	/// Conditions for execute update or skip
	/// </summary>
	public enum EUpdateFiltering
	{
		None,
		FlagEnabled,
		FuncFilter,
	}

	public enum EUpdateProcessAction
	{
		None,
		ActionUpdate,
		ActionUpdateLate,
		ActionUpdateFix,
		/// <summary>
		/// Do <see cref="Func{TResult}"/> while <see langword="true"/>
		/// </summary>
		ActionUpdateWhile,
		ActionUpdateLateWhile,
		ActionUpdateFixWhile,
		/// <summary>
		/// Do <see cref="Action"/> while <see cref="Func{TResult}"/> <see langword="true"/>
		/// </summary>
		ActionUpdateWhileSeparateInclusive,
		ActionUpdateLateWhileSeparateInclusive,
		ActionUpdateFixWhileSeparateInclusive,

		ActionUpdateWhileSeparateExclusive,
		ActionUpdateLateWhileSeparateExclusive,
		ActionUpdateFixWhileSeparateExclusive,

		/// <summary>
		/// Do <see cref="Func{TResult}"/> until <see langword="true"/>
		/// </summary>
		ActionUpdateUntil,
		ActionUpdateLateUntil,
		ActionUpdateFixUntil,

		ActionUpdateUntilSeparateInclusive,
		ActionUpdateLateUntilSeparateInclusive,
		ActionUpdateFixUntilSeparateInclusive,

		ActionUpdateUntilSeparateExclusive,
		ActionUpdateLateUntilSeparateExclusive,
		ActionUpdateFixUntilSeparateExclusive,

		InterfaceUpdate,
		InterfaceUpdateLate,
		InterfaceUpdateFix,
	}
}