using System;
using UnityEngine;


namespace IziHardGames.Libs.Engine.TrajectoryMoving
{
	[RequireComponent(typeof(Rigidbody))]
	public class ComponentTrajectoryRigid3D : ComponentTrajectory
	{
		[SerializeField] new private Rigidbody rigidbody;

		#region Unity Message	
		public override void Reset()
		{
			base.Reset();

			rigidbody = GetComponent<Rigidbody>();
		}
		#endregion
		/// <summary>
		/// продвижение вперед.
		/// </summary>
		/// <param name="progressAdd01"></param>
		public override void MoveForward(float progressAdd)
		{
			progress = Mathf.Clamp01(progress + progressAdd);

			while (progress > ranges[indexEnd])
			{
				indexEnd++;
				indexStart++;
			}

			float delta = progressAdd / ranges[indexEnd];

			float lerp = (progress - ranges[indexStart]) / (ranges[indexStart] - ranges[indexEnd]);

			rigidbody.MovePosition(Vector3.Lerp(trajectory[indexStart], trajectory[indexEnd], lerp));
			throw new NotImplementedException();
		}
		/// <summary>
		/// Движение по тракетории с одинаковым растоянием между точками
		/// </summary>
		/// <param name="progressAdd"></param>
		public override void MoveForwardLinear(float progressAdd)
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
				rigidbody.MovePosition(Vector3.Lerp(trajectory[indexStart], trajectory[indexEnd], progress));
			}
			else
			{
				rigidbody.MovePosition(trajectory[indexEnd]);

				progress = 1f;

				NotifyCompleteMove();
			}
		}
	}
}