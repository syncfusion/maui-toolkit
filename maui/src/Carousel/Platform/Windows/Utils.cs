using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	#region ItemLoadedArgs

	/// <summary>
	/// Delegate event used to declare the event
	/// </summary>
	/// <param name="sender">sender refers to the object that invoked the event that fired the event handler</param>
	/// <param name="e"> you can add whatever data you need to pass to your event handlers</param>
	internal delegate void ItemLoadedHandler(object sender, ItemLoadedArgs e);

	/// <summary>
	/// Items Loaded Args has inherited from EventArgs 
	/// </summary>
	internal class ItemLoadedArgs : EventArgs
	{
		/// <summary>
		/// initialize the new value of index.
		/// </summary>
		private int _index;

		/// <summary>
		/// initialize the new value of item
		/// </summary>
		private object? _item;

		/// <summary>
		/// Gets the name of the Index.
		/// </summary>
		public int Index
		{
			get { return _index; }
			internal set { _index = value; }
		}

		/// <summary>
		/// Gets the name of the Item.
		/// </summary>
		public object? Item
		{
			get { return _item; }
			internal set { _item = value; }
		}
	}

	#endregion

	#region ItemsCollectionChangedArgs

	/// <summary>
	/// Delegate event used to declare the event
	/// </summary>
	/// <param name="sender">sender refers to the object that invoked the event that fired the event handler</param>
	/// <param name="e"> you can add whatever data you need to pass to your event handlers</param>
	internal delegate void ItemsCollectionChangedHandler(object sender, ItemsCollectionChangedArgs e);

	/// <summary>
	/// ItemsCollectionChangedArgs has derived from EventArgs
	/// </summary>
	internal class ItemsCollectionChangedArgs : EventArgs
	{
		/// <summary>
		/// collection is a object which is created from the ItemCollection
		/// </summary>
		private ItemCollection? _collection;

		/// <summary>
		/// Initializes a new instance of the <see cref="ItemsCollectionChangedArgs"/> class.
		/// </summary>
		/// <param name="item">item to be added.</param>
		public ItemsCollectionChangedArgs(ItemCollection item)
		{
			Collection = item;
		}

		/// <summary>
		/// Gets changed items collection
		/// </summary>
		public ItemCollection? Collection
		{
			get { return _collection; }
			internal set { _collection = value; }
		}
	}

	#endregion

	#region Selector

	/// <summary>
	/// Represents a class for defining the selection properties.
	/// </summary>
	/// <exclude/>
	public partial class Selector : ItemsControl
	{
		#region Bindable properties

		/// <summary>
		/// Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(Selector), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemChanged)));

		/// <summary>
		/// Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
		/// </summary>
		public static readonly DependencyProperty SelectedIndexProperty =
			DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Selector), new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexChanged)));

		#endregion

		#region Fields

		private int _oldIndex = -1;

		private int _newIndex = -1;

		#endregion Variables

		#region Events

		/// <summary>
		/// Occurs when the selection has changed
		/// </summary>
		public event SelectionChangedEventHandler? SelectionChanged;

		#endregion Events

		#region Properties

		/// <summary>
		/// Gets or sets the index for the selected item.
		/// </summary>
		/// <value>
		/// The default value is -1
		/// </value>
		public int SelectedIndex
		{
			get { return (int)GetValue(SelectedIndexProperty); }
			set { SetValue(SelectedIndexProperty, value); }
		}

		/// <summary>
		/// Gets or sets the selected item.
		/// </summary>
		public object? SelectedItem
		{
			get
			{
				return (object)GetValue(SelectedItemProperty);
			}

			set
			{
				SetValue(SelectedItemProperty, value);
			}
		}

		#endregion Properties

		#region Override Methods

		/// <summary>
		/// Initializes the SelectedItem property.
		/// </summary>
		/// <exclude/>
		protected override void OnApplyTemplate()
		{
			if (Items.Count > 0 && (SelectedIndex >= Items.Count || SelectedIndex < -1) && ItemsSource != null)
			{
				//throw new ArgumentOutOfRangeException("Specified argument was out of the range of valid values.Parameter name: SelectedIndex");
			}
			if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
			{
				SelectedItem = Items[SelectedIndex];
			}
			else
			{
				SelectedItem = null;
			}
			base.OnApplyTemplate();
		}

		#endregion Override Methods

		#region Private Methods

		/// <summary>
		/// Called when the Selector SelectedItem Property changed.
		/// </summary>
		/// <param name="args">The DependencyPropertyChangedEventArgs instance containing the event data.</param>
#pragma warning disable IDE0060 // Remove unused parameter
		protected void OnSelectedItemChanged(DependencyPropertyChangedEventArgs args)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (SelectedIndex == -1 || SelectedIndex != _newIndex)
			{
				if (SelectedItem != null)
				{
					if (SelectedItem.Equals(DataContext))
					{
						SelectedIndex = IndexFromContainer(SelectedItem as DependencyObject);
					}
					else
					{
						SelectedIndex = Items.IndexOf(SelectedItem);
					}
				}
				else
				{
					SelectedIndex = -1;
				}
			}
		}

		/// <summary>
		/// Called when the Selector SelectedItem Property changed.
		/// </summary>
		/// <param name="args">The DependencyPropertyChangedEventArgs instance containing the event data.</param>
		protected virtual void OnSelectionChanged(DependencyPropertyChangedEventArgs args)
		{
			if (SelectionChanged != null)
			{
				IList<object> oldItems = [];
				IList<object> newItems = [];
				oldItems.Add(args.OldValue);
				newItems.Add(args.NewValue);

				Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs selectionargs = new Microsoft.UI.Xaml.Controls.SelectionChangedEventArgs(oldItems, newItems);
				SelectionChanged(this, selectionargs);
			}
		}

		/// <summary>
		/// Called when the Selector SelectedIndex Property changed.
		/// </summary>
		/// <param name="args">The DependencyPropertyChangedEventArgs instance containing the event data.</param>
		protected void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs args)
		{
			_oldIndex = Convert.ToInt32(args.OldValue);
			_newIndex = Convert.ToInt32(args.NewValue);
			if (SelectedIndex >= 0 && SelectedIndex < Items.Count)
			{
				SelectedItem = Items[SelectedIndex];
			}
			else
			{
				SelectedItem = null;
			}
		}

		/// <summary>
		/// Called when the Selector SelectedItem Property changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="args">The DependencyPropertyChangedEventArgs instance containing the event data.</param>
		private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			Selector? instance = obj as Selector;
			if (instance != null)
			{
				instance.OnSelectedItemChanged(args);
				instance.OnSelectionChanged(args);
			}
		}
		/// <summary>
		/// Called when the Selector SelectedIndex Property changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="args">The DependencyPropertyChangedEventArgs instance containing the event data.</param>
		private static void OnSelectedIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			Selector? instance = obj as Selector;
			instance?.OnSelectedIndexChanged(args);
		}
		#endregion Callback Methods

	}

	#endregion
}
