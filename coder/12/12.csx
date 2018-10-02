
var text = File.ReadAllLines("coder/12/12.txt");
var rs = text.Select(x => {
    var sc = int.Parse(x);
    if (sc < 50) return "F";
    if (sc < 55) return "D";
    if (sc < 60) return "D+";
    if (sc < 65) return "C";
    if (sc < 70) return "C+";
    if (sc < 75) return "B";
    if (sc < 80) return "B+";
    if (sc < 85) return "A";
    return "A+";
});

var group = rs.GroupBy(x => x).OrderBy(x => x.Key);
foreach (var item in group) {
    Console.WriteLine($"{item.Key} {item.Count()}");
}

/*
A+ 88
A 23
B+ 25
B 30
C+ 26
C 27
D+ 15
D 23
F 243
 */
