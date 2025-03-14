using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlGallery.Accordion
{
	public class EmployeeDetails
    {
        #region Fields

        private ObservableCollection<EmployeeInfo>? _employee;

        string[] Description = new string[]
        {
        "Robin Rane, Chairman of ABC Inc., leads with dedication and vision.Under his guidance, the company thrives and continues to make a significant impact in the industry.",
        "Paul Vent, General Manager of XYZ Corp., oversees daily operations, ensuring the company's success and growth through strategic planning and effective management practices.",
        "Clara Venus, Asst. Manager at ABC Inc., efficiently handles multiple tasks. With her strong skill set and dedication, she contributes significantly to the company's growth and success.",
        "Maria Even, a highly experienced professional, holds the position of Executive Manager at XYZ Corp. She oversees crucial operations, enforcing company policies and practices.",
        "Mark Zuen, Senior Executive at ABC Inc., skillfully manages business operations. He is adept at leadership and strategic thinking.",
        "Eric John, Technical Manager at ABC Inc., expertly leads his team to develop innovative solutions, creating value for the company",
        "Chris Marker serves as the Senior Accountant at XYZ Corp. With extensive experience, he skillfully manages the company's financial matters, ensuring accuracy and compliance.",
        "Seria Stein, an Account Executive at ABC Inc., adeptly manages client portfolios, ensuring their satisfaction. She is a great communicator, skilled in building relationships.",
        "Angelina Justin, HR Manager at XYZ Corp., expertly handles workplace dynamics with her exceptional communication and problem-solving skills, fostering a positive work environment"
        };

        #endregion

        #region Constructor

        public EmployeeDetails()
        {
            Employees = new ObservableCollection<EmployeeInfo>();
            Employees.Add(new EmployeeInfo("Robin Rane", "emp_01.png", ":  Chairman", ":  ABC Inc.", ":  09/17/1973", ":  Boston", "(617) 555-1234", false, Description[0]));
            Employees.Add(new EmployeeInfo("Paul Vent", "emp_02.png", ":  General Manager", ":  XYZ Corp.", ":  05/27/1985", ":  New York", "(212) 555-1234", true, Description[1]));
            Employees.Add(new EmployeeInfo("Clara Venus", "emp_03.png", ":  Assistant Manager", ":  ABC Inc.", ":  07/22/1988", ":  California", "(415) 123-4567", false, Description[2]));
            Employees.Add(new EmployeeInfo("Maria Even", "emp_04.png", ":  Executive Manager", ":  XYZ Corp.", ":  04/16/1970", ":  New York", "(516) 345-6789", false, Description[3]));
            Employees.Add(new EmployeeInfo("Mark Zuen", "emp_05.png", ":  Senior Executive", ":  ABC Inc.", ":  09/11/1983", ":  Boston", "(617) 123-4567", false, Description[4]));
            Employees.Add(new EmployeeInfo("Eric John", "emp_06.png", ":  Technical Manager", ":  ABC Inc.", ":  12/09/1985", ":  New Jersey", "(201) 555-1234", false, Description[5]));
            Employees.Add(new EmployeeInfo("Chris Marker", "emp_07.png", ":  Senior Accountant", ":  XYZ Corp.", ":  03/14/1986", ":  California", "(714) 555-5678", false, Description[6]));
            Employees.Add(new EmployeeInfo("Seria Stein", "emp_08.png", ":  Account Executive", ":  XYZ Corp.", ":  02/07/1985", ":  New York", "(646) 987-6543", false, Description[7]));
            Employees.Add(new EmployeeInfo("Angelina Justin", "emp_09.png", ":  HR Manager", ":  XYZ Corp.", ":  07/11/1972", ":  Boston", "(617) 987-6543", false, Description[8]));
        }

        #endregion

        #region Properties

        public ObservableCollection<EmployeeInfo>? Employees
        {
            get { return _employee; }
            set { _employee = value; }
        }

        #endregion

        
    }
}
