using UnityEngine;

namespace IziHardGames.View
{
	public interface IVisionControllable
	{
		ControlOfVisibility ControlView { get; set; }
	}
	public abstract class ControlOfVisibility : MonoBehaviour
	{
		[SerializeField] public int idBind;
		[SerializeField] public int idGroupeMain;

		[SerializeField] public bool isVisibleCachedState;
		[SerializeField] public bool isVisible;

		[SerializeField] private ControlViewInfo controlViewInfo;

		#region Unity Message

		public virtual void OnDestroy()
		{
			controlViewInfo.RegistDe(this);
		}
		public virtual void Reset()
		{
#if UNITY_EDITOR
			controlViewInfo = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/[Project] GameProject1/Scriptable Objects/ControlViewInfo.asset", typeof(ControlViewInfo)) as ControlViewInfo;
#endif
		}

#if UNITY_EDITOR
		[ContextMenu("Validate")]
		public virtual void Validate()
		{
			Reset();
		}
#endif

		#endregion


		public virtual void Set(bool isOn)
		{
			if (isOn)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		public void Initilize()
		{
			controlViewInfo.Regist(this);
		}

		public virtual void Show()
		{
#if UNITY_EDITOR
			if (isVisible) throw new System.ArgumentOutOfRangeException("Object is alread showed");
#endif
			isVisibleCachedState = isVisible;
			isVisible = true;
		}
		public virtual void Hide()
		{
#if UNITY_EDITOR
			if (!isVisible) throw new System.ArgumentOutOfRangeException("Object is alread hided");
#endif
			isVisibleCachedState = isVisible;
			isVisible = false;
		}
		[ContextMenu("Hide If Visible (Base)")]
		public virtual void HideIfVisible()
		{
			if (isVisible)
			{
				Hide();
			}
		}
		public virtual void ShowIfHided()
		{
			if (!isVisible)
			{
				Show();
			}
		}
		public virtual void Switch()
		{
			Set(!isVisible);
		}
	}
}