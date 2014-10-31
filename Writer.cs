using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Synchro.Json
{
	public class Writer
	{
		private static Dictionary<char, string> charSubstitutions = new Dictionary<char, string>() {
			{ '\\', @"\\" },
			{ '/', @"\/" },
			{ '"', @"\""" },
			{ '\b', @"\b" },
			{ '\f', @"\f" },
			{ '\n', @"\n" },
			{ '\r', @"\r" },
			{ '\t', @"\t" },
		};

		private static void WriteString(TextWriter writer, string _string)
		{
			writer.Write('\"');
			foreach (var _char in _string)
			{
				if (charSubstitutions.ContainsKey(_char))
				{
					writer.Write(charSubstitutions[_char]);
				}
				else if ((_char < ' ') || (_char > '\u007E'))
				{
					writer.Write(string.Format("\\u{0:X4}", (int) _char));
				}
				else
				{
					writer.Write(_char);
				}
			}
			writer.Write('\"');
		}

		private static void WriteNumber(TextWriter writer, int i)
		{
			writer.Write(i);
		}

		private static void WriteNumber(TextWriter writer, double d)
		{
			writer.Write(d.ToString(CultureInfo.InvariantCulture.NumberFormat));
		}

		private static void WriteArray(TextWriter writer, object[] array)
		{
			bool firstElement = true;

			writer.Write('[');
			foreach (var value in array)
			{
				if (!firstElement)
				{
					writer.Write(',');
				}
				else
				{
					firstElement = false;
				}

				WriteValue(writer, value);
			}
			writer.Write(']');
		}

		private static void WriteBoolean(TextWriter writer, bool b)
		{
			writer.Write(b ? "true" : "false");
		}

		static void WriteNull(TextWriter writer)
		{
			writer.Write("null");
		}

		private static Dictionary<Type, Action<TextWriter, object>> writerActions = new Dictionary<Type, Action<TextWriter, object>>() {
			{ typeof(JsonObject), (writer, value) => { WriteObject(writer, (JsonObject) value); } },
			{ typeof(string), (writer, value) => { WriteString(writer, (string) value); } },
			{ typeof(int), (writer, value) => { WriteNumber(writer, (int)value); } },
			{ typeof(double), (writer, value) => { WriteNumber(writer, (double)value); } },
			{ typeof(object[]), (writer, value) => { WriteArray(writer, (object[])value); } },
			{ typeof(bool), (writer, value) => { WriteBoolean(writer, (bool)value); } },
		};

		public static void WriteValue(TextWriter writer, object value)
		{
			if (value == null)
			{
				WriteNull(writer);
			}
			else if (writerActions.ContainsKey(value.GetType()))
			{
				writerActions[value.GetType()](writer, value);
			}
			else
			{
				throw new IOException(string.Format("Unknown object type {0}", value.GetType()));
			}
		}

		private static void WriteObject(TextWriter writer, JsonObject _object)
		{
			bool firstKey = true;

			writer.Write('{');
			foreach (var key in _object.Keys)
			{
				var value = _object[key];

				if (!firstKey)
				{
					writer.Write(',');
				}
				else
				{
					firstKey = false;
				}

				WriteString(writer, key);

				writer.Write(':');

				WriteValue(writer, value);
			}
			writer.Write('}');
		}
	}
}

