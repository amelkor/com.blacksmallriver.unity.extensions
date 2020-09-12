using System;
using System.Text;

namespace Bsr.Unity.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes first line if there is more than one exists.
        /// </summary>
        /// <param name="sb">String builder instance.</param>
        /// <returns>Returns true if a line was removed.</returns>
        public static bool RemoveFirstLine(this StringBuilder sb)
        {
            var newLineLength = Environment.NewLine.Length;
            var index = 0;
            do
            {
                for (var i = 0; i < newLineLength; i++)
                {
                    if (!sb[index + i].Equals(Environment.NewLine[i]))
                        break;
                    sb.Remove(0, index + newLineLength);
                    return true;
                }

                index++;
            } while (index <= sb.Length - newLineLength);

            return false;
        }
    }
}