using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq;
using System.Text;

namespace Puniemu.Src.DataManager.Logic;

public class GameDataManager
{
    public ConcurrentDictionary<string, string> GamedataCache = new();
    public GameDataManager()
    {
        CacheGamedataFromResources();
    }
    public string GetTableStringFromJson(string tableId)
    {
        var raw = GamedataCache[tableId];
        var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(raw);
        return (string)json["tableData"];
    }
    
    private void CacheGamedataFromResources()
{
    var assembly = Assembly.GetExecutingAssembly();
    string rootNamespace = assembly.GetName().Name!;
    string[] resourceNames = assembly.GetManifestResourceNames();

    Console.WriteLine($"[RESOURCE DEBUG] Total embedded resources found: {resourceNames.Length}");

    foreach (var resourceName in resourceNames)
    {
        using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
        {
            if (stream != null)
            {
                using (StreamReader reader = new StreamReader(stream, encoding: Encoding.UTF8))
                {
                    string content = reader.ReadToEnd();
                    var filteredName = resourceName
                        .Replace($"{rootNamespace}.Resources.", "")
                        .Replace(".txt", "");
                    GamedataCache[filteredName] = content;
                }
            }
        }
    }

    // --- DIAGNÓSTICO TEMPORAL ---
    Console.WriteLine("[RESOURCE DEBUG] Checking specific keys:");
    string[] toCheck = { "ywp_mst_summary", "ywp_mst_game_shoot", "ywp_mst_watch_vertical_frame" };
    foreach (var key in toCheck)
    {
        Console.WriteLine($"[RESOURCE DEBUG] '{key}' exists in cache: {GamedataCache.ContainsKey(key)}");
    }
    Console.WriteLine("[RESOURCE DEBUG] Sample of actual keys (first 10 containing 'summary' or 'shoot'):");
    foreach (var k in GamedataCache.Keys.Where(k => k.Contains("summary") || k.Contains("shoot")).Take(10))
    {
        Console.WriteLine($"[RESOURCE DEBUG] actual key: '{k}'");
    }
    // --- FIN DIAGNÓSTICO ---
}
}
