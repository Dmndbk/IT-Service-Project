using ITService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITService.UI.Wrappers
{
    public class ObjectiveWrapper : ModelWrapper<Objective>
    {
        public ObjectiveWrapper(Objective model) : base(model)
        {
        }
        public int Id => Model.Id;
        public string? EmployeeName => Model.ResponsibleEmployee?.FirstName + " " + Model.ResponsibleEmployee?.LastName;
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
        public DateTime StartDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public DateTime EndDate
        {
            get => GetValue<DateTime>();
            set => SetValue(value);
        }

        public int? ResponsibleEmployeeId
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
