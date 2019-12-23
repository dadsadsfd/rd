using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace JetBrains.Collections
{
  /// <summary>
  /// A lock-free list that copies the underlying storage on all write operations. This allows for thread-safe operation with well-defined semantics at the cost of performance in write-heavy scenarios.
  /// Used instead of pulling the whole System.Collections.Immutable nuget package.
  /// </summary>
  /// <typeparam name="T">Type of elements contained in this list</typeparam>
  internal class CopyOnWriteList<T> : IList<T>
  {
    private T[] myStorage;

    /// <summary>
    /// Creates a new empty list.
    /// </summary>
    public CopyOnWriteList()
    {
      myStorage = new T[0];
    }

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator()
    {
      return ((IEnumerable<T>) myStorage).GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    private void Modify<TParam>(TParam param, Func<T[], TParam, T[]> action)
    {
      while (true)
      {
        var currentStorage = myStorage;
        var newArray = action(currentStorage, param);
        if (Interlocked.CompareExchange(ref myStorage, newArray, currentStorage) == currentStorage)
          break;
      }
    }

    /// <inheritdoc />
    public void Add(T item)
    {
      Modify(item, (currentArray, arg2) =>
      {
        var newArray = new T[currentArray.Length + 1];
        Array.Copy(currentArray, newArray, currentArray.Length);
        newArray[currentArray.Length] = arg2;
        return newArray;
      });
    }

    /// <inheritdoc />
    public void Clear()
    {
      Modify(0, ((_, __) => new T[0]));
    }

    /// <inheritdoc />
    public bool Contains(T item)
    {
      return Array.IndexOf(myStorage, item) != -1;
    }

    /// <inheritdoc />
    public void CopyTo(T[] array, int arrayIndex)
    {
      var currentStorage = myStorage;
      Array.Copy(currentStorage, 0, array, arrayIndex, currentStorage.Length);
    }

    /// <inheritdoc />
    public bool Remove(T item)
    {
      var result = false;
      Modify(item, (currentArray, arg2) =>
      {
        var indexOfEntry = Array.IndexOf(currentArray, arg2);
        if (indexOfEntry == -1)
        {
          result = false;
          return currentArray;
        }

        var newArray = new T[currentArray.Length - 1];
        Array.Copy(currentArray, newArray, indexOfEntry);
        Array.Copy(currentArray, indexOfEntry + 1, newArray, indexOfEntry, currentArray.Length - 1 - indexOfEntry);
        result = true;
        return newArray;
      });
      return result;
    }

    /// <inheritdoc />
    public int Count => myStorage.Length;
    
    /// <inheritdoc />
    public bool IsReadOnly => false;
    
    /// <inheritdoc />
    public int IndexOf(T item)
    {
      return Array.IndexOf(myStorage, item);
    }

    private struct ItemIndexPair
    {
      public readonly int Index;
      public readonly T Item;

      public ItemIndexPair(int index, T item)
      {
        Index = index;
        Item = item;
      }
    }

    /// <inheritdoc />
    public void Insert(int index, T item)
    {
      Modify(new ItemIndexPair(index, item), (currentArray, arg2) =>
      {
        var index2 = arg2.Index;
        var item2 = arg2.Item;
        var newArray = new T[currentArray.Length + 1];
        Array.Copy(currentArray, newArray, index2);
        Array.Copy(currentArray, index2, newArray, index2 + 1, currentArray.Length - index2);
        newArray[index2] = item2;
        return newArray;
      });
    }

    /// <inheritdoc />
    public void RemoveAt(int index)
    {
      Modify(index, (currentArray, arg2) =>
      {
        var indexOfEntry = arg2;
        var newArray = new T[currentArray.Length - 1];
        Array.Copy(currentArray, newArray, indexOfEntry);
        Array.Copy(currentArray, indexOfEntry + 1, newArray, indexOfEntry, currentArray.Length - 1 - indexOfEntry);
        return newArray;
      });
    }

    /// <inheritdoc />
    public T this[int index]
    {
      get => myStorage[index];
      set => Modify(new ItemIndexPair(index, value), ((currentArray, pair) =>
      {
        var newArray = new T[currentArray.Length];
        Array.Copy(currentArray, newArray, currentArray.Length);
        newArray[pair.Index] = pair.Item;
        return newArray;
      }));
    }
  }
}