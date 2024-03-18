using System;

namespace IziHardGames.Libs.NonEngine.Concurrency
{
	/// <summary>
	/// Результат создания объекта <see cref="ConcurrencyItem"/> через <see cref="ConcurrencyBuilder"/>
	/// </summary>
	public class ConcurrencyBuild
	{
		private readonly ConcurrencyBuilder concurrencyBuilder;
		private readonly ConcurrencyItem concurrencyItem;

		public ConcurrencyBuild(ConcurrencyBuilder concurrencyBuilder)
		{

		}
	}

	/// <summary>
	/// Строитель-провайдер API разрешения конкуренции. Основная функция это определения порядка выполнения и определение состояния исключения/включения на основе установленных правил для конкурирующих объектьов
	/// </summary>
	public class ConcurrencyBuilder
	{
		public static ConcurrencyBuilder Default { get; set; }
		public readonly ConcurrencyResolver concurrencyResolver;
		//private readonly DataSchema schema;

		public ConcurrencyBuilder(ConcurrencyResolver concurrencyResolver)
		{
			this.concurrencyResolver = concurrencyResolver;
			//this.schema = new DataSchema();
		}

		public virtual ConcurrencyItem Build(ConcurrencyRules concurrencyRules, object target)
		{
			ConcurrencyGroup concurrencyGroup = concurrencyResolver.GetOrCreateGroupe(concurrencyRules);
			ConcurrencyItem concurrencyItem = ConcurrencyItem.Create(concurrencyGroup, concurrencyRules, target);
			concurrencyResolver.AddItem(concurrencyItem);
			return concurrencyItem;
		}
		public (ConcurrencyRules, ConcurrencyItem) AddAgent(ConcurrencyRules interactionRule, Action action, object bind)
		{
			throw new NotImplementedException();
		}
		public ConcurrencyRules AddAgent(ConcurrencyItem item)
		{
			throw new NotImplementedException();
		}
#if DEBUG
		public static void TestV0()
		{
			//Action action0 = default;
			//Action action1 = default;
			//Action action2 = default;

			//ConcurrencyGroup group = new ConcurrencyGroup();

			//ConcurrencyItem item0 = ConcurrencyItem.Wrap(action0, group);
			//ConcurrencyItem item1 = ConcurrencyItem.Wrap(action1, group);
			//ConcurrencyItem item2 = ConcurrencyItem.Wrap(action2, group);


			//ConcurrencyRules rule0 = ConcurrencyBuilder.Default.AddAgent(item0);
			//ConcurrencyRules rule1 = ConcurrencyBuilder.Default.AddAgent(item1);
			//ConcurrencyRules rule2 = ConcurrencyBuilder.Default.AddAgent(item2);

			//rule0.Before(item1);

			//rule1.After(item0);
			//rule1.Before(item2);

			//rule2.After(item1);

			//item0.ApplyRules();
			//item1.ApplyRules();
			//item2.ApplyRules();
		}
#endif
	}
}