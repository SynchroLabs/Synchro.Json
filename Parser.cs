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
				returnString.Append((char) thisChar);
			}

			return returnString.ToString();
		}
	}
}

