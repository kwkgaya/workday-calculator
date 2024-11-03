namespace WorkdayCalculate;

// TODO:
// Do not implement the WorkdayCalculator
// Instead, fix the bug directly in the WorkdayCalculator class
// Fixed the code in a subclass just to demonstrate OOP concepts and to keep the original tests of the WorkdayCalculator
public class PreciseWorkdayCalculator : WorkdayCalculator
{
    protected override double ConvertToMinutes(double partialWorkDays)
    {
        double workingDayTotalMinutes = (workdayEnd - workdayStart).TotalMinutes;
        return TimeSpan.FromMinutes(partialWorkDays * workingDayTotalMinutes).TotalMinutes;
    }
}
