using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NMultiTool.Library.Module.Commands.FindDuplicateFiles
{
    public class DuplicateFilesCompare
    {
        readonly List<Regex> _rules = new List<Regex>();

        public DuplicateFilesCompare(IEnumerable<string> fileLocationsToKeep)
        {
            foreach (string rule in fileLocationsToKeep)
            {
                _rules.Add(new Regex(rule));
            }
        }

        /// <summary>  Compare paths. </summary>
        ///
        /// <remarks>  Trond, 23.09.2012. </remarks>
        ///
        /// <param name="file1">   The first file. </param>
        /// <param name="file2">   The second file. </param>
        ///
        /// <returns>  . </returns>

        public int ComparePaths(DuplicateFileInfo file1, DuplicateFileInfo file2)
        {

            string path1 = file1.FullName;
            string path2 = file2.FullName;
            if (path1 == null)
            {
                if (path2 == null)
                {
                    // If path1 is null and path2 is null, they're equal. 
                    return 0;
                }
                // If path1 is null and path2 is not null, then path2 is greater. 
                return -1;
            }
            // path1 is not null...
            if (path2 == null)
            {
                //path2 is null, path1 is therefore greater.
                return 1;
            }

            foreach (Regex rule in _rules)
            {
                if (rule.IsMatch(path1) && !rule.IsMatch(path2))
                {
                    return 1;
                }
                if (!rule.IsMatch(path1) && rule.IsMatch(path2))
                {
                    return -1;
                }
            }

            //Nonen of the rules matched, compare by path length.

            int retval = path2.Length.CompareTo(path1.Length);
            if (retval != 0)
            {
                // If the strings are not of equal length,
                // the longer string is greater.
                //
                return retval;
            }
            // The strings are of equal length, sort them with ordinary string comparison.         
            return System.String.Compare(path2, path1, System.StringComparison.Ordinal);
        }
    }
}