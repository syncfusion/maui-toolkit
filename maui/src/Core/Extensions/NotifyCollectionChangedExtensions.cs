using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Extension for common NotifyCollectionChanges. 
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsExtensions
    {
        public static void ApplyCollectionChanges(this NotifyCollectionChangedEventArgs self, Action<object, int, bool> insertAction, Action<object, int> removeAction, Action resetAction)
        {
            switch (self.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (self.NewStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.NewItems!.Count; i++)
                        insertAction(self.NewItems[i]!, i + self.NewStartingIndex, true);

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (self.NewStartingIndex < 0 || self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems!.Count; i++)
                        removeAction(self.OldItems[i]!, self.OldStartingIndex);

                    int insertIndex = self.NewStartingIndex;
                    if (self.OldStartingIndex < self.NewStartingIndex)
                        insertIndex -= self.OldItems.Count - 1;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        insertAction(self.OldItems[i]!, insertIndex + i, false);

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems!.Count; i++)
                        removeAction(self.OldItems[i]!, self.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (self.OldStartingIndex < 0 || self.OldItems!.Count != self.NewItems!.Count)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                    {
                        removeAction(self.OldItems[i]!, i + self.OldStartingIndex);
                        insertAction(self.NewItems[i]!, i + self.OldStartingIndex, true);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        resetAction();
                        break;
                    }
            }
        }
    }
}
