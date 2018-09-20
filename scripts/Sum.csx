
var x = File.ReadAllLines("scripts/Sum.txt").Select(Int32.Parse).Sum();
Console.WriteLine(x);