using BlazorWasm.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWasm.Pages
{
    public partial class Index
    {
        [Inject]
        private IEmployeeService EmployeeService { get; set; }

        protected override void OnInitialized()
        {
            string employeeName = EmployeeService.GetEmployeeName();
        }
    }
}
