using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace BlazorWasm.Services
{
    public interface IEmployeeService : IScopedService
    {
        string GetEmployeeName();
    }
}
