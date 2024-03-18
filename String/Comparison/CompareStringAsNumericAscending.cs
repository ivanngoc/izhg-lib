using System;
using System.Collections.Generic;
using System.Linq;

namespace IziHardGames.Libs.NonEngine.String
{
	public class CaseInsensitiveAttribute : Attribute { }
	public class CaseSensitiveAttribute : Attribute { }

	public static class TestComparer
	{
		public static string[] alphabetEng = new string[]
		{
			"a","A","b","B","c","C","d","D","e","E","f","F","g","G","h","H","i","I","j","J","k","K","l","L","m","M","n","N","o","O","p","P","q","Q","r","R","s","S",
			"t","T","u","U","v","V","w","W","x","X","y","Y","z","Z"
		};
		public static string[] alphabetRuLowerCase = new string[]
		{
			"а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я"
		};
		public static string[] alphabetRuUpperCase = new string[]
		{
			"А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я"
		};
		public static string[] digits = new string[]
		{
			"0","1","2","3","4","5","6","7","8","9"
		};

		public static void Test()
		{
			string[] joins = alphabetEng.Union(alphabetRuLowerCase).Union(alphabetRuUpperCase).Union(digits).ToArray();
			Shuffle(joins);

			string[] result = joins.OrderBy(x=>x, new CompareStringAsNumericAscending(false)).ToArray();
			Console.WriteLine(result.Aggregate((x, y) => x + Environment.NewLine + y));
		}

		private static void Shuffle<T>(T[] values)
		{
			Random random = new Random();

			for (int i = 0; i < values.Length; i++)
			{
				int randomI =  random.Next(0, values.Length - i);
				var temp = values[i];
				values[i] = values[randomI];
				values[randomI] = temp;
			}
		}
	}

	/// <summary>
	/// Сравнение строк с цифрами не как со строкой а как с цифрой. <br/>
	/// при строчном: "TileAtProect 10">"TileAtProect 2" <br/>
	/// при этом сравнении: "TileAtProect 10"<"TileAtProect 2"<br/>
	/// </summary>
	/// TODO: сравнивать не по символьно цифры а группой цифр как единое число
	public class CompareStringAsNumericAscending : IComparer<string>
	{
		private bool isCaseSensetive;
		public CompareStringAsNumericAscending(bool isCaseSensetive)
		{
			this.isCaseSensetive = isCaseSensetive;
		}

		public int Compare(string left, string right)
		{
			if (left.Length < right.Length)
			{
				for (int i = 0; i < left.Length; i++)
				{
					if (left[i] == right[i]) continue;
					return CompareChar(left[i], right[i]);
				}
			}
			else
			{
				for (int i = 0; i < right.Length; i++)
				{
					if (left[i] == right[i]) continue;
					return CompareChar(left[i], right[i]);
				}
			}
			return 0;
		}

		private int CompareChar(char x, char y)
		{
			if (!isCaseSensetive)
			{
				x = char.ToLower(x);
				y = char.ToLower(y);
			}

			if (x == y) return 0;

			bool leftIsDigit = char.IsDigit(x);
			bool rightIsDigit = char.IsDigit(y);

			if (leftIsDigit && rightIsDigit)
			{
				int numLeft = Convert.ToInt32(x);
				int numRight = Convert.ToInt32(y);

				if (numLeft < numRight)
				{
					return -1;
				}
				if (numLeft > numRight)
				{
					return 1;
				}
			}
			else
			{
				if (char.IsLetter(x) && char.IsLetter(y))
				{
					if (x < y) return -1;
					if (x > y) return 1;
				}
				else
				{
					if (leftIsDigit) return 1;
					if (rightIsDigit) return -1;

					return x.CompareTo(y);
				}
			}
			throw new ArgumentOutOfRangeException();
		}
	}
}
