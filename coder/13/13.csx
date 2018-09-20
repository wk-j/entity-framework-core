
var text = File.ReadAllLines("coder/12/12.txt");
var rs = text.Select(x => {
    var sc = int.Parse(x);
    if (sc < 50) return 0;
    if (sc < 55) return 0.5;
    if (sc < 60) return 1;
    if (sc < 65) return 1.5;
    if (sc < 70) return 2;
    if (sc < 75) return 2.5;
    if (sc < 80) return 3;
    if (sc < 85) return 3.5;
    return 4;
});

Console.WriteLine(rs.Sum() / rs.Count());

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
