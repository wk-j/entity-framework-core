var a = new {
    A = "Hello",
    B = DateTime.Now
};

var names = a.GetType().GetProperties().Select(p => (p.Name, p.PropertyType)).ToArray();
foreach (var item in names) {
    Console.WriteLine($"{item.Name} {item.PropertyType}");
}