using System;

namespace IziHardGames.Libs.NonEngine.Graphs.Base
{
	/// <summary>
	/// Граф по научному. Служит базой для создания типов для графа для конкретных ситуаций
	/// Источник: Элементы теории графа (учебное пособие). Пензинский государственный университет 2007. Домнин Л.Н. => Далее [0*].
	/// </summary>
	public class Base
	{

	}

	public class Graph
	{
		public Vertici[] verticis;
		public Edge[] edges;

		/// <summary>
		/// Порядок графа. Совпадает с количеством вершин. 
		/// </summary>
		public int magnitude;

		public VerticiesSet[] verticiesSets;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="graph"></param>
		/// <returns></returns>
		/// [0*] стр 7
		public bool IsIsomorphicTo(Graph graph)
		{
			throw new NotImplementedException();
		}
	}
	/// <summary>
	/// Подграф
	/// </summary>
	/// [0*] стр 10
	public class SubGraph
	{

	}
	/// <summary>
	/// Связные графы с минимальным количеством ребер образует класс так называемых деревьев
	/// </summary>
	/// [0*] стр 14 
	public class Tree : Graph
	{

	}
	/// <summary>
	/// Частный случай дерева. Простая цепь. Чередующаяся последовательность вершин и ребер. В последовательности вершин могут быть повторения
	/// </summary>
	/// [0*] стр 14 
	public class Chain : Tree
	{

	}

	/// <summary>
	/// В последовательности цепи не могут быть повторения
	/// </summary>
	public class ChainSimple : Chain
	{

	}
	/// <summary>
	/// Замкнутая простая цепь
	/// </summary>
	public class Cycle : ChainSimple
	{

	}
	/// <summary>
	/// Вершина  графа
	/// </summary>
	public struct Vertici
	{
		/// <summary>
		/// Степень вершины. Определяет инцидентность вершины или по другому сколько ребер примыкает к этой вершине
		/// </summary>
		public int degree;
	}
	/// <summary>
	/// Дополнительные данные о вершине графа
	/// </summary>
	public struct VerticiAtrtributes
	{
		/// <summary>
		/// Вершина является изолированной если <see cref="Vertici.degree"/>==0
		/// </summary>
		/// [0*] стр 6
		public bool isIsolated;
		/// <summary>
		/// Вершина является концевой если <see cref="Vertici.degree"/>==1
		/// </summary>
		/// [0*] стр 6 
		public bool isEndpoint;
	}
	/// <summary>
	/// Ребро/Грань графа
	/// </summary>
	public struct Edge
	{

	}
	public struct EdgeAttributes
	{
		/// <summary>
		/// Ребро примыкающее к концевой вершине также называется концевым. <see cref="VerticiAtrtributes.isEndpoint"/>
		/// </summary>
		/// [0*] стр 6
		public bool isEndpoint;
	}


	[Flags]
	public enum EGraphCharacteristics
	{
		All = -1,
		None = 0,
		/// <summary>
		/// Дополнение графа
		/// </summary>
		Adjustment,
		/// <summary>
		/// Самодополнительный граф
		/// </summary>
		SelfAdjustment,
		/// <summary>
		/// Регулярный или однородный граф. Все вершины имеют одинаковую степень
		/// </summary>
		Regular,
		/// <summary>
		/// Любой кубический граф (все вершины 3ей степени) имеет четное число вершин.
		/// </summary>
		Cubic,
		/// <summary>
		/// Двудольный граф (линия разделения должна проходить по всем ребрам таким образом чтобы образовалось два подграфа каждый из которых будет состоять из 
		/// вершин противоположных концов разделяемых ребер, но стоит иметь ввиду что из за изоморфности линия разделения будет не линейной)
		/// </summary>
		Bipartite,

		MultiGraph,
		PseudoGraph,
	}

	/// <summary>
	/// Степенная послежовательность графа
	/// </summary>
	/// [0*] стр 6 
	public class DegreeSequenceOfGraph
	{
		public int[] degreees;
	}

	/// <summary>
	/// Множество вершин
	/// </summary>
	public class VerticiesSet
	{

	}
}