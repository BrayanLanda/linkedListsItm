namespace Backend;

public class DoublyLinkedList<T> where T : IComparable<T>
{
    public Node<T>? Head { get; private set; }
    public Node<T>? Tail { get; private set; }
    public int Count { get; private set; }

    public void Add(T value)
    {
        var newNode = new Node<T>(value);

        if (Head is null)
        {
            Head = Tail = newNode;
            Count++;
            return;
        }

        if (value.CompareTo(Head.Value) < 0)
        {
            newNode.Next = Head;
            Head.Previous = newNode;
            Head = newNode;
            Count++;
            return;
        }

        var current = Head;
        while (current.Next is not null && current.Next.Value.CompareTo(value) <= 0)
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
}
