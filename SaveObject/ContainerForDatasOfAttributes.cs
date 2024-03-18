using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.SaveObject
{
	/// <summary>
	/// Контейнер для разнотипных объектов. Общая куча данных представлена в виде массива байт <see cref="datas"/>. 
	/// Интерператация объектов и mapping массива данных происходит по массиву заголовков <see cref="headers"/>
	/// </summary>
	[Serializable]
	public class ContainerForDatasOfAttributes
	{
		/// <summary>
		/// Последовательность структур в виде байтов. Границы объектов определяют заголовки <see cref="headers"/>
		/// </summary>
		public byte[] datas;
		public HeaderForDataOfAttribute[] headers;
		public ref readonly HeaderForDataOfAttribute this[int index]
		{
			get => ref headers[index];
		}

		public void Initilize(int dataLength, int countOfHeaders)
		{
			datas = new byte[dataLength];
			headers = new HeaderForDataOfAttribute[countOfHeaders];
		}

		public int FindHeaderIndexById(int id)
		{
			for (int i = 0; i < headers.Length; i++)
			{
				if (headers[i].idHeader == id) return i;
			}
			throw new ArgumentOutOfRangeException($"No header with id {id}");
		}
		public ref readonly HeaderForDataOfAttribute FindHeaderById(int id)
		{
			for (int i = 0; i < headers.Length; i++)
			{
				if (headers[i].idHeader == id) return ref headers[i];
			}
			throw new ArgumentOutOfRangeException($"No header with id {id}");
		}
		public T FindValueByHeaderId<T>(int id) where T : unmanaged
		{
			for (int i = 0; i < headers.Length; i++)
			{
				if (headers[i].idHeader == id)
				{
					return GetValue<T>(in this[i]);
				}
			}
			throw new ArgumentOutOfRangeException();
		}
		public T GetValue<T>(int indexOfHeader) where T : unmanaged
		{
			return GetValue<T>(in this[indexOfHeader]);
		}

		public T GetValue<T>(in HeaderForDataOfAttribute header) where T : unmanaged
		{
			var span = new Span<byte>(datas,header.indexStart, header.length);
			return MemoryMarshal.Cast<byte, T>(span)[0];
		}

		public T GetValue<T>(int indexFrom, int length) where T : unmanaged
		{
			var span = new Span<byte>(datas,indexFrom, length);
			return MemoryMarshal.Cast<byte, T>(span)[0];
		}

		public void SetValueById<T>(T value, int id) where T : unmanaged
		{
			int index = FindHeaderIndexById(id);
			SetValueByIndex<T>(value, index);
		}

		public void SetValueByIndex<T>(T value, int indexOfHeader) where T : unmanaged
		{
			ref readonly var header = ref headers[indexOfHeader];
			var span = new Span<byte>(datas,header.indexStart, header.length);
			MemoryMarshal.Cast<byte, T>(span)[0] = value;
		}

		protected static int CalculateDataLength(ICollection<HeaderForDataOfAttribute> headers)
		{
			int lengthTotal = default;

			foreach (var header in headers)
			{
				lengthTotal += header.length;
			}
			return lengthTotal;
		}



#if UNITY_EDITOR || DEBUG
		public static void Test()
		{
			ContainerForDatasOfAttributes containerForDatasOfAttributes = new ContainerForDatasOfAttributes();
			HeaderForDataOfAttribute header = new HeaderForDataOfAttribute();
			long val = containerForDatasOfAttributes.GetValue<long>(header.indexStart, header.length);
		}
#endif
	}
}