using System;
using NUnit.Framework;
using System.IO;

namespace Synchro.Json.Test
{
	[TestFixture()]
	public class ParserTests
	{
		[Test()]
		public void TestParseString()
		{
			Assert.AreEqual("abc", Parser.ParseValue(new StringReader("\"abc\"")));
		}

		[Test()]
		public void TestParseStringEscapes()
		{
			Assert.AreEqual("\"\\/\b\f\n\r\t\u20AC", Parser.ParseValue(new StringReader(@"""\""\\\/\b\f\n\r\t\u20AC""")));
		}

		[Test()]
		public void TestParseInteger()
		{
			Assert.AreEqual(0, Parser.ParseValue(new StringReader("0")));
			Assert.AreEqual(int.MaxValue, Parser.ParseValue(new StringReader(string.Format("{0}", int.MaxValue))));
			Assert.AreEqual(int.MinValue, Parser.ParseValue(new StringReader(string.Format("{0}", int.MinValue))));
		}

		[Test()]
		public void TestParseArray()
		{
			Assert.AreEqual(new object[] {}, Parser.ParseValue(new StringReader("[]")));
			Assert.AreEqual(new object[] { 0 }, Parser.ParseValue(new StringReader("[0]")));
			Assert.AreEqual(new object[] { "abc" }, Parser.ParseValue(new StringReader("[\"abc\"]")));
			Assert.AreEqual(new object[] { 0, "abc" }, Parser.ParseValue(new StringReader("[0,\"abc\"]")));
			Assert.AreEqual(new object[] { 0, "abc", new object[] { 1, "def" } }, Parser.ParseValue(new StringReader("[0,\"abc\",[1,\"def\"]]")));
		}

		[Test()]
		public void TestParseObject()
		{
			Assert.AreEqual(new JsonObject(), Parser.ParseValue(new StringReader("{}")));
			Assert.AreEqual(
				new JsonObject()
				{
					{ "foo", 0 },
					{ "bar", "kitty" },
					{ "baz", new object[] { 8, "dog" } }
				},
				Parser.ParseValue(new StringReader("{\"foo\":0,\"bar\":\"kitty\",\"baz\":[8,\"dog\"]}")));
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
	}
}

