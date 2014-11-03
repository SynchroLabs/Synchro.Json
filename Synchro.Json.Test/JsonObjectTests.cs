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

		[Test()]
		public void TestDeepClone()
		{
			JsonObject stuff = new JsonObject() {
				{ "a", new JsonObject() {
						{ "b", new JsonObject() {
								{ "c", "d" } } } }
				},
				{ "e", new object[] { new JsonObject() { { "f", "g" } }, "h" } }
			};
			JsonObject duplicateStuff = new JsonObject() {
				{ "a", new JsonObject() {
						{ "b", new JsonObject() {
								{ "c", "d" } } } }
				},
				{ "e", new object[] { new JsonObject() { { "f", "g" } }, "h" } }
			};
			var stuffClone = stuff.DeepClone();

			Assert.AreEqual(duplicateStuff, stuffClone);

			Assert.AreNotSame(stuff["a"], stuffClone["a"]);
			Assert.AreNotSame(stuff["e"], stuffClone["e"]);
			Assert.AreNotSame(((object[])stuff["e"])[0], ((object[])stuffClone["e"])[0]);
			Assert.AreNotSame(((JsonObject) stuff["a"])["b"], ((JsonObject) stuffClone["a"])["b"]);
		}
	}
}
