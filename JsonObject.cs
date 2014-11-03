using System;
using System.Collections.Generic;

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
	}
}

