using System.Collections.Generic;
using IziHardGames.Libs.NonEngine.Game.Abstractions;
using IziHardGames.Libs.NonEngine.Sorting;

namespace IziHardGames.Ticking.Lib
{
	/// <summary>
	/// Определяет тип обновления согласно движку (Update/Late Update/Fix Update)
	/// </summary>
	public class UpdateChannelV1 : ITickChannel
	{
		///// <summary>
		///// Определяет порядок в котором будет вызвана группа из <see cref="UpdateProcess"/>. Не путать с Priority
		///// </summary>
		//public int order;
		/// <summary>
		/// <see cref="IUpdateDataProvider.FrameCount"/>
		/// </summary>
		public int countFrames;
		/// <summary>
		/// Время от <see cref="IUpdateDataProvider.DeltaTime"/>
		/// </summary>
		public float timeDelta;

		public UpdateGroupe GroupeByDefault;

		private List<UpdateGroupe> items = new List<UpdateGroupe>(64);
		private List<UpdateGroupe> itemsAdd = new List<UpdateGroupe>(64);
		private List<UpdateGroupe> ItemsRemove = new List<UpdateGroupe>(64);

		public UpdateChannelV1()
		{
			GroupeByDefault = new UpdateGroupe(UpdateGroupe.DEFAULT_INITIAL_CAPACITY)
			{
				idGroupe = UpdateGroupe.DEFAULT_ID_GROUPE,
				updateChannel = this,
				order = 0,
			};
			items.Add(GroupeByDefault);
		}

		public void Execute()
		{
			if (itemsAdd.Count > 0)
			{
				for (int i = 0; i < itemsAdd.Count; i++)
				{
					AddInstant(itemsAdd[i]);
				}
				itemsAdd.Clear();
			}

			for (int i = 0; i < items.Count; i++)
			{
				items[i].Execute();
			}

			if (ItemsRemove.Count > 0)
			{
				for (int i = 0; i < ItemsRemove.Count; i++)
				{
					RemoveInstant(ItemsRemove[i]);
				}
				ItemsRemove.Clear();
			}
		}

		public void AddScheduled(UpdateGroupe item)
		{
			itemsAdd.Add(item);
		}
		public void AddInstant(UpdateGroupe item)
		{
			item.updateChannel = this;

			if (items.Count > 0)
			{
				item.order = items.InsertWithBinaryTreeAscending(item);

				for (int i = item.order + 1; i < items.Count; i++)
				{
					items[i].order++;
				}
			}
			else
			{
				item.order = default;
				items.Add(item);
			}
		}
		public void RemoveScheduled(UpdateGroupe item)
		{
			ItemsRemove.Add(item);
		}


		public UpdateGroupe GetOrCreateGroupe(UpdateControlToken updateToken)
		{
			foreach (var item in itemsAdd)
			{
				if (item.idGroupe == updateToken.idGroupe) return item;
			}
			foreach (var item in items)
			{
				if (item.idGroupe == updateToken.idGroupe) return item;
			}
			UpdateGroupe updateGroupe = new UpdateGroupe(UpdateGroupe.DEFAULT_INITIAL_CAPACITY)
			{
				idGroupe = updateToken.idGroupe,
				order = updateToken.idGroupe,
			};
			AddScheduled(updateGroupe);
			return updateGroupe;
		}

		public bool TryAddScheduled(UpdateGroupe item)
		{
			if (items.Contains(item) || itemsAdd.Contains(item)) return false;
			AddScheduled(item);
			return true;
		}

		public void RemoveInstant(UpdateGroupe item)
		{
			items.Remove(item);
		}

#if UNITY_EDITOR
		public List<UpdateGroupe> DebugGetUpdateGroups() => items;
#endif
	}
}