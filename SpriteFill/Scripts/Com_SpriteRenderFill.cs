using System;
using UnityEngine;


namespace IziHardGames.GameProject2
{
	public class Com_SpriteRenderFill : MonoBehaviour
	{
		[SerializeField] public int index;
		[SerializeField] public SpriteRenderer spriteRenderer;
		[SerializeField, Range(0, 1f)] float fillAmount;
		public Func<float> funcGetValue;

		#region Unity Message
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			//SetValueLeftToRight(fillAmount);
		}
		private void Reset()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.material = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Library/_Engine/Rendering/SpriteFill/Materials/MaterialFill.mat", typeof(Material)) as Material;
		}
#endif
		#endregion
		/// <summary>
		/// set sprite filling amount
		/// </summary>
		/// <param name="ff"></param>
		public void SetValueLeftToRight(float ff)
		{
			fillAmount = ff;

			spriteRenderer.material.SetFloat("_FillX", ff);
		}
		public void SetColor(Color color)
		{
			spriteRenderer.color = color;
		}
	}
}