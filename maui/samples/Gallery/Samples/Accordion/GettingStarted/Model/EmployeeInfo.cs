namespace Syncfusion.Maui.ControlGallery.Accordion
{
	public class EmployeeInfo
    {
        #region Constructor

        public EmployeeInfo(string name, string image, string position, string organizationunit, string dateofbirth, string location, string phone, bool isexpanded, string description)
        {
            Name = name;
            Image = image;
            Position = position;
            OrganizationUnit = organizationunit;
            DateOfBirth = dateofbirth;
            Location = location;
            Phone = phone;
            IsExpanded = isexpanded;
            Description = description;
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        public string Image { get; set; }

        public string Position { get; set; }

        public string OrganizationUnit { get; set; }

        public string DateOfBirth { get; set; }

        public string Location { get; set; }

        public string Phone { get; set; }

        public bool IsExpanded { get; set; }

        public string Description { get; set; }

        #endregion
    }
}
