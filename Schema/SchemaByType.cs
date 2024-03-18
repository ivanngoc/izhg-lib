using IziHardGames.Libs.NonEngine.Scheming.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.Scheming
{
	public class SchemaByType
	{
		public List<SchemaByTypeItem> schemaItems = new List<SchemaByTypeItem>();
		public List<SchemaByTypeGroupe> schemaGroupes = new List<SchemaByTypeGroupe>();

		public DataSchema Export()
		{
			throw new NotImplementedException();
		}

		public void AddItem<TType>(int iKeyParent, int jKey)
		{
			int iKey = default;
			SchemaByTypeItem schemaItem = new SchemaByTypeItem();
		}

		public void After<TTarget, TAfter>()
		{
			throw new NotImplementedException();
		}

		public SchemaByTypeItem GetByType<TType>()
		{
			return schemaItems.First(x => x.type == typeof(TType));
		}
	}

	public class SchemaByTypeGroupe
	{
		public List<SchemaByTypeItem> items;
	}
	public class SchemaByTypeItem
	{
		public Type type;
		public SchemaByType schema;
		public SchemaByTypeGroupe schemaGroupe;

		public SchemaByTypeItem parent;
		public SchemaByTypeItem next;
		public SchemaByTypeItem previous;

		public void Before<T>()
		{
			throw new NotImplementedException();
		}
		public DataSchemaItem After<T>()
		{
			//List<SchemaItem> schemaItems = schema.tempItems;
			/////not <see cref="Type.IsAssignableFrom(Type)"/>
			//var existed = schemaItems.FirstOrDefault(x => x.type == typeof(T));

			//if (existed == null)
			//{
			//	int index = schemaItems.IndexOf(this);
			//	if (index < 0) throw new ArgumentOutOfRangeException("В списке нет этого объекта. После создания этого объекта он должен быть помещен в список схемы");
			//	existed = new SchemaItem(typeof(T), schema);
			//	schemaItems.Insert(index + 1, existed);
			//}
			//return this;
			throw new NotImplementedException();
		}
	}
}