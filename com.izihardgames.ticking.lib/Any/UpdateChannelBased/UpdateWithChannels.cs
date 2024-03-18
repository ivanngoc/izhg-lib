using System;

namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Сервис распределения вызовов от <see cref="IUpdateProvider"/> к потребителям.
	/// Регистрирует и группирует <see cref="UpdateStep"/><br/>
	/// Главная задача - выполнение на Main Thread цепочки вызовов. 
	/// </summary>
	/// <remarks>
	/// Update Order
	/// 
	/// </remarks>
	public class UpdateWithChannels : IUpdateService
	{
		public static UpdateWithChannels Shared;

		public UpdateChannelV1 updateChannelDefault = new UpdateChannelV1();
		public UpdateChannelV1 updateChannelDefaultLate = new UpdateChannelV1();
		public UpdateChannelV1 updateChannelFixed = new UpdateChannelV1();

		#region Static
		public static void InsertScheduled(UpdateControlToken item)
		{
			InsertScheduled(Shared, item);
		}
		public static void InsertScheduled(UpdateWithChannels updateService, UpdateControlToken token)
		{
			UpdateChannelV1 updateChannel;
			switch (token.updateOptions.eUpdateChannel)
			{
				case EUpdateChannel.All: throw new NotImplementedException();
				case EUpdateChannel.None: throw new NotImplementedException();
				case EUpdateChannel.Default: updateChannel = updateService.updateChannelDefault; break;
				case EUpdateChannel.DefaultLate: updateChannel = updateService.updateChannelDefaultLate; break;
				case EUpdateChannel.Fixed: updateChannel = updateService.updateChannelFixed; break;
				case EUpdateChannel.PreciseThreadTimer: throw new NotImplementedException();
				default: throw new NotImplementedException();
			}

			UpdateGroupe updateGroupe = updateChannel.GetOrCreateGroupe(token);

			UpdateStep updateStep = updateGroupe.GetOrCreateStep(token);
			updateStep.updateChannel = updateChannel;
			updateStep.updateGroupe = updateGroupe;

			UpdateJob updateJob = updateStep.CreateJob(token);
		}
		#endregion
	}
	/// <inheritdoc cref="UpdateWithChannels"/>
	public class UpdateService<TDataProvider> : UpdateWithChannels where TDataProvider : IUpdateDataProvider
	{
		public static UpdateService<TDataProvider> singleton;
		private readonly IUpdateProvider<TDataProvider> updateProvider;

		public UpdateService(IUpdateProvider<TDataProvider> updateProvider)
		{
			this.updateProvider = updateProvider;
			var type = updateProvider.EUpdateChannel;

			if (type.HasFlag(EUpdateChannel.All))
			{
				updateProvider.ConsumerAdd(EUpdateChannel.Default, UpdateDefault);
				updateProvider.ConsumerAdd(EUpdateChannel.DefaultLate, UpdateDefaultLate);
				updateProvider.ConsumerAdd(EUpdateChannel.Fixed, UpdateFix);
			}
			else
			{
				if (type.HasFlag(EUpdateChannel.Default))
				{
					updateProvider.ConsumerAdd(EUpdateChannel.Default, UpdateDefault);
				}
				if (type.HasFlag(EUpdateChannel.DefaultLate))
				{
					updateProvider.ConsumerAdd(EUpdateChannel.DefaultLate, UpdateDefaultLate);
				}
				if (type.HasFlag(EUpdateChannel.Fixed))
				{
					updateProvider.ConsumerAdd(EUpdateChannel.Fixed, UpdateFix);
				}
			}
		}

		private void UpdateDefault(TDataProvider data, UpdateChannelV1 updateChannel)
		{
			float deltaTime = data.DeltaTime;

			updateChannel.countFrames = data.FrameCount;
			updateChannel.timeDelta = deltaTime;
			updateChannel.Execute();
		}
		private void UpdateDefault(TDataProvider data)
		{
			UpdateDefault(data, updateChannelDefault);
		}
		private void UpdateDefaultLate(TDataProvider data)
		{
			UpdateDefault(data, updateChannelDefaultLate);
		}
		private void UpdateFix(TDataProvider data)
		{
			UpdateDefault(data, updateChannelFixed);
		}
	}

}