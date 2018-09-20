var text = File.ReadAllText("coder/03-TransposeSong/T.txt");
var dict = new Dictionary<string, string> {
    {"C", "CCC"},
    {"Dm", "DDD"},
    {"Em", "EEE"},
    {"F", "FFF"},
    {"G", "GGG"},
    {"Am", "AAA"},

    {"CCC", "D"},
    {"DDD", "Em"},
    {"EEE", "F#m"},
    {"FFF", "G"},
    {"GGG", "A"},
    {"AAA", "Bm"}
};

// var rs = dict.Aggregate(text, (acc, kv) => acc.Replace($"[{kv.Key}]", $"[{kv.Key}]~[{kv.Value}]"));

foreach (var (k, v) in dict) {
    var kk = $"[{k}]";
    var vv = $"[{v}]";
    text = text.Replace(kk, vv);
}
Console.WriteLine(text);
