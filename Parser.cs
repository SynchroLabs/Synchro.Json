using System;
using System.IO;
using System.Text;

namespace Synchro.Json
{
	public class Parser
	{
		public static string ParseString(TextReader reader)
		{
			int thisChar;
			var returnString = new StringBuilder();

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
	}
}

