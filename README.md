![NET_MAUI__Toolkit_Banner](https://cdn.syncfusion.com/content/images/maui/maui-toolkit--controls-banner.png)

# Syncfusion® Toolkit for .NET MAUI

The [Syncfusion® Toolkit for .NET MAUI](https://www.syncfusion.com/net-maui-toolkit?utm_source=msftdotnet&utm_medium=banner&utm_campaign=mauipremium_sep25) is a high-performance collection of UI controls designed to streamline cross-platform app development across Android, iOS, macOS, and Windows. With this toolkit, developers can deliver beautiful, feature-rich applications with minimal effort, cutting down development time while ensuring a seamless and engaging user experience across platforms.

The Syncfusion® Toolkit is built with community collaboration in mind, aiming to incorporate user feedback and contributions. It is the perfect companion for developers looking to build engaging cross-platform applications faster and more efficiently using the Syncfusion® ecosystem.

[![NuGet version](https://img.shields.io/nuget/v/Syncfusion.Maui.Toolkit)](https://www.nuget.org/packages/Syncfusion.Maui.Toolkit)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Syncfusion.Maui.Toolkit)](https://www.nuget.org/stats/packages/Syncfusion.Maui.Toolkit?groupby=Version)
[![License](https://img.shields.io/github/license/syncfusion/maui-toolkit)](./LICENSE.txt)
![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Platform](https://img.shields.io/badge/platform-Android%20%7C%20iOS%20%7C%20Mac%20%7C%20Windows-blue)
[![.NET 9](https://img.shields.io/badge/.NET%209-5C2D91?logo=.net&logoColor=white)](https://dotnet.microsoft.com/download/dotnet/9.0)
![Contributions](https://img.shields.io/badge/contributions-welcome-green.svg)
![Contributors](https://img.shields.io/github/contributors/syncfusion/maui-toolkit)
![GitHub issues](https://img.shields.io/github/issues/syncfusion/maui-toolkit)
![GitHub pull requests](https://img.shields.io/github/issues-pr/syncfusion/maui-toolkit)

![NET_MAUI_Toolkit_product](https://cdn.syncfusion.com/content/images/maui/maui-toolkit--controls.png)

## Getting Started ##

* [Install .NET MAUI](https://dot.net/maui)
* [Syncfusion .NET MAUI Toolkit Documentation](https://help.syncfusion.com/maui-toolkit/introduction/overview)
* [Development Guide](./.github/DEVELOPMENT.md)


## Controls list

| **Category**         | **Control**           | **Description**                                                                                         |
|----------------------|-----------------------|---------------------------------------------------------------------------------------------------------|
| Data Visualization   | Cartesian Charts      | Versatile data representation using line, bar, and area charts.                                          |
|                      | Circular Charts       | Display proportions and comparisons using pie and doughnut charts.                                       |
|                      | Funnel Charts         | Represent processes and data flow, often used in sales and analytics.                                    |
|                      | Polar Charts          | Showcase categories in a circular format, ideal for unordered data.                                      |
|                      | Pyramid Charts        | Visualize hierarchical data, perfect for business and analytics applications.                            |
|                      | Spark Charts          | Display trends using micro charts like line, column, area, and win-loss. Ideal for compact data summaries. |
|                      | Sunburst Charts       | Visualize hierarchical data using concentric circles, ideal for multi-level category data.               |
| Calendars            | Calendar              | The calendar supports month, year, decade, and century views, multiple selection modes, and customizable appearance.      |
| Editors              | Date Picker           | User interface element for selecting specific dates.                                                     |
|                      | Date Time Picker      | Combined interface to select dates and times.                                                            |
|                      | Numeric Entry         | Advanced control for numeric input with support for various formats and validation.                      |
|                      | Numeric Up Down       | Adjust values with up/down buttons and culture-specific formatting.                                      |
|                      | OTP Input             | Secure input field for one-time passwords with customizable length, masking options, and validation.     |
|                      | Picker               | Versatile UI Element for making selections in set of options.                                             |
|                      | Time Picker           | Interface for choosing specific times of day.                                                            |
| Navigation           | Bottom Sheet          | Slides up from the bottom of the screen to display additional content or functionality.                  |
|                      | Navigation Drawer     | Slide-in menu for navigation, positionable on any side of the app with customizable animations.          |
|                      | Tab View              | Organize app content with customizable tabs, enabling easy navigation across sections.                   |
| Layout               | Accordion             | Organizes content into multiple expandable sections, allowing only one section to be open at a time for better navigation and space management.|
|                      | Cards                 | Create dismissible cards or a stack of cards, and customize their background, borders, and corners.      |
|                      | Carousel              | Smooth, touch-enabled sliding galleries for showcasing images or featured content.                       |
|                      | Expander              | Allows users to expand or collapse content dynamically, improving space management and user experience.  |
|                      | Popup                 | Allows users to display an alert message with customizable buttons or load any desired content inside a popup view.|
|                      | Text Input Layout     | Enhances input fields with floating labels and validation, improving user interaction.                   |
| Buttons              | Button                | Customizable button control with icon support, background images, and visual state styling.              |
|                      | Chips                 | Interactive tags for filtering, labeling, or visual options, perfect for e-commerce or task management.  |
|                      | Segmented Control     | Quickly switch between views or categories, ideal for apps with multiple layout options.                 |
| Notification         | Circular Progress Bar | Represents task progression through a circular visualization.                                            |
|                      | Linear Progress Bar   | Represents task progression through a linear visualization.                                              |
|                      | Pull to Refresh       | Allows users to refresh live data by pulling down, ideal for real-time data syncing.                     |
| Miscellaneous        | Effects View          | Add visual enhancements like shadows, blurs, or highlights to make UI elements stand out.                |
|                      | Shimmer               | Indicates loading content with customizable wave directions, great for data-heavy apps.                  |


## Installation

You can install the [Syncfusion® Toolkit for .NET MAUI](https://www.nuget.org/packages/Syncfusion.Maui.Toolkit) via NuGet: 

```
dotnet add package Syncfusion.Maui.Toolkit
```
Alternatively, add it directly in your `.csproj` file:

```xml
<PackageReference Include="Syncfusion.Maui.Toolkit" Version="x.x.x" />
```

## Configure Syncfusion® Toolkit

In order to use the Syncfusion® .NET MAUI Toolkit you need to call the extension method in your MauiProgram.cs file as follows:

**MauiProgram.cs**

```csharp
using Syncfusion.Maui.Toolkit.Hosting;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
		.UseMauiApp<App>()
		// Initialize the Syncfusion .NET MAUI Toolkit by adding the below line of code
		.ConfigureSyncfusionToolkit()
		// After initializing the Syncfusion .NET MAUI Toolkit, optionally add additional fonts
		.ConfigureFonts(fonts =>
		{
			fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
		});

		// Continue initializing your .NET MAUI App here

		return builder.Build();
	}
}
```

### XAML usage

In order to make use of the toolkit within XAML you can use this namespace:

```xml
xmlns:toolkit="http://schemas.syncfusion.com/maui/toolkit"
```

## Usage Example

Here’s a quick example to get you started with one of the controls, such as the Cartesian Chart:

The following XAML code demonstrates how to set up a basic `SfCartesianChart` using the Syncfusion® MAUI Toolkit. This code snippet should be included in the `MainPage.xaml` file of your MAUI project. It sets up the necessary namespaces, binds the `ViewModel` to the `ContentPage`, and configures the `SfCartesianChart` with `CategoryAxis` for the X-axis and `NumericalAxis` for the Y-axis. The creation of the `ViewModel` will be explained in the following section.

**MainPage.xaml**

```xml
<ContentPage 
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="ChartGettingStarted.MainPage"
    xmlns:chart="clr-namespace:Syncfusion.Maui.Toolkit.Charts;assembly=Syncfusion.Maui.Toolkit"
    xmlns:model="clr-namespace:ChartGettingStarted">
    
	<!-- Set the BindingContext to the ViewModel -->
    <ContentPage.BindingContext>
        <model:ViewModel/>
    </ContentPage.BindingContext>

    <!-- Define a Syncfusion Cartesian Chart to visualize data -->
    <chart:SfCartesianChart>

        <!-- Set the title for the Cartesian chart -->
        <chart:SfCartesianChart.Title>
            <Label Text="Height Comparison" HorizontalOptions="Center" />
        </chart:SfCartesianChart.Title>

        <!-- Add a legend to the chart for identifying series -->
        <chart:SfCartesianChart.Legend>
            <chart:ChartLegend />
        </chart:SfCartesianChart.Legend>

        <!-- Define the horizontal (X) axis as a category axis -->
        <chart:SfCartesianChart.XAxes>
            <chart:CategoryAxis>
                <!-- Set the title for the X-axis -->
                <chart:CategoryAxis.Title>
                    <chart:ChartAxisTitle Text="Name" />
                </chart:CategoryAxis.Title>
            </chart:CategoryAxis>
        </chart:SfCartesianChart.XAxes>

        <!-- Define the vertical (Y) axis as a numerical axis -->
        <chart:SfCartesianChart.YAxes>
            <chart:NumericalAxis>
                <!-- Set the title for the Y-axis -->
                <chart:NumericalAxis.Title>
                    <chart:ChartAxisTitle Text="Height(in cm)" />
                </chart:NumericalAxis.Title>
            </chart:NumericalAxis>
        </chart:SfCartesianChart.YAxes>

        <!-- Initialize a ColumnSeries to visualize the data in columns -->
        <chart:ColumnSeries Label="Height"
                         EnableTooltip="True" 
                         ShowDataLabels="True" 
                         ItemsSource="{Binding Data}" 
                         XBindingPath="Name" 
                         YBindingPath="Height">
            <!-- Define the settings for data labels of the columns -->
            <chart:ColumnSeries.DataLabelSettings>
                <chart:CartesianDataLabelSettings LabelPlacement="Inner" />
            </chart:ColumnSeries.DataLabelSettings>
        </chart:ColumnSeries>

    </chart:SfCartesianChart>
</ContentPage>

```

Define a simple data model C# class named `Person` to represent a data point, such as a person with a name and height, in your application.

**Person.cs**

```csharp
    /// <summary>
    /// Represents a person with a name and height.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the height of the person.
        /// </summary>	
        public double Height { get; set; }
    }
```

Next, create a ViewModel class in C# and initialize it with a list of `Person` objects:

**ViewModel.cs**

```csharp
    /// <summary>
    /// ViewModel class that provides a list of Person objects for data binding.
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Gets or sets the list of Person objects.
        /// </summary>	
        public List<Person> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the ViewModel class with sample data.
        /// </summary>
        public ViewModel()
        {
            // Initialize the Data property with a list of Person objects
            Data = new List<Person>()
            {
                new Person { Name = "David", Height = 170 },
                new Person { Name = "Michael", Height = 96 },
                new Person { Name = "Steve", Height = 65 },
                new Person { Name = "Joel", Height = 182 },
                new Person { Name = "Bob", Height = 134 }
            };
        }
    }
```

## Support

For any other queries, reach our [Syncfusion support team](https://mauitoolkit.syncfusion.com).

## Contributing
Contributions are welcome! If you'd like to contribute, please check out our [contributing guide](./.github/CONTRIBUTING.md) for details on how to get started. Whether you find a bug, have a feature request, or want to submit code, we appreciate your help in improving the toolkit.

See the [Development Guide](./.github/DEVELOPMENT.md) for more details about this repository and project structure.

<a href="https://github.com/syncfusion/maui-toolkit/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=syncfusion/maui-toolkit" />
</a>

## About Syncfusion®
Founded in 2001 and headquartered in Research Triangle Park, N.C., Syncfusion® has more than 35,000 customers and more than 1 million users, including large financial institutions, Fortune 500 companies, and global IT consultancies.
 
Today, we provide 1800+ components and frameworks for web ([Blazor](https://www.syncfusion.com/blazor-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [ASP.NET Core](https://www.syncfusion.com/aspnet-core-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [ASP.NET MVC](https://www.syncfusion.com/aspnet-mvc-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [Angular](https://www.syncfusion.com/angular-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [React](https://www.syncfusion.com/react-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [Vue](https://www.syncfusion.com/vue-ui-components?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), and [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget)), mobile ([.NET MAUI](https://www.syncfusion.com/maui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [Xamarin](https://www.syncfusion.com/xamarin-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), and [JavaScript](https://www.syncfusion.com/javascript-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget)), and desktop development ([WinForms](https://www.syncfusion.com/winforms-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [WPF](https://www.syncfusion.com/wpf-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [WinUI](https://www.syncfusion.com/winui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget), [Flutter](https://www.syncfusion.com/flutter-widgets?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget) and [UWP](https://www.syncfusion.com/uwp-ui-controls?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget)).
___

[sales@syncfusion.com](mailto:sales@syncfusion.com?Subject=Syncfusion%20Maui%toolkit%20-%20NuGet) | [www.syncfusion.com](https://www.syncfusion.com?utm_source=nuget&utm_medium=listing&utm_campaign=maui-toolkit-nuget) | Toll Free: 1-888-9 DOTNET
