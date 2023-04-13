using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace InstaTracker.Helpers;

public class SObservableCollection<T> : ObservableCollection<T>
{
    public void AddRange(IEnumerable<T> items)
    {
        CheckReentrancy();

        foreach (T item in items)
            Items.Add(item);

        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}