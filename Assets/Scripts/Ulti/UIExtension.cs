using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

public static class UIExtension
{
    public static string EncodeString(this string input)
    {
        Dictionary<string, string> toBeEncoded = new Dictionary<string, string>() { { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "%20" },
    { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" }, { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },{ "-", "%2D" },
    { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" }, { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" } };
        Regex replaceRegex = new Regex(@"[%!# $&'()*+,-/:;=?@\[\]]");
        MatchEvaluator matchEval = match => toBeEncoded[match.Value];
        string encoded = replaceRegex.Replace(input, matchEval);
        return encoded;
    }
    public static string DateTimeToDate(this string input, int indexDebug)
    {
        try
        {
            //	string iString = "2005-05-05 22:12 PM";
            //	input = input.Replace("T", " ");
            //	Debug.LogError(input);
            if (input == null) return "";
            if (input.Trim().Length == 0) return "";
            DateTime oDate = DateTime.ParseExact(input, "yyyy-MM-ddTHH:mm:ss", null);
            return oDate.ToString("dd/MM/yyyy");
        }
        catch (Exception ex)
        {
            Debug.LogError(input + "index:" + indexDebug + "___" + ex.ToString());
            return input;
        }

    }

    public static string BadDateToGoodDate(this string input)
    {
        try
        {
            DateTime myDate = DateTime.ParseExact(input, "dd/MM/yyyy", null);
            return myDate.ToString("yyyy-MM-ddTHH:mm:ss");
        }
        catch (Exception ex)
        {
            Debug.LogError("___" + ex.ToString());
            return input;
        }

    }

    public static string DateTimeToString(this DateTime input)
    {
        try
        {
            string outPut = input.ToString("yyyy-MM-ddTHH:mm:ss");
            //	Debug.Log(outPut);
            return outPut;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            //	Debug.LogError(ex.ToString());
            return "";
        }
    }

    public static string IntoNumberText(this int input)
    {
        try
        {
            return input.ToString("#,#", CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return input.ToString();
        }
    }

    public static string toJSON(this string[] input)
    {
        try
        {
            string outPut = "{";
            for (int i = 0; i < input.Length; i = i + 2)
            {
                if (input[i + 1] != null && input[i + 1].Trim().Length > 0)
                {
                    if (i > 0) outPut += ",";
                    //	Debug.LogError("hmm " + input[i].Trim());
                    outPut += '\u0022' + input[i].Trim() + '\u0022' + ":" + '\u0022' + input[i + 1].Trim() + '\u0022';
                }
                else
                {
                    Debug.LogError("null");
                }
            }
            outPut += "}";
            return outPut;
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            return null;
        }

    }

    private static readonly string[] VietnameseSigns = new string[]
        {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
        };

    public static string RemoveSign4VietnameseString(string str)
    {
        for (int i = 1; i < VietnameseSigns.Length; i++)
        {
            for (int j = 0; j < VietnameseSigns[i].Length; j++)
                str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
        }
        return str;
    }
    public static string ToPrint(this string input)
    {
        //string[] dics = new string[] {
        // "ạ", "a",
        // "à", "a",
        // "á", "a",
        // "ậ", "a",
        // "ệ","e"
        // };

        //    if (AppCache.InCoDau) return input;
        Debug.LogError("Input " + input);
        List<List<string>> dics = new List<List<string>>()
        {
            new List<string>(){"a","ạ","à", "ả", "á", "â", "ấ", "ậ", "ầ","ă", "ắ", "ằ" ,"ặ"},
            new List<string>(){"d", "đ"},
            new List<string>(){"e", "ệ", "ề","ế","ê" },
            new List<string>(){"o", "ó", "ò", "ọ", "ô", "ộ","ớ","ơ" },
            new List<string>(){"u", "ư","ứ","ử" },
               new List<string>(){"y", "ý" },
            new List<string>(){"i", "í","ì" },
        };
        foreach (var item in dics)
        {
            string _target = item[0];
            for (int i = 1; i < item.Count; i++)
            {

                input = input.Replace(item[i], _target);
                input = input.Replace(item[i].ToUpper(), _target.ToUpper());
                //    Debug.LogError("Repace " + item[i] + " with " + _target + " = " + input);
            }
        }
        input = input.Replace("\u0300", "");
        input = input.Replace("\u02C6", "");
        input = input.Replace("\u0306", "");

        //input = input.Replace(/ à | á | ạ | ả | ã | â | ầ | ấ | ậ | ẩ | ẫ | ă | ằ | ắ | ặ | ẳ | ẵ / g, "a");
        //input = input.replace(/ è | é | ẹ | ẻ | ẽ | ê | ề | ế | ệ | ể | ễ / g, "e");
        //input = input.replace(/ ì | í | ị | ỉ | ĩ / g, "i");
        //input = input.replace(/ ò | ó | ọ | ỏ | õ | ô | ồ | ố | ộ | ổ | ỗ | ơ | ờ | ớ | ợ | ở | ỡ / g, "o");
        //input = input.replace(/ ù | ú | ụ | ủ | ũ | ư | ừ | ứ | ự | ử | ữ / g, "u");
        //input = input.replace(/ ỳ | ý | ỵ | ỷ | ỹ / g, "y");
        //input = input.replace(/ đ / g, "d");
        // Some system encode vietnamese combining accent as individual utf-8 characters
        //str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/ g, ""); // Huyền sắc hỏi ngã nặng 
        //str = str.replace(/ \u02C6 |\u0306|\u031B/ g, ""); // Â, Ê, Ă, Ơ, Ư

        //Debug.LogError("output  " + input);

        input = RemoveSign4VietnameseString(input);
        return input;
    }
}
