using System;
using NUnit.Framework;
using System.IO;

namespace Synchro.Json.Test
{
	[TestFixture()]
	public class WriterTests
	{
		private static void TestRoundtrip(string json)
		{
			object parsedObject = Parser.ParseValue(new StringReader(json));
			var writer = new StringWriter();

			Writer.WriteValue(writer, parsedObject);

			Assert.AreEqual(json, writer.ToString());
		}

		[Test()]
		public void TestWriteValue()
		{
			TestRoundtrip("{\"foo\":0,\"bar\":\"kitty\",\"baz\":[8,\"dog\"]}");
		}
	}
}

