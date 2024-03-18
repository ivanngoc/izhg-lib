using System;
using System.Reflection;
using UnityEngine;

namespace IziHardGames.Libs.Engine.SerilizableWraps
{
	/// <summary>
	/// Meta data for displaying in Unity Editor.
	/// Contains fileds that of premitive types and typws supported by UnityEditor like array, list and etc
	/// </summary>
	[Serializable]
	public abstract class Wrap
	{

	}

	///<inheritdoc cref="Wrap"/>
	[Serializable]
	public class WrapForObject : WrapForType
	{
		/// <summary>
		/// id of concrete exemplar of <see cref="Type"/>
		/// </summary>
		public int hashcode;

		public WrapForObject(object obj) : base(obj.GetType())
		{
			hashcode = obj.GetHashCode();
		}
	}

	///<inheritdoc cref="Wrap"/>
	[Serializable]
	public class WrapForType : Wrap
	{
		[Header("Object Info")]
		[SerializeField] public string typeNameShort;
		[SerializeField, TextArea] public string typeNameFull;

		public WrapForType(Type reflectedType)
		{
			typeNameShort = reflectedType.Name;
			typeNameFull = reflectedType.FullName;
		}
	}

	///<inheritdoc cref="Wrap"/>
	[Serializable]
	public class WrapForMethod : WrapForType
	{
		[Header("Method Info")]
		[SerializeField] public string nameShort;
		[SerializeField, TextArea] public string NameFull;

		public WrapForMethod(MethodInfo method) : base(method.ReflectedType)
		{
			nameShort = method.Name;
			NameFull = $"{method.ReflectedType}.{method.Name}()";
		}
	}
}