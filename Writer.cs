using System;
using System.IO;

namespace Synchro.Json
{
	public class Writer
	{
		private static void WriteString(TextWriter writer, string _string)
		{
			writer.Write('\"');
			foreach (var _char in _string)
			{
				switch (_char)
				{
					case '\\':
						writer.Write(@"\\");
						break;
					case '/':
						writer.Write(@"\/");
						break;
					case '"':
						writer.Write(@"\""");
						break;
					case '\b':
						writer.Write(@"\b");
						break;
					case '\f':
						writer.Write(@"\f");
						break;
					case '\n':
						writer.Write(@"\n");
						break;
					case '\r':
						writer.Write(@"\r");
						break;
					case '\t':
						writer.Write(@"\t");
						break;
					default:
						if ((_char < ' ') || (_char > '\u007E'))
						{
							writer.Write(string.Format("\\u{0:X4}", (int) _char));
						}
						else
						{
							writer.Write(_char);
						}
						break;
				}
			}
			writer.Write('\"');
		}

		private static void WriteNumber(TextWriter writer, int i)
		{
			writer.Write(i);
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

		public static void WriteValue(TextWriter writer, object value)
		{
			if (value.GetType() == typeof(JsonObject))
			{
				WriteObject(writer, (JsonObject)value);
			}
			else if (value.GetType() == typeof(string))
			{
				WriteString(writer, (string)value);
			}
			else if (value.GetType() == typeof(int))
			{
				WriteNumber(writer, (int)value);
			}
			else if (value.GetType() == typeof(object[]))
			{
				WriteArray(writer, (object[])value);
			}
			else if (value.GetType() == typeof(bool))
			{
				WriteBoolean(writer, (bool)value);
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

