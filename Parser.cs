using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Synchro.Json
{
	public class Parser
	{
		private static void SkipWhitespace(TextReader reader)
		{
			while (Char.IsWhiteSpace((char) reader.Peek()))
			{
				reader.Read();
			}
		}

		private static string ParseString(TextReader reader)
		{
			int thisChar;
			var returnString = new StringBuilder();

			SkipWhitespace(reader);

			// Skip the opening quotes

			reader.Read();

			// Read until closing quotes

			while ((thisChar = reader.Read()) != '"')
			{
				if (thisChar == '\\')
				{
					thisChar = reader.Read();

					switch (thisChar)
					{
						case 'b':
							thisChar = '\b';
							break;
						case 'f':
							thisChar = '\f';
							break;
						case 'r':
							thisChar = '\r';
							break;
						case 'n':
							thisChar = '\n';
							break;
						case 't':
							thisChar = '\t';
							break;
						case 'u':
							// Parse four hex digits
							var hexBuilder = new StringBuilder(4);
							for (int counter = 0; counter < 4; ++counter)
							{
								hexBuilder.Append((char)reader.Read());
							}
							thisChar = Convert.ToInt32(hexBuilder.ToString(), 16);
							break;
						case '\\':
						case '"':
						case '/':
						default:
							break;
					}
				}

				returnString.Append((char) thisChar);
			}

			return returnString.ToString();
		}

		private static int ParseNumber(TextReader reader)
		{
			int sign = 1;
			int thisChar;
			int number = 0;

			SkipWhitespace(reader);

			thisChar = reader.Read();
			if (thisChar == '-')
			{
				sign = -1;
			}
			else
			{
				number = thisChar - '0';
			}

			while ((reader.Peek() >= '0') && (reader.Peek() <= '9'))
			{
				number *= 10;
				number += reader.Read() - '0';
			}

			return number * sign;
		}

		private static object[] ParseArray(TextReader reader)
		{
			var finalList = new List<object>();

			SkipWhitespace(reader);

			// Skip the opening bracket

			reader.Read();

			SkipWhitespace(reader);

			// Read until closing bracket

			while (reader.Peek() != ']')
			{
				// Read a value

				finalList.Add(ParseValue(reader));

				SkipWhitespace(reader);

				// Skip the comma if any

				if (reader.Peek() == ',')
				{
					reader.Read();
				}

				SkipWhitespace(reader);
			}

			// Skip the closing bracket

			reader.Read();

			return finalList.ToArray();
		}

		private static JsonObject ParseObject(TextReader reader)
		{
			var finalObject = new JsonObject();

			SkipWhitespace(reader);

			// Skip the opening brace

			reader.Read();

			SkipWhitespace(reader);

			// Read until closing brace

			while (reader.Peek() != '}')
			{
				string name;
				object value;

				// Read a string

				name = ParseString(reader);

				SkipWhitespace(reader);

				// Skip the colon

				reader.Read();

				SkipWhitespace(reader);

				// Read the value

				value = ParseValue(reader);

				SkipWhitespace(reader);

				finalObject[name] = value;

				// Skip the comma if any

				if (reader.Peek() == ',')
				{
					reader.Read();
				}

				SkipWhitespace(reader);
			}

			// Skip the closing brace

			reader.Read();

			return finalObject;
		}

		public static object ParseValue(TextReader reader)
		{
			SkipWhitespace(reader);

			int lookahead = reader.Peek();

			if ((lookahead == '-') || ((lookahead >= '0') && (lookahead <= '9')))
			{
				return ParseNumber(reader);
			}
			else if (lookahead == '[')
			{
				return ParseArray(reader);
			}
			else if (lookahead == '{')
			{
				return ParseObject(reader);
			}
			else
			{
				return ParseString(reader);
			}
		}
	}
}

