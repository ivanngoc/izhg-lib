using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.Concurrency
{
	/// <summary>
	/// Common variant of pipeline to resolve concurency (collisions)
	/// </summary>
	public class ConcurrencyResolver
	{
		public static ConcurrencyResolver Default { get; set; }

		public List<ConcurrencyRules> rules = new List<ConcurrencyRules>();
		/// <summary>
		/// Each item is unique
		/// </summary>
		public List<ConcurrencyItem> items = new List<ConcurrencyItem>();
		/// <summary>
		/// Each group can contain same items.
		/// </summary>
		public List<ConcurrencyGroup> concurrencyGroups = new List<ConcurrencyGroup>();

		public virtual void Execute()
		{
			Refresh();
			CollectDatas();
			CallLinkedHandlers();
			ExchangeWithInternalResults();
			CheckConditions();
#if DEBUG
			foreach (var rule in rules)
			{
				if (rule.withNotLinked.Count > 0) throw new Exception($"There is still some rules without established relations");
			}
			if (false)
			{

				Compare();
				Sort();
			}
#endif
			Eliminate();
			Pullout();
			// avoid recursion
			// how to avoid dead lock?
			// should there be floating result?
			Finish();
		}

		internal ConcurrencyGroup GetOrCreateGroupe(ConcurrencyRules concurrencyRules)
		{
			ConcurrencyGroup concurrencyGroup = concurrencyGroups.FirstOrDefault(x => x.idGroupe == concurrencyRules.idGroupe);

			if (concurrencyGroup == null)
			{
				concurrencyGroup = new ConcurrencyGroup(concurrencyRules.idGroupe);
				concurrencyGroups.Add(concurrencyGroup);
			}
			return concurrencyGroup;
		}

		internal void AddItem(ConcurrencyItem item)
		{
			items.Add(item);

			AddRule(item.concurrencyRules);

			Type bindType = item.bind.GetType();

			/// восстановление связей (создание по упрощенной схеме)
			// многие к этому
			// add new object to rules that requested that object
			foreach (var rule in rules)
			{
				for (int i = 0; i < rule.withNotLinked.Count; i++)
				{
					Type requestType = rule.withNotLinked[i];

					if (requestType.IsAssignableFrom(bindType))
					{
						rule.OnlyWith(item);
						rule.withNotLinked.RemoveAt(i);
						i--;
					}
				}

				for (int i = 0; i < rule.yiledToNotLinked.Count; i++)
				{
					Type requestType = rule.yiledToNotLinked[i];
					if (requestType.IsAssignableFrom(bindType))
					{
						rule.YieldTo(item);
						rule.yiledToNotLinked.RemoveAt(i);
						i--;
					}
				}
				for (int i = 0; i < rule.eliminateNotLinked.Count; i++)
				{
					Type requestType = rule.eliminateNotLinked[i];
					if (requestType.IsAssignableFrom(bindType))
					{
						rule.Eliminate(item);
						rule.eliminateNotLinked.RemoveAt(i);
						i--;
					}
				}
			}
			// этот ко многим
			// collect objects that requested by this item
			foreach (var existedItem in items)
			{
				Type comparedType = existedItem.bind.GetType();
				if (item.concurrencyRules.withNotLinked.Contains(comparedType))
				{
					item.concurrencyRules.withNotLinked.Remove(comparedType);
					item.concurrencyRules.OnlyWith(existedItem);
				}

				if (item.concurrencyRules.yiledToNotLinked.Contains(comparedType))
				{
					item.concurrencyRules.yiledToNotLinked.Remove(comparedType);
					item.concurrencyRules.YieldTo(existedItem);
				}
				if (item.concurrencyRules.eliminateNotLinked.Contains(comparedType))
				{
					item.concurrencyRules.eliminateNotLinked.Remove(comparedType);
					item.concurrencyRules.Eliminate(existedItem);
				}
			}
		}

		internal void AddRule(ConcurrencyRules concurrencyRules)
		{
			if (!rules.Contains(concurrencyRules))
			{
				rules.Add(concurrencyRules);
				concurrencyRules.BindTo(this);
			}
		}

		/// <summary>
		/// Execute methods to copy or get-copy input data to local storage for further operations.
		/// </summary>
		private void CollectDatas()
		{
			foreach (var item in items)
			{
				item.CollectDatas();
			}
		}
		private void CallLinkedHandlers()
		{
			foreach (var item in items)
			{
				if (item.isActivated)
				{
					item.CallLinkedHandlers();
				}
			}
		}
		private void ExchangeWithInternalResults()
		{
			foreach (var item in items)
			{
				item.PushCalculatedDatas();
			}
		}

		private void CheckConditions()
		{
			foreach (var rule in rules)
			{
				rule.CalculateConditionFlag();
			}

			foreach (var item in items)
			{
				if (item.isActivated)
				{
					item.isActivated = item.concurrencyRules.concurrencyConditionChecker.isPassedChecks;
				}
			}
		}

		private void Compare()
		{
			foreach (var group in concurrencyGroups)
			{
				group.CompareItems();
			}
		}
		private void Sort()
		{
			foreach (var group in concurrencyGroups)
			{
				group.Sort();
			}
		}
		private void Eliminate()
		{
			foreach (var group in concurrencyGroups)
			{
				group.Eliminate();
			}
		}
		/// <summary>
		///
		/// </summary>
		/// <remarks>
		/// <see cref="ConcurrencyRules.OnlyWith{T}"/> is executed at <see cref="CheckConditions"/> as part of <see cref="ConcurrencyConditionChecker"/>
		/// </remarks>
		private void Pullout()
		{
			foreach (var item in items)
			{
				if (item.isActivated)
				{
					foreach (var with in item.concurrencyRules.with)
					{
						if (!with.isActivated)
						{
							item.isActivated = false;
							goto NEXT0;
						}
					}
					item.isWith = true;

					NEXT0:
					foreach (var without in item.concurrencyRules.without)
					{
						if (without.isActivated)
						{
							item.isActivated = false;
							goto NEXT2;
						}
					}
					item.isWithout = true;
					NEXT2: { }
				}
			}
		}

		private void Finish()
		{
			foreach (var item in items)
			{
				item.FinishConcurrency();
			}
		}

		private void Refresh()
		{
			foreach (var group in concurrencyGroups)
			{
				group.Clean();
			}
			foreach (var item in items)
			{
				item.Clean();
			}
		}
	}
}