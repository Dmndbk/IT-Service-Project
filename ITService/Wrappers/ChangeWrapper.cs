using ITService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITService.UI.Wrappers
{
    public class ChangeWrapper : ModelWrapper<Change>
    {
        public ChangeWrapper(Change model) : base(model)
        {
        }
        public int Id => Model.Id;
        
        public string? EmployeeName => Model.RelatedEmployee?.FirstName + " " + Model.RelatedEmployee?.LastName;
        public string? ServiceName => Model.RelatedService?.Name;
        public string? Status => Model.CurrentStatus?.Name;
        public string Theme
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string Description
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        public string? SolutionDescription
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public string? ImplementationPlan
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public string? RollbackPlan
        {
            get => GetValue<string?>();
            set => SetValue(value);
        }
        public DateTime EndDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int? RelatedEmployeeId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }

        public int? RelatedServiceId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
        public int? CurrentStatusId
        {
            get => GetValue<int?>();
            set => SetValue(value);
        }
    }
}
