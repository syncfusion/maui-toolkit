using Syncfusion.Maui.Toolkit.Chips;

namespace Syncfusion.Maui.ControlsGallery.Chips;
public class EntryBehavior : Behavior<Entry>
{
    #region Fields

    /// <summary>
    /// The input chip group.
    /// </summary>
    SfChipGroup? inputChipGroup;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the input chip group.
    /// </summary>
    /// <value>The input chip group.</value>
    public SfChipGroup? InputChipGroup
    {
        get
        {
            return inputChipGroup;
        }
        set
        {
            inputChipGroup = value;
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SampleBrowser.Chips.EntryBehavior"/> class.
    /// </summary>
    public EntryBehavior()
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// On the attached property.
    /// </summary>
    /// <param name="entry">Bindable.</param>
    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);

        entry.Completed += (object? sender, EventArgs e) =>
        {
            if (inputChipGroup != null && !string.IsNullOrEmpty(entry.Text))
            {
                ChipViewModel viewModel = (entry.BindingContext as ChipViewModel)!;
                if (viewModel != null && viewModel.SelectedItem != null)
                {
                    if (viewModel.SelectedItem.ToString() == "Television")
                    {
                        viewModel.televisionItems.Add(entry.Text);
                    }
                    else if (viewModel.SelectedItem.ToString() == "Washer")
                    {
                        viewModel.washerItems.Add(entry.Text);
                    }
                    else if (viewModel.SelectedItem.ToString() == "Air Conditioner")
                    {
                        viewModel.airConditionerItems.Add(entry.Text);
                    }
                }
                entry.Text = string.Empty;
                entry.Placeholder = "Enter brand name";

            }
            if (string.IsNullOrEmpty(entry.Text))
            {
                entry.Unfocus();
            }
        };
    }

    #endregion

}
