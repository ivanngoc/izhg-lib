using System;

namespace IziHardGames.Libs.NonEngine.Scheming.Datas
{
	[Serializable]
	public class DataSchemaItem
	{
		public int id;
		/// <summary>
		/// index as id in <see cref="DataSchema.values"/>
		/// </summary>
		public int idValue;
		public int idSchema;
		public int idSchemaGroupe;

		/// <summary>
		/// Положение в вертикальной последовательности. В вертикальной иерархии не может быть одинаковых <see cref="iKey"/>
		/// </summary>
		public int iKey;
		/// <summary>
		/// Положение в вертикальной последовательности. По вертикали нет иерархии поэтому <see cref="jKey"/> может совпадать. 
		/// Это означает что элементы равнозначны. На примере методов это будет гооворить о том что методы могут быть запущены паралельно.
		/// Может также быть отступом, по которому определяется вложенность (как Tab при наборе текста или indentation), но в отличие от <see cref="iKeyParent"/> не определяет четкую иерархию.
		/// По сути на выбор можно создать иерархию вложенности либо по <see cref="jKey"/> либо по <see cref="iKeyParent"/>
		/// </summary>
		public int jKey;
		/// <summary>
		/// Вложенность по вертикальной иерархии. В массиве вертикальной иерархии все объекты будут находится последовательно независимо от вложенности. 
		/// Этот ключ позволяет определить иерархию (вложенность) так как Root object всегда один.
		/// Объекты имеющие одинаковый <see cref="iKeyParent"/> будут считаться за siblings.
		/// </summary>
		public int iKeyParent;
		public int channel;

		public DataSchemaItem(int type, int schema, int idGroupe)
		{
			idValue = type;
			idSchema = schema;
			idSchemaGroupe = idGroupe;
		}
	}
}