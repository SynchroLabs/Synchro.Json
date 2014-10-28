using System;
using NUnit.Framework;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Synchro.Json.Test
{
	[TestFixture()]
	public class ParserTests
	{
		private static void TestRoundtrip(string expectedJson, object expectedObject)
		{
			object parsedObject = Parser.ParseValue(new StringReader(expectedJson));
			var writer = new StringWriter();

			Assert.AreEqual(expectedObject, parsedObject);

			Writer.WriteValue(writer, parsedObject);

			Assert.AreEqual(expectedJson, writer.ToString());
		}

		[Test()]
		public void TestParseString()
		{
			TestRoundtrip("\"abc\"", "abc");
		}

		[Test()]
		public void TestParseStringEscapes()
		{
			TestRoundtrip(@"""\""\\\/\b\f\n\r\t\u20AC""", "\"\\/\b\f\n\r\t\u20AC");
		}

		[Test()]
		public void TestParseInteger()
		{
			TestRoundtrip("0", 0);
			TestRoundtrip(string.Format("{0}", int.MaxValue), int.MaxValue);
			TestRoundtrip(string.Format("{0}", int.MinValue), int.MinValue);
		}

		[Test()]
		public void TestParseArray()
		{
			TestRoundtrip("[]", new object[] {});
			TestRoundtrip("[0]", new object[] { 0 });
			TestRoundtrip("[\"abc\"]", new object[] { "abc" });
			TestRoundtrip("[0,\"abc\"]", new object[] { 0, "abc" });
			TestRoundtrip("[0,\"abc\",[1,\"def\"]]", new object[] { 0, "abc", new object[] { 1, "def" } });
		}

		[Test()]
		public void TestParseObject()
		{
			TestRoundtrip("{}", new JsonObject());
			TestRoundtrip(
				"{\"foo\":0,\"bar\":\"kitty\",\"baz\":[8,\"dog\"]}",
				new JsonObject() {
					{ "foo", 0 },
					{ "bar", "kitty" },
					{ "baz", new object[] { 8, "dog" } }
				}
			);
		}

		[Test()]
		public void TestParseObjectWithWhitespace()
		{
			Assert.AreEqual(new JsonObject(), Parser.ParseValue(new StringReader("{}")));
			Assert.AreEqual(
				new JsonObject()
				{
					{ "foo", 0 },
					{ "bar", "kitty" },
					{ "baz", new object[] { 8, "dog" } }
				},
				Parser.ParseValue(new StringReader("  {  \"foo\"  :  0  ,  \"bar\"  :  \"kitty\"  ,  \"baz\"  :  [  8  ,  \"dog\"  ]  }  ")));
		}

		[Test()]
		public void TestParseBoolean()
		{
			TestRoundtrip("true", true);
			TestRoundtrip("false", false);
		}

		[Test()]
		public void TestParseNull()
		{
			TestRoundtrip("null", null);
		}

		[Test()]
		[ExpectedException(typeof(IOException))]
		public void TestUnterminatedString()
		{
			Parser.ParseValue(new StringReader("\"abc"));
		}

		[Test()]
		public void TestComments()
		{
			var jsonWithComments = @"
// This is a comment

{
	// The foo element is my favorite

	""foo""  :  0,
	""bar""  :  ""kitty"",

	// The baz element, he's OK also

	""baz""  :  [  8  ,  ""dog""  ]
}
";
			Assert.AreEqual(
				new JsonObject()
				{
					{ "foo", 0 },
					{ "bar", "kitty" },
					{ "baz", new object[] { 8, "dog" } }
				},
				Parser.ParseValue(new StringReader(jsonWithComments)));
		}

		[Test()]
		public void TestParseDouble()
		{
			TestRoundtrip("0.001", .001);
			TestRoundtrip("6.02E+23", 6.02E+23);
		}

		[Test()]
		public void TestParseDoubleCrazyLocale()
		{
			var crazyCulture = new CultureInfo("en-US");
			var oldCulture = Thread.CurrentThread.CurrentCulture;

			crazyCulture.NumberFormat.NumberDecimalSeparator = "Z";

			Thread.CurrentThread.CurrentCulture = crazyCulture;
			try
			{
				TestRoundtrip("0.001", .001);
				TestRoundtrip("6.02E+23", 6.02E+23);
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = oldCulture;
			}
		}
	}
}

