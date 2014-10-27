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
			Assert.AreEqual("abc", Parser.ParseString(new StringReader("\"abc\"")));
		}

		[Test()]
		public void TestParseStringEscapes()
		{
			Assert.AreEqual("\"\\/\b\f\n\r\t\u20AC", Parser.ParseString(new StringReader(@"""\""\\\/\b\f\n\r\t\u20AC""")));
		}
	}
}

