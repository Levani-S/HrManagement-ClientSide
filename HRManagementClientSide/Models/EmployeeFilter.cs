using HRManagementClientSide.Enums;

namespace HRManagementClientSide.Models
{
    public class EmployeeFilter
    {
        public FilteringEmployee PropertyType { get; set; }
        public string PropertyValue { get; set; }
    }
}
