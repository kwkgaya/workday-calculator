
namespace WorkdayCalculate;

public interface IWorkdayCalculator : IWorkdayCalculatorConfigurer
{
    /// <summary>
    /// Adds (or subtracts) <paramref name="workingDays" /> to the given <paramref name="start"/> date according to the configurations provided
    /// </summary>
    /// <param name="start">The start date</param>
    /// <param name="workingDays">Working days to add</param>
    /// <returns>The resulting working day and time</returns>
    DateTime CalculateWorkingDays(DateTime start, double workingDays);
}