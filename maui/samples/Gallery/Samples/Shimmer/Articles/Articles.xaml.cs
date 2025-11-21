using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Shimmer.SfShimmer
{
	public partial class Articles : SampleView
	{
		readonly IDispatcherTimer _timer;
		public Articles()
		{
			InitializeComponent();
			_timer = Dispatcher.CreateTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(3000);
			_timer.Tick += Timer_Tick;
			_timer.Start();
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (listView == null || shimmer == null)
			{
				return;
			}
			if (DeviceInfo.Current.Platform == DevicePlatform.WinUI || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
			{
				height = 600;
			}

		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			shimmer.IsActive = false;
			_timer.Stop();
		}

		public override void OnDisappearing()
		{
			_timer.Stop();
			_timer.Tick -= Timer_Tick;
		}
	}

	public class BookViewModel
	{
		private ObservableCollection<BookInfo> _info1;

		public ObservableCollection<BookInfo> Info1
		{
			get { return _info1; }
			set { _info1 = value; }
		}

		public BookViewModel()
		{
			_info1 =
		[
			new BookInfo() { BookImage = "book0.png",Author = "James McCaffrey", BookName = "Neural Networks Using C#", Summary = "Neural networks are an exciting field of software development used to calculate outputs from input data." },
			new BookInfo() { BookImage = "book1.png",Author = "Dirk Strauss", BookName = "C# Code Contracts", Summary = "Code Contracts provide a way to convey code assumptions in your .NET applications. In C# Code Contracts Succinctly, author Dirk Strauss explains how to use Code Contracts to validate logical correctness in code." },
			new BookInfo() { BookImage = "book2.png",Author = "James McCaffrey", BookName = "Machine Learning Using C#", Summary = "In Machine Learning Using C# Succinctly, you'll learn several different approaches to applying machine learning to data analysis and prediction problems." },
			new BookInfo() { BookImage = "book3.png",Author = "Mark Twain", BookName = "Entity Framework Code First", Summary = "Follow author Ricardo Peres as he introduces the newest development mode for Entity Framework, Code First." },
			new BookInfo() { BookImage = "book4.png",Author = "Joseph Conrad", BookName = "SQL Server for C# Developers", Summary = "Developers of C# applications with a SQL Server database can learn to connect to a database using classic ADO.NET and look at different methods of developing databases using the Entity Framework." },
			new BookInfo() { BookImage = "book5.png",Author = "Nathaniel Hawthorne", BookName = "Assembly Language", Summary = "Assembly language is as close to writing machine code as you can get without writing in pure hexadecimal." },
		];
		}
	}

	public partial class BookInfo : INotifyPropertyChanged
	{
		#region Fields

		private string? _bookName;
		private string? _bookImage;
		private string? _summary;
		private string? _author;

		public BookInfo(string? author)
		{
			_author = author;
		}

		#endregion

		#region Constructor

		public BookInfo()
		{

		}

		#endregion

		#region Public Properties

		public string? BookName
		{
			get { return _bookName; }
			set
			{
				_bookName = value;
				RaisePropertyChanged("ContactName");
			}
		}

		public string? Author
		{
			get { return _author; }
			set
			{
				_author = value;
				RaisePropertyChanged("Author");
			}
		}

		public string? Summary
		{
			get { return _summary; }
			set
			{
				_summary = value;
				RaisePropertyChanged("Message");
			}
		}

		public string? BookImage
		{
			get { return _bookImage; }
			set
			{
				if (value != null)
				{
					_bookImage = value;
					RaisePropertyChanged("ContactImage");
				}
			}
		}

		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler? PropertyChanged;

		private void RaisePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}