using System;
using System.Collections.Generic;

namespace IziHardGames.Libs.NonEngine.Query.FromCollections
{
	/// <summary>
	/// Выборка из коллекции с постепенным уменьшением результата выборки путем добавления условий выборки.
	/// По факту получается создание двух частей этой коллекции: в первой - не прошедшие условие, а во второй - прошедшие.
	/// Создавать или не создавать буфер коллекции зависит от ситуации. Но предпочтетельней работать с той коллекцией которая предоставляется
	/// </summary>
	public class QueryChain<TItem>
	{
		public IList<TItem> items;
	}
	/// <summary>
	/// Если представить коллекцию как прямую, то каждая выборка будет создавать отрезок правее предыдущей выборки. Сам отрезок будет делиться еще на 2 отрезка: левый и правый.
	/// В правый отрезок будут входить элементы прошедшие выборку. В левый - не прошедшую эту выборку
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	/// <typeparam name="TArg1"></typeparam>
	/// <typeparam name="TArg2"></typeparam>
	public class QueryChain<TItem, TQueryData> : QueryChain<TItem>, IDisposable
		where TQueryData : IQueryData
	{
		/// <summary>
		/// Граница текущего деления <see cref="items"/>. Левее этого значения будут индексы элементов не прошедших последнюю фильтрацию. 
		/// Элементы со значение индекса с этим значением включительно будут считаться как прошедшие последний фильтр.
		/// Индекс с которого начинается эфимерный сегмент прошедших фильтр элементов.
		/// Пока фильтр проходит хотя бы 1 элемент, то это значение будет меньше <see cref="IList{T}.Count"/>.
		/// Если фультрацию <see cref="Filter(TArg1, TArg2, Func{TItem, TArg1, TArg2, bool})"/> не пройдет ни один элемент то этот индекс выйдет за границы массива и будет равен количеству элементов.
		/// </summary>
		public int delimeter;
		public List<TQueryData> queryDatas = new List<TQueryData>();

		public QueryChain(List<TItem> items)
		{
			this.items = items;
		}

		/// <summary>
		/// Perfom additional query.
		/// </summary>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="selector"></param>
		/// <returns></returns>
		public QueryChain<TItem, TQueryData> Filter(ref TQueryData queryData, Func<TItem, TQueryData, bool> selector)
		{
			int countPassed = default;
			int startOfQuery = delimeter;

			for (int i = delimeter; i < items.Count; i++)
			{
				if (selector(items[i], queryData))
				{
					countPassed++;
				}
				else
				{
					items.Swap<IList<TItem>, TItem>(i, delimeter);
					delimeter++;
				}
			}
			queryData.SetIndexOfQueryStart(startOfQuery);
			queryData.SetIndexOfQuerySuccesfulStart(delimeter);
			queryData.SetCountOfQuriedItems(countPassed);
			queryDatas.Add(queryData);
			return this;
		}

		public void Dispose()
		{
			items = default;
			queryDatas.Clear();
		}
	}



	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TItem"></typeparam>
	/// <typeparam name="TArg1"></typeparam>
	/// <typeparam name="TArg2"></typeparam>
	/// <remarks>
	/// Если после каждой выборки сохранять этот объект, то каждый делиметр будет как точка на отрезке. Получится сортировка коллекции по истории выборки, где каждый отрезок будет результатом сложения фильтров.
	/// </remarks>
	public ref struct QueryStruct<TItem, TArg1, TArg2>
	{
		public IList<TItem> items;
		public TArg1 arg1;
		public TArg2 arg2;
		/// <summary>
		/// Индекс с которого начинаются прошедшие выборку элементы
		/// </summary>
		public int delimeter;

		public QueryStruct(List<TItem> items) : this()
		{
			this.items = items;
		}

		public QueryStruct(IList<TItem> items, TArg1 arg1, TArg2 arg2, int delimeter, Func<TItem, TArg1, TArg2, bool> selector)
		{
			this.items = items;
			this.arg1 = arg1;
			this.arg2 = arg2;

			for (int i = delimeter; i < items.Count; i++)
			{
				if (selector(items[i], arg1, arg2))
				{
					items.Swap<IList<TItem>, TItem>(i, delimeter);
					delimeter++;
				}
			}
			this.delimeter = delimeter;
		}
	}
	public ref struct QueryStruct<T>
	{
		public IList<T> items;
		/// <summary>
		/// Индекс с котрого начинаются прошедшие фильтр элементы
		/// </summary>
		public int delimeter;

		public QueryStruct(IList<T> items)
		{
			this.items = items;
			delimeter = default;
		}

		public ref QueryStruct<T> Select(Func<T, bool> selector)
		{
			throw new NotImplementedException();
		}

		public static ref QueryStruct<T> CreateQuery(IList<T> items)
		{
			QueryStruct<T> queryStruct = new QueryStruct<T>(items);
			throw new NotImplementedException();
			//return ref queryStruct;
		}
	}

	public interface IQueryData
	{
		public void SetIndexOfQueryStart(int index);
		public void SetIndexOfQuerySuccesfulStart(int index);
		public void SetCountOfQuriedItems(int count);
	}
}