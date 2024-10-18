# Development Guide

This page contains the steps to build and run the Syncfusion Toolkit for .NET MAUI repository from source. If you are looking to build apps with the Syncfusion Toolkit for .NET MAUI, please refer to the links in the [README](https://github.com/syncfusion/maui-toolkit/blob/main/README.md) to get started.

## Initial Setup

### Windows
1. **Install Visual Studio 17.10 or newer.**
   - Follow [these steps](https://learn.microsoft.com/dotnet/maui/get-started/installation?tabs=vswin) to include .NET MAUI.
2. **If building iOS with Pair to Mac:**
   - Install the latest stable version of Xcode on your Mac. You can download it from the [App Store](https://apps.apple.com/us/app/xcode/id497799835?mt=12) or the [Apple Developer portal](https://developer.apple.com/download/more/?name=Xcode).
3. **Install Android SDKs:**
   - If you're missing any of the Android SDKs, Visual Studio should prompt you to install them. If it doesn’t prompt you, use the [Android SDK Manager](https://learn.microsoft.com/xamarin/android/get-started/installation/android-sdk) to install the necessary SDKs.
4. **Install Open JDK 17:**
   - Follow the instructions at [OpenJDK 17 Download](https://learn.microsoft.com/en-us/java/openjdk/download#openjdk-17).

### Mac
1. **Install Visual Studio Code:**
   - Download it from [VSCode Download](https://code.visualstudio.com/download).
2. **Install the .NET MAUI Dev Kit for VS Code:**
   - Follow the steps in the marketplace link: [Install MAUI Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-maui).

## Building the Build Tasks

Before opening the solution in Visual Studio or VS Code, **you MUST build the build tasks**.

1. Open a command prompt, terminal, or shell window.
2. Navigate to the location of your cloned `syncfusion/maui-toolkit` repository. For example:
   ```shell
   cd \repos\maui-toolkit
   ```
3. Run the following commands:
   ```shell
   dotnet tool restore
   dotnet build ./Syncfusion.Maui.Toolkit.sln
   ```

## Opening the Solution

### Windows
- Open the `Syncfusion.Maui.Toolkit.sln` file in Visual Studio from the root of the repository.

### Mac
- Open the root folder of the repository in Visual Studio Code.

## Branch Information

As a general rule, use the [main branch](https://github.com/syncfusion/maui-toolkit/tree/main) for development.

## Sample Projects

### Samples Directory
```
├── samples 
│   ├── Syncfusion.Maui.ControlsGallery
│   ├── Syncfusion.Maui.Controls.Samples.Sandbox
```

- **Syncfusion.Maui.ControlsGallery**: A full gallery sample showcasing all the controls and features of the Syncfusion .NET MAUI Toolkit.
- **Syncfusion.Maui.Controls.Samples.Sandbox**: An empty project useful for testing reproductions or use cases.