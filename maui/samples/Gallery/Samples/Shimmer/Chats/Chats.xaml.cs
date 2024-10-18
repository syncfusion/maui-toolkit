using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Shimmer.SfShimmer;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Chats : SampleView
{
    IDispatcherTimer timer;

    public Chats()
    {
        InitializeComponent();
        timer = Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromMilliseconds(3000);
        timer.Tick += Timer_Tick;
        timer.Start();
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
            height = 570;
        }

        listView.RowHeight = (int)(height / shimmer.RepeatCount);
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        shimmer.IsActive = false;
        timer.Stop();
    }

    public override void OnDisappearing()
    {
        timer.Stop();
        timer.Tick -= Timer_Tick;
    }
}


public class ViewModel
{
    private ObservableCollection<ContactInfo> info;

    public ObservableCollection<ContactInfo> Info
    {
        get { return info; }
        set { info = value; }
    }

    public ViewModel()
    {
        info = new ObservableCollection<ContactInfo>
        {
            new ContactInfo() { ContactImage = "people_circle17.png", ContactName = "Brenda", Message = "Hey, how's it going? What have you been up to lately?" },
            new ContactInfo() { ContactImage = "people_circle9.png", ContactName = "Jennifer", Message = "Do you have any plans for the weekend?" },
            new ContactInfo() { ContactImage = "people_circle5.png", ContactName = "Watson", Message = "Have you watched any good movies or TV shows lately?" },
            new ContactInfo() { ContactImage = "people_circle23.png", ContactName = "Torrey", Message = "Hi, how are u?" },
            new ContactInfo() { ContactImage = "people_circle16.png", ContactName = "Georgia", Message = "How you doin'?" },
            new ContactInfo() { ContactImage = "people_circle26.png", ContactName = "Daniel", Message = "Call me at 6" },
            new ContactInfo() { ContactImage = "people_circle25.png", ContactName = "Katie", Message = "Shall we go?" },
            new ContactInfo() { ContactImage = "people_circle18.png", ContactName = "Peter", Message = "Join the meeting!" },
        };
    }
}

public class ContactInfo : INotifyPropertyChanged
{
    #region Fields

    private string? contactName;
    private string? contactImage;
    private string? message;

    #endregion

    #region Constructor

    public ContactInfo()
    {

    }

    #endregion

    #region Public Properties

    public string? ContactName
    {
        get { return this.contactName; }
        set
        {
            this.contactName = value;
            RaisePropertyChanged("ContactName");
        }
    }

    public string? Message
    {
        get { return this.message; }
        set
        {
            this.message = value;
            RaisePropertyChanged("Message");
        }
    }

    public string? ContactImage
    {
        get { return this.contactImage; }
        set
        {
            if (value != null)
            {
                this.contactImage = value;
                this.RaisePropertyChanged("ContactImage");
            }
        }
    }

    #endregion

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler? PropertyChanged;

    private void RaisePropertyChanged(String name)
    {
        if (PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(name));
    }

    #endregion
}
