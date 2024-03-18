//using System;
//using System.Collections;
//using System.Collections.Generic;


//public class TestInteractie
//{
//#if UNITY_EDITOR
//	//[ContextMenu("Test ENUM")]
//	public void TestEnum()
//	{
//		var values = Enum.GetValues(typeof(EEquipmentSlot));

//		for (int i = 0; i < values.Length; i++)
//		{
//			Console.Write($"{(int)values.GetValue(i)}|{values.GetValue(i)}");
//		}
//	}
//#endif

//}

//[Flags]
//public enum EEquipmentSlot
//{
//	None,
//	head,
//	torso,
//	waist,
//	legs,
//	feets,
//	arms,
//	handLeft,
//	handRight,
//	accessory1,
//	accessory2,
//	handBoth = handLeft | handRight,
//	handAny,
//}