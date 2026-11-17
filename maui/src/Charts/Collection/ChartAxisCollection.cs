using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a collection of <see cref="ChartAxisRangeStyle"/> objects.
	/// </summary>
	public class ChartAxisRangeStyleCollection : ObservableCollection<ChartAxisRangeStyle>
	{
		/// <summary>
		/// Raised whenever an item is added, removed, replaced, or any tracked
		/// item raises its own <see cref="ChartAxisRangeStyle.StyleValueChanged"/> event.
		/// </summary>
		internal event NotifyCollectionChangedEventHandler? RangeStyleChanged;

		/// <summary>
		/// Raises the <see cref="ObservableCollection{T}.CollectionChanged"/> event, re-subscribes item style-change
		/// notifications, and raises <see cref="RangeStyleChanged"/> so the owner axis redraws on membership
		/// and property changes.
		/// </summary>
		/// <param name="e">The event data describing the collection change.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (e == null)
			{
				return;
			}

			if (e.NewItems != null)
			{
				foreach (ChartAxisRangeStyle item in e.NewItems)
				{
					if (item != null)
					{
						item.StyleValueChanged += OnItemStyleValueChanged;
					}
				}
			}

			if (e.OldItems != null)
			{
				foreach (ChartAxisRangeStyle item in e.OldItems)
				{
					if (item != null)
					{
						item.StyleValueChanged -= OnItemStyleValueChanged;
					}
				}
			}

			base.OnCollectionChanged(e);

			// Membership changed (Add/Remove/Replace/Reset): notify the axis to update its layout,
			// exactly matching the notifications previously raised only for item property changes.
			RangeStyleChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		/// <summary>
		/// Removes all elements from the collection and detaches their style-change notifications.
		/// </summary>
		protected override void ClearItems()
		{
			foreach (ChartAxisRangeStyle item in Items)
			{
				item.StyleValueChanged -= OnItemStyleValueChanged;
			}

			base.ClearItems();
		}

		private void OnItemStyleValueChanged(object? sender, EventArgs e)
		{
			RangeStyleChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}
	}
}