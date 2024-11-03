
namespace WorkdayCalculate;

public interface IWorkdayCalculator : IWorkdayCalculatorConfigurer
{
    /// <summary>
    /// Adds (or subtracts) <paramref name="workingDays" /> to the given <paramref name="start"/> date according to the configurations provided
    /// </summary>
    /// <param name="start"></param>
    /// <param name="workingDays"></param>
    /// <returns>The resulting working day and time</returns>
    DateTime AddWorkingDays(DateTime start, double workingDays);
}