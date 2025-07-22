using System;
using System.Collections.Generic;

namespace pdfforge.PDFCreator.Utilities.Tokens;

public class UserToken : IToken
{
    private readonly string _name;
    public readonly Dictionary<string, string> KeyValueDict;

    public UserToken()
    {
        _name = "User";
        KeyValueDict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public string GetValue()
    {
        return "";
    }

    public string GetValueWithFormat(string formatString)
    {
        var split = formatString.Split(new[] { ':' }, 2);
        var key = split[0];

        if (KeyValueDict.ContainsKey(key))
            return KeyValueDict[key];

        if (split.Length > 1)
            return split[1];

        return "";
    }

    public string GetName()
    {
        return _name;
    }

    public void AddKeyValuePair(string key, string value)
    {
        if (!string.IsNullOrEmpty(key))
            if (!KeyValueDict.ContainsKey(key)) //Use value from first appearance
                KeyValueDict[key] = value;
    }

    public void Merge(UserToken otherUserToken)
    {
        if (otherUserToken == null)
            return;

        foreach (var keyValuePair in otherUserToken.KeyValueDict)
            AddKeyValuePair(keyValuePair.Key, keyValuePair.Value);
    }
}
