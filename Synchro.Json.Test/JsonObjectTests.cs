using System;
using NUnit.Framework;

namespace Synchro.Json.Test
{
	[TestFixture()]
	public class JsonObjectTests
	{
		[Test()]
		public void TestInteger()
		{
			JsonObject stuff = new JsonObject();

			stuff["foo"] = 7;

			Assert.AreEqual(7, stuff["foo"]);
		}

		[Test()]
		public void TestString()
		{
			JsonObject stuff = new JsonObject();

			stuff["bar"] = "kitty";

			Assert.AreEqual("kitty", stuff ["bar"]);
		}

		[Test()]
		public void TestArray()
		{
			JsonObject stuff = new JsonObject();

			stuff["baz"] = new object[] { 8, "dog" };

			Assert.AreEqual(new object[] { 8, "dog" }, stuff ["baz"]);
			Assert.AreEqual(8, ((object[]) stuff["baz"])[0]);
			Assert.AreEqual("dog", ((object[]) stuff["baz"])[1]);
		}
	}
}
