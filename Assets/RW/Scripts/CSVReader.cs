using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// Taken from here: 
// https://bravenewmethod.com/2014/09/13/lightweight-csv-reader-for-unity/
// Code parses a CSV, converting values into ints or floats if able, and 
// returning a List<Dictionary<string, object>>.
public class CSVReader
{
    // Define delimiters, regular expression craziness
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    // Define line delimiters, regular experession craziness
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r"; 
    static char[] TRIM_CHARS = { '\"' };
    //Declare method
    public static List<Dictionary<string, object>> Read(string file) 
    {
        // Print filename, make sure parsed correctly
        Debug.Log("CSVReader is reading " + file);
        //declare dictionary list
        var list = new List<Dictionary<string, object>>();
        //Loads the TextAsset named in the file argument of the function
        TextAsset data = Resources.Load(file) as TextAsset;
        // Split data.text into lines using LINE_SPLIT_RE characters
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);
        //Check that there is more than one line
        if (lines.Length <= 1) return list;
        //Split header (element 0)
        var header = Regex.Split(lines[0], SPLIT_RE); 
        // Loops through lines
        for (var i = 1; i < lines.Length; i++)
        {
            // Split lines according to SPLIT_RE, store in var 
            // (usually string array)
            var values = Regex.Split(lines[i], SPLIT_RE);
            // Skip to end of loop (continue) if value is 0 length OR first
            // value is empty
            if (values.Length == 0 || values[0] == "") continue;
            // Creates dictionary object
            var entry = new Dictionary<string, object>(); 
            // Loops through every value
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j]; // Set local variable value
                                          // Trim characters
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS)
                    .Replace("\\", "");
                object finalvalue = value; //set final value
                int n; // Create int, to hold value if int
                float f; // Create float, to hold value if float
                // If-else to attempt to parse value into int or float
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry); // Add Dictionary ("entry" variable) to list
        }
        return list; //Return list
    }
}