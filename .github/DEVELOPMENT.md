# Development Guide

This page contains the steps to build and run the Syncfusion Toolkit for .NET MAUI repository from source. If you are looking to build apps with the Syncfusion Toolkit for .NET MAUI, please head over to the links in the [README](https://github.com/syncfusion/maui-toolkit/blob/main/README.md) to get started.

## Initial setup
   ### Windows
   - Install VS 17.10 or newer.
      - Follow [these steps](https://learn.microsoft.com/dotnet/maui/get-started/installation?tabs=vswin) to include MAUI.
   - If building iOS with pair to Mac, install current, stable Xcode on your Mac. Install from the [App Store](https://apps.apple.com/us/app/xcode/id497799835?mt=12) or [Apple Developer portal](https://developer.apple.com/download/more/?name=Xcode).
   - If you're missing any of the Android SDKs, Visual Studio should prompt you to install them. If it doesn't prompt you, then use the [Android SDK Manager](https://learn.microsoft.com/xamarin/android/get-started/installation/android-sdk) to install the necessary SDKs.
   - Install [Open JDK 17](https://learn.microsoft.com/en-us/java/openjdk/download#openjdk-17).

   ### Mac
   - Install [VSCode](https://code.visualstudio.com/download).
   - Follow the steps for installing the .NET MAUI Dev Kit for VS Code: https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-maui
      
## Building the Build Tasks
Before opening the solution in Visual Studio/VS Code, you **MUST** build the build tasks.

1. Open a command prompt/terminal/shell window.
1. Navigate to the location of your cloned `syncfusion/maui-toolkit` repo, for example:
     ```shell
     cd \repos\maui-toolkit
     ```
1. Run these commands:
    ```dotnetcli
    dotnet tool restore
    dotnet build ./Syncfusion.Maui.Toolkit.sln
    ```

## Windows

Open the `Syncfusion.Maui.Toolkit.sln` file in Visual Studio from the root of the repo.

## Mac

Open the root folder of the repository in VS Code.


## What branch should I use?

As a general rule:
- [main](https://github.com/syncfusion/maui-toolkit/tree/main)


## Sample projects

### Samples
 ```
├── samples 
│   ├── Syncfusion.Maui.ControlsGallery
│   ├── Syncfusion.Maui.Controls.Samples.Sandbox
│   │   ├── Maui.Controls.Sample.Sandbox
```

- *Syncfusion.Maui.ControlsGallery*: Full gallery sample with all of the controls and features of the Syncfusion .NET MAUI Toolkit.
- *Syncfusion.Maui.Samples.Sandbox*: Empty project useful for testing reproductions or use cases.
