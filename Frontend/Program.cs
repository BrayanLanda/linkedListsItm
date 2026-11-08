using Backend;

var list = new DoublyLinkedList<string>(new SmartComparer());
string? option;

do
{
    ShowMenu();
    option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.Write("Ingrese el valor: ");
            var input = Console.ReadLine() ?? "";
            foreach (var part in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                list.Add(part);
            break;

        case "2":
            ShowItems(list.GetForward());
            break;

        case "3":
            ShowItems(list.GetBackward());
            break;

        case "4":
            list.SortDescending();
            Console.WriteLine("La lista fue ordenada descendentemente.");
            break;

        case "5":
            ShowModes();
            break;

        case "6":
            ShowGraph();
            break;

        case "7":
            var toFind = ReadValue("Ingrese el valor a buscar: ");
            if (toFind is not null)
            {
                Console.WriteLine(list.Exists(toFind)
                    ? $"Sí existe el {toFind}."
                    : $"No existe el {toFind}.");
            }
            break;

        case "8":
            var toRemoveOne = ReadValue("Ingrese el valor a eliminar: ");
            if (toRemoveOne is not null)
            {
                Console.WriteLine(list.RemoveFirst(toRemoveOne)
                    ? $"Se eliminó una ocurrencia del {toRemoveOne}."
                    : $"No existe el {toRemoveOne}.");
            }
            break;

        case "9":
            var toRemoveAll = ReadValue("Ingrese el valor a eliminar: ");
            if (toRemoveAll is not null)
            {
                var removed = list.RemoveAll(toRemoveAll);
                Console.WriteLine(removed > 0
                    ? $"Se eliminaron {removed} ocurrencias del {toRemoveAll}."
                    : $"No existe el {toRemoveAll}.");
            }
            break;

        case "0":
            break;

        case null:
            option = "0";
            break;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }

    Console.WriteLine();

} while (option != "0");

void ShowMenu()
{
    Console.WriteLine("1. Adicionar.");
    Console.WriteLine("2. Mostrar hacia adelante.");
    Console.WriteLine("3. Mostrar hacia atrás.");
    Console.WriteLine("4. Ordenar descendentemente.");
    Console.WriteLine("5. Mostrar la(s) moda(s).");
    Console.WriteLine("6. Mostrar gráfico.");
    Console.WriteLine("7. Existe.");
    Console.WriteLine("8. Eliminar una ocurrencia.");
    Console.WriteLine("9. Eliminar todas las ocurrencias.");
    Console.WriteLine("0. Salir.");
    Console.Write("Opción: ");
}

string? ReadValue(string message)
{
    Console.Write(message);
    var text = Console.ReadLine()?.Trim();
    if (!string.IsNullOrEmpty(text))
        return text;

    Console.WriteLine("Valor no válido.");
    return null;
}

void ShowItems(IEnumerable<string> items)
{
    if (list.Count == 0)
    {
        Console.WriteLine("La lista está vacía.");
        return;
    }
    Console.WriteLine(string.Join(", ", items));
}

void ShowModes()
{
    if (list.Count == 0)
    {
        Console.WriteLine("La lista está vacía.");
        return;
    }

    var modes = list.GetModes();
    modes.Sort(new SmartComparer());

    if (modes.Count == 1)
    {
        Console.WriteLine($"La moda es {modes[0]}.");
        return;
    }

    var head = string.Join(", ", modes[..^1]);
    Console.WriteLine($"Las modas son {head} y {modes[^1]}.");
}

void ShowGraph()
{
    if (list.Count == 0)
    {
        Console.WriteLine("La lista está vacía.");
        return;
    }

    foreach (var (value, count) in list.GetOccurrences())
        Console.WriteLine($"{value} {new string('*', count)}");
}

class SmartComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (int.TryParse(x, out var a) && int.TryParse(y, out var b))
            return a.CompareTo(b);
        return string.CompareOrdinal(x, y);
    }
}