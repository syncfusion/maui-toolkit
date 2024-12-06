namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class Person
	{
		public Address? Address { get; set; }

		public Address[]? Addresses { get; set; }
	}
	public class Address
	{
		public string? City { get; set; }
	}
	public class ModelTestCase()
	{
		public double XValue { get; set; }
		public double YValue { get; set; }
		public DateTime DateValue { get; set; }

	}
	public class TestClass
	{
		public int IntProperty { get; set; }
		public int[] IntArrayProperty { get; set; } = Array.Empty<int>();
	}
}
