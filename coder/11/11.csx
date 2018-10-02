
class F {
    public string Name { set; get; }
    public string Sign { set; get; }
}

var text = File.ReadAllLines("coder/11/11.txt");
var fs = text.Select(x => {
    var tokens = x.Split(' ');
    return new F {
        Name = tokens[0],
        Sign = tokens[1]
    };
});

var gr = fs.GroupBy(x => x.Sign).Where(x => x.Count() == 1).Select(x => x.First()).OrderBy(x => x.Name);
foreach (var item in gr) {
    Console.WriteLine(item.Name);
}
