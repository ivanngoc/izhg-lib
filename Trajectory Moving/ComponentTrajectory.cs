using System;
using System.Collections.Generic;
using UnityEngine;

namespace IziHardGames.Libs.Engine.TrajectoryMoving
{
	/// <summary>
	/// Не сплайновая траектория движения. Интерполирована точками
	/// </summary>
	public class ComponentTrajectory : MonoBehaviour
	{
		/// <summary>
		/// 0...1
		/// </summary>
		[SerializeField] protected bool isComplete;

		[SerializeField] protected float progress;
		[SerializeField] protected float totalDistance;

		[SerializeField] protected int indexStart;
		[SerializeField] protected int indexEnd;
		[SerializeField] protected List<Vector3> trajectory;
		/// <summary>
		/// точки <see cref="trajectory"/> выраженные в нормализованном значении. половина пути будет = 0.5. Начало 0 и конец 1.
		/// </summary>
		[SerializeField] protected List<float> ranges;
		[SerializeField] protected List<float> distances;

		public event Action OnMoveCmpleteEvent;

		#region Unity Message		
		public virtual void Reset()
		{

		}
		#endregion

		public virtual void Initilize(int capacity)
		{
			trajectory = new List<Vector3>(capacity);
		}
		public virtual void Initilize_De()
		{

		}

		public void SetTrajectory()
		{
			distances.Clear();

			distances.Add(0);

			totalDistance = default;

			for (int i = 1; i < trajectory.Count; i++)
			{
				float distanceSegment = (trajectory[i - 1] - trajectory[i]).magnitude;

				distances.Add(distanceSegment);

				totalDistance += distanceSegment;
			}

			ranges.Clear();
			ranges.Add(0);

			for (int i = 1; i < trajectory.Count; i++)
			{
				ranges.Add(ranges[i] / totalDistance);
			}
		}

		public void SetTrajectory(List<Vector3> trajectoryArg)
		{
			trajectory = trajectoryArg;

			SetTrajectory();
		}

		public void SetTrajectoryCopy(List<Vector3> trajectoryArg)
		{
			trajectory.Clear();

			trajectoryArg.CopyTo(trajectory);

			SetTrajectory();
		}

		public virtual void SetProgress(float progress)
		{
			throw new NotImplementedException();
		}

		public virtual void MoveForward(float progressAdd)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Движение по тракетории с одинаковым растоянием между точками
		/// </summary>
		/// <param name="progressAdd"></param>
		public virtual void MoveForwardLinear(float progressAdd)
		{
			progress += progressAdd;

			if (progress >= 1)
			{
				if (indexEnd + 1 < trajectory.Count)
				{
					indexStart++;
					indexEnd++;
				}
				else
				{
					isComplete = true;
				}
			}
			else
			{
				isComplete = true;
			}

			if (!isComplete)
			{
				transform.position = Vector3.Lerp(trajectory[indexStart], trajectory[indexEnd], progress);
			}
			else
			{
				transform.position = trajectory[indexEnd];

				progress = 1f;

				NotifyCompleteMove();
			}

		}

		protected void NotifyCompleteMove()
		{
			OnMoveCmpleteEvent?.Invoke();
		}
		/// <summary>
		/// Вставляет точку в конец маршрута
		/// </summary>
		public void TrajectoryPointAdd()
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Удаляет точку из маршрута по индексу
		/// </summary>
		public void TrakectoryPointRemove(int index)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Сокращает марщрут на указаное количество точек с конца
		/// </summary>
		public void TrakectoryPointShrink(int count)
		{
			throw new NotImplementedException();
		}
	}
}