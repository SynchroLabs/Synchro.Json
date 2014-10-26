using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Synchro.Json.Test
{
	class JsonObject : Dictionary<string,object>
	{
	}

	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase ()
		{
			JsonObject stuff = new JsonObject();
			stuff["foo"] = 7;
			stuff["bar"] = "kitty";
			stuff["baz"] = new object[] { 8, "dog" };
			Assert.AreEqual(7, stuff["foo"]);
			Assert.AreEqual("kitty", stuff ["bar"]);
			Assert.AreEqual(new object[] { 8, "dog" }, stuff ["baz"]);
			Assert.AreEqual(8, ((object[]) stuff["baz"])[0]);
			Assert.AreEqual("dog", ((object[]) stuff["baz"])[1]);
		}
	}
}

