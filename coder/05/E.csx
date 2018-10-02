var lines = File.ReadAllLines("coder/04/E.txt");

var tokens = lines.Select(x =>
    x.Split(' ')
        .Skip(1)
        .Select(k =>
            k.TrimStart('f')
            .TrimStart('g')
            .TrimStart('m')
            .TrimStart('s')
            .TrimStart('t'))
    .Select(Int32.Parse))
    .SelectMany(x => x).Sum();

Console.WriteLine(tokens / 31.0);