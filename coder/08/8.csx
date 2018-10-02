class Route {
    public string Start { set; get; }
    public string End { set; get; }
    public int Long { set; get; }
}


var text = File.ReadAllLines("coder/08/8.txt");
var routes = text.Select(x => {
    var k = x.Split(' ');
    return new Route { Start = k[0], End = k[1], Long = int.Parse(k[2]) };
});

int Travel(Route next, int total) {
    if (next.End != "DEST") {
        var rs = routes.Where(x => x.Start == next.End);
        var all = rs.Select(item => {
            Console.Write($"{next.Start} -> {item.Start}-{next.Long} ");
            return Travel(item, total + next.Long);
        });
        return all.Min();
    } else {
        Console.Write($"{next.Start} -> {next.End}-{next.Long} ");
        return total + next.Long;
    }
}

var homes = routes.Where(x => x.Start == "HOME");


var min = homes.Select(x => Travel(x, 0)).Min();
Console.WriteLine("###");
Console.WriteLine(min);

/*
HOME U 91
HOME R 9
HOME J 84
HOME F 105
A R 116
A O 75
A Y 28
A U 109
B L 94
B S 99
C V 52
C R 69
C J 54
C N 54
D O 10
D N 102
D G 111
D L 82
E U 93
E Q 29
E T 96
E N 48
F G 3
F O 39
F V 72
G L 108
G O 67
G M 83
G P 39
H P 7
H X 96
H S 33
I L 10
I R 99
J U 49
J N 10
J X 17
K Z 119
L T 56
L Q 106
M V 99
N Q 67
N X 20
N V 100
N U 76
O X 16
O W 20
O DEST 59
O Y 105
P X 44
Q U 75
Q R 34
Q T 110
Q S 67
R Z 35
R V 42
R S 46
S X 50
S Y 36
S DEST 30
T Y 49
T U 61
U V 98
V W 80
W Z 86
W Y 61
X DEST 95
X Z 3
X Y 13
Y Z 72
Y DEST 8
Z DEST 55
 */
