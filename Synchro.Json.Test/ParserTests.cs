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
	}
}

