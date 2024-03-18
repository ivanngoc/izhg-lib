#if UNITY_EDITOR
using IziHardGames.Ticking.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IziHardGames.ApplicationLevel
{
	public static class FactoryUpdateSchemeFromScripting
	{
		public static Dictionary<string, GroupeNested> nestedGroupsOpen = new Dictionary<string, GroupeNested>();
		public static Dictionary<string, GroupeNested> nestedGroupsClose = new Dictionary<string, GroupeNested>();

		private static int countNestedGroups;
		private static int countNestedItems;
		private static GroupeNested currentNestedGroupe;

		public static int countItems;

		private static List<ValuePair> items = new List<ValuePair>();
		public static List<string> groupeNames = new List<string>();
		public static string currentGroupeName;

		public static Action<ValuePair> handlerAddItem;
		public static Action<string> handlerAddGroupe;
		public static Action<string> handlerRemoveGroupe;

		private static bool isBegin;
		public static EUpdateChannel currentChannel;

		public static void Initlize(Action<ValuePair> handler, Action<string> handlerAddGroupe, Action<string> handlerRemoveGroupe)
		{
			countItems = default;

			items.Clear();
			groupeNames.Clear();
			FactoryUpdateSchemeFromScripting.handlerAddItem = handler;
			FactoryUpdateSchemeFromScripting.handlerAddGroupe = handlerAddGroupe;
			FactoryUpdateSchemeFromScripting.handlerRemoveGroupe = handlerRemoveGroupe;
			isBegin = default;


			nestedGroupsOpen.Clear();
			nestedGroupsClose.Clear();
			countNestedGroups = default;
			currentNestedGroupe = default;
			countNestedItems = default;
		}

		public static void GroupeBeginNotNested(string groupeName)
		{
			if (isBegin)
			{
				throw new NotSupportedException($"This approach doesn't allow to nest groups. Finish Current group or use another method");
			}
			isBegin = true;
			groupeNames.Add(groupeName);
			currentGroupeName = groupeName;
			handlerAddGroupe.Invoke(groupeName);
		}
		public static void GroupeEndNotNested()
		{
			isBegin = false;
			handlerRemoveGroupe.Invoke(currentGroupeName);
			currentGroupeName = string.Empty;
		}


		public static void ParallelBegin()
		{
			throw new NotImplementedException();
		}
		public static void ParallelEnd()
		{
			throw new NotImplementedException();
		}

		public static void Update(string name)
		{
			throw new NotImplementedException();
		}

		public static void LateBegin()
		{
			currentChannel = EUpdateChannel.DefaultLate;
		}

		public static void LateEnd()
		{
			currentChannel = EUpdateChannel.None;
		}
		public static void NormalBegin()
		{
			currentChannel = EUpdateChannel.Default;
		}
		public static void NormalEnd()
		{
			currentChannel = EUpdateChannel.None;
		}

		public static void Update(Type type, string methodName)
		{
			// by default
			Update(type, GetMethodInfo(type, methodName), EUpdatePhase.None);
		}
		public static void Update(Type type, string methodName, EUpdatePhase eUpdatePhase)
		{
			Update(type, GetMethodInfo(type, methodName), eUpdatePhase);
		}
		public static void Update(Type type, MethodInfo methodInfo, EUpdatePhase eUpdatePhase)
		{
			AddToBufferAndCheck(type, methodInfo, eUpdatePhase);
		}

		private static void AddToBufferAndCheck(Type type, MethodInfo methodInfo, EUpdatePhase eUpdatePhase)
		{
			ValuePair valuePair = new ValuePair(type, methodInfo, eUpdatePhase, currentGroupeName, currentChannel);
			if (items.Contains(valuePair))
			{
				throw new ArgumentException($"Already existed. {type.FullName}.{methodInfo.Name}");
			}
			valuePair.sequenceNumber = countItems;
			items.Add(valuePair);
			handlerAddItem.Invoke(valuePair);
			countItems++;
		}
		private static MethodInfo GetMethodInfo(Type type, string name)
		{
			return type.GetMethod(name);
		}


		#region Nested
		public static void NestedGroupeBegin(string nameAsId)
		{
			GroupeNested toOpen = new GroupeNested(countNestedGroups, nameAsId, currentNestedGroupe);
			toOpen.updateChannel = currentChannel;
			toOpen.Open(countNestedItems);
			nestedGroupsOpen.Add(nameAsId, toOpen);
			currentNestedGroupe = toOpen;
			countNestedGroups++;
		}
		public static void NestedGroupeEnd(string nameAsId = default)
		{
			GroupeNested toClose = currentNestedGroupe;
			if (!string.IsNullOrEmpty(nameAsId) && nameAsId != toClose.nameAsId)
			{
				throw new ArgumentOutOfRangeException($"You are trying to close not current nested groupe. You must Close Corresponded groupe");
			}

			nestedGroupsClose.Add(toClose.nameAsId, toClose);
			toClose.Close(countNestedItems);
			currentNestedGroupe = toClose.previois;
		}
		public static void UpdateNested(Type type, string methodName)
		{
			UpdateNested(type, methodName, EUpdatePhase.None);
		}
		public static void UpdateNested(Type type, string methodName, EUpdatePhase eUpdatePhase)
		{
			var val = new ValuePair(type, GetMethodInfo(type, methodName), eUpdatePhase, currentNestedGroupe.nameAsId, currentChannel);
			val.sequenceNumber = countNestedItems;
			countNestedItems++;
			currentNestedGroupe.Add(val);
		}

		public static void CompleteNest()
		{
			foreach (var open in nestedGroupsOpen)
			{
				if (!nestedGroupsClose.TryGetValue(open.Key, out GroupeNested close))
				{
					throw new ArgumentOutOfRangeException($"Groupe with nameAsId {open.Key} is not closed");
				}
			}
		}
		#endregion
	}

	public class ValuePair : IEquatable<ValuePair>
	{
		public Type type;
		public MethodInfo method;
		public EUpdatePhase eUpdatePhase;
		public string groupeName;
		public string name;
		public string value;
		/// <summary>
		/// Index in loop
		/// </summary>
		public int sequenceNumber;
		public EUpdateChannel currentChannel;

		public ValuePair(Type type, MethodInfo methodInfo, EUpdatePhase eUpdatePhase, string currentGroupeName, EUpdateChannel currentChannel)
		{
			this.type = type;
			method = methodInfo;
			this.eUpdatePhase = eUpdatePhase;
			groupeName = currentGroupeName;
			name = $"{type.Name}.{methodInfo.Name}()";
			value = $"{type.FullName}.{methodInfo.Name}";
			this.currentChannel = currentChannel;
		}


		public bool Equals(ValuePair other)
		{
			return type == other.type && method == other.method;
		}
	}

	public class GroupeNested
	{
		/// <summary>
		/// Must be unique for each groupe
		/// </summary>
		public string nameAsId;
		public int sequenceNumber;
		public GroupeNested previois;
		public bool isOpened;
		public bool isClosed;
		public List<ValuePair> values = new List<ValuePair>();
		public List<GroupeNested> groups = new List<GroupeNested>();
		public int indexBegin;
		public int indexEnd;
		public object transform;
		public EUpdateChannel updateChannel;

		public GroupeNested(int index, string nameAsId, GroupeNested previous)
		{
			this.nameAsId = nameAsId;
			this.sequenceNumber = index;
			this.previois = previous;
		}

		internal void Add(ValuePair valuePair)
		{
			values.Add(valuePair);
		}
		internal void Add(GroupeNested groupe)
		{
			groups.Add(groupe);
		}

		internal void Open(int countNestedItems)
		{
			isOpened = true;
			int val = countNestedItems - 1;
			indexBegin = (val < 0) ? 0 : val;
		}
		internal void Close(int countNestedItems)
		{
			isClosed = true;
			int val = countNestedItems - 1;
			indexEnd = (val < 0) ? 0 : val;
		}
	}

}
#endif
