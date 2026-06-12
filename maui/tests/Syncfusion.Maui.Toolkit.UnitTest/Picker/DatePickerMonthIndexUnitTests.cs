using Syncfusion.Maui.Toolkit.Picker;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Syncfusion.Maui.Toolkit.UnitTest;

public class DatePickerMonthIndexUnitTests : BaseUnitTest
{
	[Fact]
	public void ArrayIndexOf_ReturnsCorrectIndex_ForFullMonthNames()
	{
		string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;

		for (int i = 0; i < 12; i++)
		{
			int index = Array.IndexOf(monthNames, monthNames[i]);
			Assert.Equal(i, index);
		}
	}

	[Fact]
	public void ArrayIndexOf_ReturnsCorrectIndex_ForAbbreviatedMonthNames()
	{
		string[] abbrevMonthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;

		for (int i = 0; i < 12; i++)
		{
			int index = Array.IndexOf(abbrevMonthNames, abbrevMonthNames[i]);
			Assert.Equal(i, index);
		}
	}

	[Fact]
	public void ArrayIndexOf_ReturnsMinusOne_ForInvalidMonth()
	{
		string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
		int index = Array.IndexOf(monthNames, "InvalidMonth");
		Assert.Equal(-1, index);
	}

	[Fact]
	public void ArrayIndexOf_MatchesListIndexOf_ForAllMonths()
	{
		string[] monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames;
		var monthList = monthNames.ToList();

		for (int i = 0; i < monthNames.Length; i++)
		{
			int arrayResult = Array.IndexOf(monthNames, monthNames[i]);
			int listResult = monthList.IndexOf(monthNames[i]);
			Assert.Equal(listResult, arrayResult);
		}
	}

	[Fact]
	public void ArrayIndexOf_MatchesListIndexOf_ForAbbreviatedMonths()
	{
		string[] abbrevMonthNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedMonthNames;
		var monthList = abbrevMonthNames.ToList();

		for (int i = 0; i < abbrevMonthNames.Length; i++)
		{
			int arrayResult = Array.IndexOf(abbrevMonthNames, abbrevMonthNames[i]);
			int listResult = monthList.IndexOf(abbrevMonthNames[i]);
			Assert.Equal(listResult, arrayResult);
		}
	}
}
