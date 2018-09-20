var text = File.ReadAllText("scripts/Transpose.txt");
var dict = new Dictionary<string, string> {
    {"C", "D"},
    {"Dm", "@"},
    {"Em", "#"},
    {"F", "_"},
    {"G", "A"},
    {"Am", "Bm"},
    {"_", "G"},
    {"@", "Em"},
    {"#", "F#m"}
};

var rs = dict.Aggregate(text, (acc, kv) => acc.Replace(kv.Key, kv.Value));
Console.WriteLine(rs);
