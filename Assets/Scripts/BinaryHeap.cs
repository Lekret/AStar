using System;

public interface IBinaryHeapItem<T> : IComparable<T> where T : IBinaryHeapItem<T>
{
    int HeapIndex { get; set; }
}

public class BinaryHeap<T> where T : IBinaryHeapItem<T>
{
    private T[] _items;
    private int _count;
    private int _capacity;

    public BinaryHeap() : this(100)
    {
    }

    public BinaryHeap(int capacity)
    {
        _capacity = capacity;
        _items = new T[capacity];
    }

    public int Count => _count;
    
    public int Capacity
    {
        get => _capacity;
        set => Resize(_capacity);
    }

    public void Clear()
    {
        _count = 0;
    }
    
    public bool Contains(T item)
    {
        return item.HeapIndex < _count && Equals(_items[item.HeapIndex], item);
    }

    public void Resize(int capacity)
    {
        _capacity = capacity;
        Array.Resize(ref _items, _capacity);
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }
    
    public void Add(T item)
    {
        if (_count >= _capacity)
            Resize(_capacity > 0 ? _capacity * 2 : 4);

        item.HeapIndex = _count;
        _items[_count] = item;
        SortUp(item);
        _count++;
    }

    public T RemoveFirst()
    {
        if (_count == 0)
            throw new IndexOutOfRangeException("Heap count is zero, can't take first item");

        var first = _items[0];
        _count--;
        ref var refFirst = ref _items[0];
        refFirst = _items[_count];
        refFirst.HeapIndex = 0;
        SortDown(refFirst);
        return first;
    }

    private void SortUp(T item)
    {
        var parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            var parentItem = _items[parentIndex];
            if (item.CompareTo(parentItem) <= 0)
                break;

            Swap(item, parentItem);
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            var leftChildIndex = item.HeapIndex * 2 + 1;
            var rightChildIndex = item.HeapIndex * 2 + 2;

            if (leftChildIndex >= _count)
                return;

            var swapIndex = leftChildIndex;

            if (rightChildIndex < _count && _items[leftChildIndex].CompareTo(_items[rightChildIndex]) < 0)
            {
                swapIndex = rightChildIndex;
            }

            if (item.CompareTo(_items[swapIndex]) >= 0)
                return;

            Swap(item, _items[swapIndex]);
        }
    }

    private void Swap(T left, T right)
    {
        _items[left.HeapIndex] = right;
        _items[right.HeapIndex] = left;
        (left.HeapIndex, right.HeapIndex) = (right.HeapIndex, left.HeapIndex);
    }
}