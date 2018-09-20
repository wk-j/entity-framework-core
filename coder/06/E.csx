var lines = File.ReadAllLines("coder/06/E.txt");

var tokens = lines.Select(x =>
    x.Split(' ').Skip(1)).SelectMany(x => x);

int Sum(string key) {
    return tokens.Where(x => x.StartsWith(key)).Select(x => x.TrimStart(key.ElementAt(0))).Select(Int32.Parse).Sum();
}

var rs = new {
    Food = Sum("f"),
    Game = Sum("g"),
    Movie = Sum("m"),
    Stationery = Sum("s"),
    Transportation = Sum("t")
};

/*
food 833
game 140
movie 200
stationery 0
transportation 127
TOTAL 1300
 */

Console.WriteLine($@"
food {rs.Food}
game {rs.Game}
movie {rs.Movie}
stationery {rs.Stationery}
transportation {rs.Transportation}
TOTAL { new[] { rs.Food, rs.Game, rs.Movie, rs.Stationery, rs.Transportation }.Sum() }
");