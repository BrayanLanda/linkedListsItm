namespace Backend;

public class DoublyLinkedList<T>
{
    public Node<T>? Head { get; private set; }
    public Node<T>? Tail { get; private set; }
    public int Count { get; private set; }

    private bool _isDescending;
    private readonly IComparer<T> _comparer;

    public DoublyLinkedList(IComparer<T>? comparer = null)
    {
        _comparer = comparer ?? Comparer<T>.Default;
    }

    private int Compare(T a, T b) => _isDescending ? _comparer.Compare(b, a) : _comparer.Compare(a, b);

    private bool AreEqual(T a, T b) => _comparer.Compare(a, b) == 0;

    public void Add(T value)
    {
        var newNode = new Node<T>(value);

        if (Head is null)
        {
            Head = Tail = newNode;
            Count++;
            return;
        }

        if (Compare(value, Head.Value) < 0)
        {
            newNode.Next = Head;
            Head.Previous = newNode;
            Head = newNode;
            Count++;
            return;
        }

        var current = Head;
        while (current.Next is not null && Compare(current.Next.Value, value) <= 0)
        {
            current = current.Next;
        }

        newNode.Previous = current;
        newNode.Next = current.Next;

        if (current.Next is not null)
            current.Next.Previous = newNode;
        else
            Tail = newNode;

        current.Next = newNode;
        Count++;
    }

    public IEnumerable<T> GetForward()
    {
        var current = Head;
        while (current is not null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    public IEnumerable<T> GetBackward()
    {
        var current = Tail;
        while (current is not null)
        {
            yield return current.Value;
            current = current.Previous;
        }
    }

    public bool Exists(T value)
    {
        var current = Head;
        while (current is not null)
        {
            if (AreEqual(current.Value, value))
                return true;
            current = current.Next;
        }
        return false;
    }

    public bool RemoveFirst(T value)
    {
        var current = Head;
        while (current is not null)
        {
            if (AreEqual(current.Value, value))
            {
                Unlink(current);
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public int RemoveAll(T value)
    {
        int removed = 0;
        var current = Head;
        while (current is not null)
        {
            var next = current.Next;
            if (AreEqual(current.Value, value))
            {
                Unlink(current);
                removed++;
            }
            current = next;
        }
        return removed;
    }

    private void Unlink(Node<T> node)
    {
        if (node.Previous is not null)
            node.Previous.Next = node.Next;
        else
            Head = node.Next;

        if (node.Next is not null)
            node.Next.Previous = node.Previous;
        else
            Tail = node.Previous;

        Count--;
    }

    public void SortDescending()
    {
        if (_isDescending) return;

        var current = Head;
        while (current is not null)
        {
            var next = current.Next; 
            current.Next = current.Previous;
            current.Previous = next;
            current = next;
        }

        (Head, Tail) = (Tail, Head);
        _isDescending = true;
    }

    public List<(T Value, int Count)> GetOccurrences()
    {
        var result = new List<(T Value, int Count)>();
        var current = Head;

        while (current is not null)
        {
            if (result.Count > 0 && AreEqual(result[^1].Value, current.Value))
                result[^1] = (result[^1].Value, result[^1].Count + 1);
            else
                result.Add((current.Value, 1));

            current = current.Next;
        }

        return result;
    }

    public List<T> GetModes()
    {
        var occurrences = GetOccurrences();
        if (occurrences.Count == 0) return new List<T>();

        int max = occurrences.Max(o => o.Count);
        return occurrences.Where(o => o.Count == max).Select(o => o.Value).ToList();
    }
}