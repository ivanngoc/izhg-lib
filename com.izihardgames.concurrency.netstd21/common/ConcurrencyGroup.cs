using IziHardGames.Libs.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.Concurrency
{

	/// <summary>
	/// Groups can share same <see cref="ConcurrencyItem"/> but there is must not be cross links (relations) and duplicate relations and same concurrency source.
	/// </summary>
	public class ConcurrencyGroup
	{
		public int idGroupe;
		public List<ConcurrencyItem> items = new List<ConcurrencyItem>();
		protected List<ConcurrencyItem> bakedOrder = new List<ConcurrencyItem>();
		/// <summary>
		/// Источгник/провайдер конурентного ресурса. Например канал инпута клавиши Enter слюбым состоянием (нажатие, отжатие, зажатие и т.д.)
		/// </summary>
		protected object concurrencySource;


		public ConcurrencyGroup(int idGroupe)
		{
			this.idGroupe = idGroupe;
		}

		internal void Add(ConcurrencyItem item)
		{
			items.Add(item);
		}

		/// <summary>
		/// Сравнение <see cref="ConcurrencyItem"/> между собой. Особенность в том что каждый <see cref="ConcurrencyItem.bind"/> имеет свой тип и необходимо выполнить 
		/// сравнение и сортировку для разнотипных объектов которые не обязательно имеют интерфейс <see cref="IComparable"/> или <see cref="IComparable{T}"/>.
		/// Для эти целей вводится <see cref="ConcurrencyItem.priority"/> а сортировка выполняется с помощью <see cref="ConcurrencyRules"/>
		/// </summary>
		internal void CompareItems()
		{
			throw new NotImplementedException();
		}
		internal void Sort()
		{
			SortingIList.SortSelectionAscending<List<ConcurrencyItem>, ConcurrencyItem, int>(items, x => x.order);
		}


		internal void Eliminate()
		{
			foreach (var item in items)
			{
				if (item.isActivated)
				{
					// отключение происходит по иерархии. Если объект выше отключается значит объекты ниже должны быть отключены.
					// Поэтому сначана выключаем все кто находится ниже текущего item
					foreach (var supress in item.concurrencyRules.toSuppress)
					{
						if (supress.isActivated)
						{
							supress.isEliminated = true;
						}
					}
					// затем проверяем нужно выключить ли сам item
					foreach (var yield in item.concurrencyRules.yeildTo)
					{
						if (yield.isActivated)
						{
							item.isEliminated = true;
							break;
						}
					}
				}
			}
		}

		internal void Clean()
		{


		}

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// </remarks>
		/// A B C. A->B->C. Сначала вставляется С. Затем A. получается CA. потом вставляется B. Тут ситуация противоречивости и нужно восстанавливать порядок.
		/// Сколько есть подходов как из неупорядоченного списка с нестрогим (относительным) порядком у элементов создать упорядоченный (Стремиться нужно к минимального числу иттераций)
		/// 1. самый простой это втыкать по порядку а затем вносить обновления до тех пор пока не останется не определенностей.
		/// 2. селективный - по графу определять связи и вставлять. если связи кончаются а элементы остаются (в графе существует несколько контуров без связей) то брать любой следующий.
		/// 3. взвешанный??? как нибудь расчитывать приоритет а затем сортировать список по этому приоритету.
		/// 
		/// Исходя из перечисленного нужно определить что ситуации когда в списке есть несвязные группы быть не должно. Но на практике такое возможно? По идее нет связи то нет конкуренции и нечего было добавлять элемент в группу
		/// Все это можно обойти в период формирования правил
		public void ApplyRules()
		{
			/// найти среднюю точку для вставки между всеми <see cref="ConcurrencyRules.beforeThis"/> и <see cref="ConcurrencyRules.afterThis"/>. 
			/// По умолчанию будет крайняя правая досиупеая позиция если наример между точкой before и after будет 5 элементов то вставка будет за 5ым. и все они должны иметь одинаковый приоритет то есть неважно кто среди этих 5 и этого вставочного объекта будет первый. Их можно переставлять на этом промежутке хоть как.
			foreach (var itemToInsert in items)
			{
				itemToInsert.priority = bakedOrder.LastOrDefault()?.priority ?? 0;
				// сначала ищем крайний правый уже имеющийся в списке элемент за которым гарантировано должен быть вставляемый объект
				foreach (var itemToCompare in itemToInsert.concurrencyRules.beforeThis)
				{
					if (bakedOrder.Contains(itemToCompare))
					{
						if (itemToCompare.priority < itemToInsert.priority)
						{
							itemToInsert.priority = itemToCompare.priority + 1;
						}
					}
				}

				foreach (var itemToCompare in itemToInsert.concurrencyRules.afterThis)
				{
					if (bakedOrder.Contains(itemToCompare))
					{
						if (itemToInsert.priority > itemToCompare.priority)
						{
							itemToInsert.priority = itemToCompare.priority - 1;
						}
					}
				}

				// by default insert as last
				int insertPosition = bakedOrder.Count;

				for (int i = 0; i < bakedOrder.Count; i++)
				{
					if (itemToInsert.priority < bakedOrder[i].priority)
					{
						insertPosition = i;
						break;
					}
				}
				bakedOrder.Insert(insertPosition, itemToInsert);
			}
		}

	}
}