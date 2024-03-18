#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IziHardGames.Develop.MenuItems
{
	public class UnityExampleMenuItems
	{
		/*
        % – CTRL on Windows / CMD on OSX
        //# – Shift
        & – Alt
        LEFT/RIGHT/UP/DOWN – Arrow keys
        F1…F2 – F keys
        HOME, END, PGUP, PGDN
        */

		/// <summary>
		/// <see cref="https://learn.unity.com/tutorial/editor-scripting#5c7f8528edbc2a002053b5f9"/>
		/// </summary>
		[MenuItem("Example*/TopMenuItem")] // ctrl+shift+a
										   //[MenuItem("Example*/TopMenuItem %#a")] // ctrl+shift+a
		private static void TopMenuItem()
		{
			Debug.Log($"{typeof(UnityExampleMenuItems)}.{typeof(UnityExampleMenuItems).GetMethod("TopMenuItem", BindingFlags.NonPublic | BindingFlags.Static).Name}");
		}
	}
}
#endif