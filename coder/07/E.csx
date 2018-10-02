var lines = File.ReadAllLines("coder/07/_E.txt");

var tokens = lines.Select(x =>
    x.Split(' ')
        .Skip(1)
        .Select(k =>
            k.TrimStart('f')
            .TrimStart('g')
            .TrimStart('m')
            .TrimStart('s')
            .TrimStart('t'))
    .Select(Int32.Parse));



var d = tokens.Select((x, i) => {
    return new {
        Day = new DateTime(2018, 3, i + 1).ToString("ddd"),
        Total = x.Sum()
    };
});

var groups = d.GroupBy(x => x.Day);
foreach (var item in groups) {
    Console.WriteLine($"{item.Key} {item.Sum(x => x.Total)}");
}


/*
Mon 1214
Tue 891
Wed 844
Thu 1033
Fri 901
Sat 69
Sun 156
 */
