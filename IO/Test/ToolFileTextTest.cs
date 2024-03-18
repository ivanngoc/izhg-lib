#pragma warning disable
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace IziHardGames.IO
{
	public static class ToolFileTextTest
	{

		public static void Do()
		{
			string s0 = "0000  000";
			string s1 = "1111";
			string s2 = "2222";
			string s3 = "3333";
			string s4 = "4444";
			string s5 = "5555";
			string s6 = "6666";
			string s7 = "7777";
			string s8 = "8888";

			string dir = Path.Combine(Directory.GetCurrentDirectory(), "ToolFileTextTest");

			string filename = "ExampleTxt.txt";

			var abs = ToolFileText.PathSet(dir, filename);

			if (File.Exists(abs))
			{
				ToolFile.Clean(dir, filename);
			}

			ToolFile.Create(dir, filename);

			Console.WriteLine(abs);

			ToolFileText.LineAppend(s1);
			Console.WriteLine("s1");
			ToolFileText.LineAppend(s2);
			Console.WriteLine("s2");
			ToolFileText.LineAppend(s3);
			Console.WriteLine("s3");
			ToolFileText.LinePrepend(s0);
			Console.WriteLine("s0");
		}

		public static void TestSearch()
		{
			string dir = Path.Combine(Directory.GetCurrentDirectory(), "ToolFileTextTest");

			string filename = "ExampleTxt.txt";

			var abs = ToolFileText.PathSet(dir, filename);

			ToolFile.Create(dir, filename);

			Console.WriteLine(abs);

			if (StreamTextReader.LineFindThatContain(dir, filename, "0x00000009", out int index))
			{
				FileStream fileStream = File.OpenRead(Path.Combine(dir, filename));

				StreamTextReader.TrySeekToLine(fileStream, index);

				byte[] b = new byte[4096];

				int read = 10;

				Console.WriteLine($"index {index} | Length: {fileStream.Length} : Pos {fileStream.Position}");

				//int num = fileStream.Read(b, 0, read);

				//Console.WriteLine($"Content [{Encoding.UTF8.GetString(b, 0, num)}]");
				fileStream.Dispose();

				Console.WriteLine($"Content [{File.ReadLines(abs).ElementAt(index)}]");

				Console.WriteLine($"index: {index}");
			}
		}

		public static void TestInsert()
		{
			string dir = Path.Combine(Directory.GetCurrentDirectory(), "ToolFileTextTest");

			string filename = "ExampleTxt.txt";

			var abs = ToolFileText.PathSet(dir, filename);

			if (File.Exists(abs))
			{
				ToolFile.Clean(dir, filename);
			}

			ToolFile.Create(dir, filename);

			Console.WriteLine(abs);

			ToolFileText.LineInsert("111", 0, Encoding.UTF8);
			ToolFileText.LineInsert("00000", 1, Encoding.UTF8);
		}

		public static void TestSeekNewLine()
		{
			string dir = Path.Combine(Directory.GetCurrentDirectory(), "ToolFileTextTest");

			string filename = "ExampleTxt.txt";

			var abs = ToolFileText.PathSet(dir, filename);

			FileStream fileStream = File.Open(abs, FileMode.Open);

			byte[] vs = new byte[33554432];

			int lineCount = default;

			while (true)
			{
				if (Console.ReadKey() != default)
				{
					long linestart = default;

					if (StreamTextReader.TryGetIndexOfNewLineChar(fileStream, vs, fileStream.Position, out long pos))
					{
						fileStream.Seek(linestart, SeekOrigin.Begin);

						int read = fileStream.Read(vs, 0, (int)(pos - linestart));

						lineCount++;

						Console.WriteLine($"{lineCount}|" + $"|{Encoding.UTF8.GetString(vs, 0, read)}|");
					}
					else
					{
						fileStream.Seek(linestart, SeekOrigin.Begin);

						int read = fileStream.Read(vs, 0, (int)(fileStream.Length - linestart));

						Console.WriteLine($"last {lineCount}|" + " " + Encoding.UTF8.GetString(vs, 0, read));
					}
					linestart = pos;
				}
			}

			fileStream.Dispose();
		}
	}
}