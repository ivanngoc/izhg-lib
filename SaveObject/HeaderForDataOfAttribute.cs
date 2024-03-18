using System;
using System.Runtime.InteropServices;

namespace IziHardGames.Libs.NonEngine.SaveObject
{
	/// <summary>
	/// Specify position of attribute at <see cref="ContainerForDatasOfAttributes.datas"/>. Also specify <see cref="idType"/> for pick Type to interpetate that byte range.
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	public struct HeaderForDataOfAttribute
	{
		/// <summary>
		/// TypeToInterpretate
		/// </summary>
		[FieldOffset(0)] public int idType;
		/// <summary>
		/// index in <see cref="ContainerForDatasOfAttributes.datas"/> from which data for given attribute is started
		/// </summary>
		[FieldOffset(4)] public int indexStart;
		/// <summary>
		/// Length of data-struct
		/// </summary>
		[FieldOffset(8)] public int length;

		[FieldOffset(12)] public int idHeader;
	}
}