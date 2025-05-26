using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public static class LocalizationHelper
{
    private static Dictionary<string, Dictionary<string, string>> _allMessages;
    private static string _currentLanguage = "en";

    public static void LoadMessages(string languageCode)
    {
        _currentLanguage = languageCode ?? "en";
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Translations", "messages.json");

        if (!File.Exists(path))
        {
            _allMessages = new Dictionary<string, Dictionary<string, string>>();
            return;
        }

        string json = File.ReadAllText(path);
        _allMessages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
    }

    public static string T(string key)
    {
        if (_allMessages != null &&
            _allMessages.TryGetValue(_currentLanguage, out var langDict) &&
            langDict.TryGetValue(key, out string value))
        {
            return value;
        }

        return key; // fallback: exibe a própria chave se não encontrar
    }
}

