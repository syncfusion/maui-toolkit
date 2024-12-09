using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery
{

	/// <summary>
	/// 
	/// </summary>
	public partial class CardLayoutModel : Element
	{
		/// <summary>
		/// 
		/// </summary>
		public bool IsApplicable { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String? Title { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<SampleModel>? Samples { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String? StatusTag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public CardLayoutModel()
		{
			IsApplicable = true;
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty =
		   BindableProperty.Create("IsSelected", typeof(bool), typeof(CardLayoutModel), false);

		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public partial class SampleSubCategoryModel : Element
	{
		/// <summary>
		/// 
		/// </summary>
		public String? SubCategoryName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsApplicable { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool IsSubCategory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<CardLayoutModel>? CardLayouts { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String? StatusTag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty IsSubCategoryClickedProperty =
		   BindableProperty.Create("IsSubCategoryClicked", typeof(bool), typeof(SampleSubCategoryModel), false);

		/// <summary>
		/// 
		/// </summary>
		public bool IsSubCategoryClicked
		{
			get => (bool)GetValue(IsSubCategoryClickedProperty);
			set => SetValue(IsSubCategoryClickedProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public SampleSubCategoryModel()
		{
			IsSubCategory = true;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public partial class SampleCategoryModel : Element
	{
		/// <summary>
		/// 
		/// </summary>
		public String? CategoryName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string CollapseImage
		{
			get => (string)GetValue(CollapseImageProperty);
			set => SetValue(CollapseImageProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty CollapseImageProperty =
			BindableProperty.Create(nameof(CollapseImage), typeof(string), typeof(SampleCategoryModel));



		/// <summary>
		/// 
		/// </summary>
		public SampleSubCategoryModel SelectedCategory
		{
			get => (SampleSubCategoryModel)GetValue(SelectedCategoryProperty);
			set => SetValue(SelectedCategoryProperty, value);
		}



		/// <summary>
		/// 
		/// </summary>
		// Using a DependencyProperty as the backing store for IsCollapsed.  This enables animation, styling, binding, etc...
		public static readonly BindableProperty SelectedCategoryProperty =
			BindableProperty.Create("SelectedCategory", typeof(SampleSubCategoryModel), typeof(SampleCategoryModel), null);

		/// <summary>
		/// 
		/// </summary>
		public bool IsCollapsed
		{
			get => (bool)GetValue(IsCollapsedProperty);
			set => SetValue(IsCollapsedProperty, value);
		}



		/// <summary>
		/// 
		/// </summary>
		// Using a DependencyProperty as the backing store for IsCollapsed.  This enables animation, styling, binding, etc...
		public static readonly BindableProperty IsCollapsedProperty =
			BindableProperty.Create("IsCollapsed", typeof(bool), typeof(SampleCategoryModel), true);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty IsSelectedProperty =
		   BindableProperty.Create("IsSelected", typeof(bool), typeof(SampleCategoryModel), false);

		/// <summary>
		/// Property represents whether the Status Tag should visible for Category
		/// </summary>
		public static readonly BindableProperty CategoryStatusTagProperty =
		  BindableProperty.Create("CategoryStatusTag", typeof(bool), typeof(SampleCategoryModel), false);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty IsCategoryClickedProperty =
		   BindableProperty.Create("IsCategoryClicked", typeof(bool), typeof(SampleCategoryModel), false);

		/// <summary>
		/// 
		/// </summary>
		public bool IsCategoryClicked
		{
			get => (bool)GetValue(IsCategoryClickedProperty);
			set => SetValue(IsCategoryClickedProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool HasCategory { get; set; }

		/// <summary>
		/// Property represents whether the Status Tag should visible for Category
		/// </summary>
		public bool CategoryStatusTag
		{
			get => (bool)GetValue(CategoryStatusTagProperty);
			set => SetValue(CategoryStatusTagProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<SampleSubCategoryModel>? SampleSubCategories { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public String? StatusTag { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public SampleCategoryModel()
		{
			IsCollapsed = false;

			HasCategory = true;

			CollapseImage = "\ue708";
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class ControlCategoryModel
	{
		/// <summary>
		/// 
		/// </summary>
		public String? Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ObservableCollection<ControlModel>? AllControls { get; set; }

	}

	/// <summary>
	/// 
	/// </summary>
	public class MainPageListModel
	{
		/// <summary>
		/// 
		/// </summary>
		public Object? objValue { get; set; }
	}

}
