class In {
    public string Actor { set; get; }
    public string Inf { set; get; }
    public string Action { set; get; }
    public string Score { set; get; }
}

var text = File.ReadAllLines("coder/10/9.txt");

int GetScore(int count, string action) {
    if (action is "like") return count;
    if (action is "retweet") return count * 3;
    return count * 2;
}

var ins = text.Select(x => {
    var tokens = x.Split(" ");
    var action = tokens[2];
    var count = int.Parse(tokens[3]);
    return new {
        Actor = tokens[0],
        Inf = tokens[1],
        Score = GetScore(count, action)
    };
});


var rs = ins.GroupBy(x => x.Actor).OrderByDescending(x => x.Sum(k => k.Score)).Take(5);
foreach (var item in rs) {
    Console.WriteLine($"{item.Key} {item.Sum(x => x.Score)}");
    // Console.WriteLine($"{item.Key}");
}