using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Synchro.Json
{
	public class JsonObject : Dictionary<string, object>
	{
		public static object DeepCloneOrSame(object _object)
		{
			if (_object is JsonObject)
			{
				return ((JsonObject)_object).DeepClone();
			}
			else if (_object is object[])
			{
				object[] arrayValue = (object[])_object;
				object[] newArray = new object[arrayValue.Length];
				int index = 0;

				foreach (var arrElement in arrayValue)
				{
					newArray[index++] = DeepCloneOrSame(arrElement);
				}

				return newArray;
			}
			else
			{
				return _object;
			}
		}

		public JsonObject DeepClone()
		{
			var newObject = new JsonObject();

			foreach (var key in Keys)
			{
				var value = this[key];

				newObject[key] = DeepCloneOrSame(this[key]);
			}

			return newObject;
		}

		private static Regex pathRegex = new Regex(@"\[(\d+)\]");

		public object FindByPath(string path)
		{
			var pathElements = pathRegex.Replace(path, ".$1").Split('.');
			object currentObject = this;

			foreach (var element in pathElements)
			{
				if (currentObject is object[])
				{
					currentObject = ((object[])currentObject)[int.Parse(element)];
				}
				else if (currentObject is JsonObject)
				{
					currentObject = ((JsonObject)currentObject)[element];
				}
			}

			return currentObject;
		}
	}
}

