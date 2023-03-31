using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

public static class CSVReader
{
    private const string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private const string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    private static readonly char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string text)
    {
        var list = new List<Dictionary<string, object>>();

        var lines = Regex.Split (text, LINE_SPLIT_RE);

        if (lines.Length <= 1)
            return list;

        var header = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);

            if (values.Length == 0 || values[0] == "")
                continue;

            var entry = new Dictionary<string, object>();

            for (var j = 0; j < header.Length && j < values.Length; j++ )
            {
                var value = values[j]
                    .TrimStart(TRIM_CHARS)
                    .TrimEnd(TRIM_CHARS)
                    .Replace("\\", "");

                object finalValue = value;

                if (int.TryParse(value, out var n))
                {
                    finalValue = n;
                }
                else if (float.TryParse(value, out var f))
                {
                    finalValue = f;
                }

                entry[header[j]] = finalValue;
            }

            list.Add(entry);
        }

        return list;
    }
}

public static class CsvExtensions
{
    public static int GetInt(this Dictionary<string, object> obj, string key, bool isRequired = false)
    {
        if (obj.ContainsKey(key))
        {
            if (string.IsNullOrEmpty(obj[key].ToString()))
            {
                if (isRequired)
                    throw new Exception($"Required field {key} in object {JsonConvert.SerializeObject(obj)} is empty");

                return 0;
            }
            
            try
            {
                return Convert.ToInt32(obj[key]);
            }
            catch (Exception)
            {
                Debug.LogError($"Can't convert field {key} to int in object {JsonConvert.SerializeObject(obj)}");
                throw;
            }
        }

        if (isRequired)
            throw new Exception($"Can't find required field {key} in object {JsonConvert.SerializeObject(obj)}");

        return 0;
    }

    public static string GetString(this Dictionary<string, object> obj, string key)
    {
        if (obj.ContainsKey(key))
            return obj[key].ToString();

        throw new Exception($"Can't find field {key} in object {JsonConvert.SerializeObject(obj)}");
    }

    public static T GetEnum<T>(this Dictionary<string, object> obj, string key) where T: Enum
    {
        if (!obj.ContainsKey(key))
            throw new Exception($"Can't find field {key} in object {JsonConvert.SerializeObject(obj)}");

        try
        {
            return (T)Enum.Parse(typeof(T), obj.GetString(key));
        }
        catch (Exception)
        {
            throw new Exception($"Can't parse enum {typeof(T)} value of key {key} in object {JsonConvert.SerializeObject(obj)}");
        }
    }

    public static bool GetBool(this Dictionary<string, object> obj, string key)
    {
        if (!obj.ContainsKey(key))
            throw new Exception($"Can't find field {key} in object {JsonConvert.SerializeObject(obj)}");

        try
        {
            return Convert.ToBoolean(obj[key]);
        }
        catch (Exception)
        {
            throw new Exception($"Can't parse bool value of key {key} in object {JsonConvert.SerializeObject(obj)}");
        }
    }
}