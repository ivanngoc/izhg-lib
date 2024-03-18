#pragma warning disable
using IziHardGames.Core;
using System;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


namespace IziHardGames.Ticking.Lib.ApplicationLevel
{
	/// <summary>
	/// Дата и время
	/// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
	/// </summary>
	public class ClockDateTime : MonoBehaviour, IziHardGames.Core.IOrderable, IziHardGames.Core.IUpdatableDefault
	{
		public int Priority { get => ConstantsCore.GROUPE_CORE; }

		[Space]
		[NonSerialized, Unused] float leftSecUntilSync = 0;
		[NonSerialized, Unused] float syncPeriod = 600f;
		[NonSerialized, Unused] string lastServerDateTimeResult;
		[SerializeField] string dateTimeString;
		[Space]
		public DateTime defaultDateTime = default;
		/// <summary>
		/// 
		/// </summary>
		public DateTime currentDateTime;
		/// <summary>
		/// 
		/// </summary>
		public static DateTime currentDateTimeStatic;

		public TimeSpan timeSpan;

		[SerializeField] public double millsecondsDouble = default;
		[Space]
		[SerializeField] public bool isOnlineTime;

		[Space]
		[SerializeField] UnityEvent OnDateTimeSyncEvent;
		[SerializeField] UnityEvent OnDateTimeSyncErrorEvent;

		private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("es-ES");

		#region Unity Message
		private void Reset()
		{
			leftSecUntilSync = 0;
			syncPeriod = 600;
		}
		#endregion

		public void Initilize_De()
		{
			enabled = false;
		}
		public void Initilize()
		{
			enabled = true;
			millsecondsDouble = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
		}

		public void ExecuteUpdate()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;

			leftSecUntilSync -= unscaledDeltaTime;

			if (false)
			{
				if (leftSecUntilSync < 0)
				{
					SyncTimeWithServer();

					leftSecUntilSync = syncPeriod;
				}
			}

			millsecondsDouble += unscaledDeltaTime * 1000;

			currentDateTime = GetTimeFromMilliseconds((long)millsecondsDouble);

			currentDateTimeStatic = currentDateTime;

			dateTimeString = currentDateTime.ToString("u", cultureInfo);
		}

		/// состав
		/// <see cref="Chane"/>
		/// <see cref="ChaneModule"/>
		/// <see cref=""/>
		string StripMiliseconds(string t)
		{
			string[] split = t.Split('.');

			return split[0];
		}

		/// <summary>
		/// Утсановить системное время
		/// </summary>
		public void SetLocalTime()
		{
			currentDateTime = DateTime.Now;
			millsecondsDouble = currentDateTime.Millisecond;
		}


		public void SyncTimeWithServer()
		{
			throw new System.NotImplementedException();
		}

		private void HandleTimeRequestResult(AsyncOperation asyncOperation)
		{
			throw new System.NotImplementedException();
		}
		/// <summary>
		/// обработка ошибки синхронизации
		/// </summary>
		public void HandleTimeSyncError()
		{
			throw new System.NotImplementedException();
		}
		/// <summary>
		/// Получить дату время из милисекунд
		/// </summary>
		/// 1 милисекунда = 10 000 тактов (ticks)
		/// <param name="milliseconds"></param>
		public DateTime GetTimeFromMilliseconds(long milliseconds)
		{
			if (milliseconds < 0) return default;

			TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);

			DateTime dateTime = new DateTime(time.Ticks);

			return dateTime;
		}

		#region Subtype
		/// <summary>
		/// для инспектора
		/// </summary>
		[Serializable]
		public class CustomDateTime
		{
			public DateTime storedDateTime;
			public string storedStringDateTime;

			public int years;
			public int months;
			public int days;

			public int hours;
			public int minutes;
			public int seconds;

			public string DateTimeString { get { return $"{years}-{months}-{days} {hours}:{minutes}:{seconds}"; } }

			public DateTime DateTimeFromFields { get { return new DateTime(years, months, days, hours, minutes, seconds); } }

			public CustomDateTime(string dateTime)
			{
				//storedDateTime = JsonParseHelper.JsonToDateTime(dateTime);
				storedStringDateTime = dateTime;

				years = storedDateTime.Year;
				months = storedDateTime.Month;
				days = storedDateTime.Day;

				hours = storedDateTime.Hour;
				minutes = storedDateTime.Minute;
				seconds = storedDateTime.Second;
			}
		}
		#endregion
	}
}


