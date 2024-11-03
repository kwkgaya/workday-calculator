using System;

namespace WorkdayCalculate;

public class WorkdayCalculator : IWorkdayCalculator
{
    private protected TimeSpan workdayStart;
    private protected TimeSpan workdayEnd;
    private readonly HashSet<DateOnly> holidays = [];
    private readonly HashSet<DateOnly> recurringHolidays = [];

    public void SetWorkday(TimeSpan workdayStart, TimeSpan workdayEnd)
    {
        if (workdayEnd <= workdayStart)
        {
            throw new ArgumentException($"{nameof(workdayStart)} should be before {nameof(workdayEnd)}");
        }

        this.workdayStart = workdayStart;
        this.workdayEnd = workdayEnd;
    }

    public void AddHoliday(DateOnly date)
    {
        holidays.Add(date);
    }

    public void AddRecurringHoliday(DateOnly date)
    {
        var recurringHolyday = new DateOnly(1, date.Month, date.Day);
        recurringHolidays.Add(recurringHolyday);
    }

    public void RemoveHoliday(DateOnly date)
    {
        holidays.Remove(date);
    }

    public void RemoveRecurringHoliday(DateOnly date)
    {
        var recurringHolyday = new DateOnly(1, date.Month, date.Day);
        recurringHolidays.Remove(recurringHolyday);
    }


    public DateTime CalculateWorkingDays(DateTime start, double workingDays)
    {
        double remainingDays = Math.Abs(workingDays);
        var result = workingDays > 0
                        ? AddWorkingDays(start, remainingDays)
                        : SubtractWorkingDays(start, remainingDays);

        // 4. Round to the nearest minute
        result = result.Date + TimeSpan.FromMinutes(Math.Round(result.TimeOfDay.TotalMinutes));
        
        return result;
    }

    private DateTime AddWorkingDays(DateTime start, double remainingDays)
    {
        DateTime result = start;

        // 1. In case start does not fall within the workday,
        // Adjust the result to fall within the work day
        if (start.TimeOfDay < workdayStart)
        {
            result = start.Date + workdayStart;
        }
        else if (start.TimeOfDay > workdayEnd)
        {
            result = start.Date.AddDays(1).Date + workdayStart;
        }

        // 2. Add the partialDays
        var partialDays = (remainingDays - Math.Floor(remainingDays));
        if (partialDays > 0)
        {
            // Calculate the fractional timespan to add
            double fractionMinutes = ConvertToMinutes(partialDays);
            result += TimeSpan.FromMinutes(fractionMinutes);

            // Adjust result to the workday
            if (result.TimeOfDay > workdayEnd || result.TimeOfDay < workdayStart)
            {
                // Calculate the part of fraction remaining to be added to the next working day
                var remainingFractionForNextWorkday = (result - workdayEnd).TimeOfDay;
                // Take the result to the next workday's start and add the remainder
                result = result.Date.AddDays(1).Date + workdayStart + remainingFractionForNextWorkday;
            }
        }

        // 3. Add remaining full working days
        while (remainingDays >= 1)
        {
            result = result.AddDays(1);
            if (IsWorkingDay(new DateOnly(result.Year, result.Month, result.Day)))
            {
                remainingDays--;
            }
        }

        return result;
    }

    private DateTime SubtractWorkingDays(DateTime start, double remainingDays)
    {
        DateTime result = start;

        // 1. In case start date does not fall within the workday,
        // Adjust the result to fall within the work day
        if (start.TimeOfDay < workdayStart)
        {
            result = start.AddDays(-1).Date + workdayEnd;
        }
        else if (start.TimeOfDay > workdayEnd)
        {
            result = start.Date + workdayEnd;
        }

        // 2. Subtract the partialDays
        var partialDays = (remainingDays - Math.Floor(remainingDays));
        if (partialDays > 0)
        {
            // Calculate the fractional timespan to subtract
            double fractionMinutes = ConvertToMinutes(partialDays);
            result -= TimeSpan.FromMinutes(fractionMinutes);

            // Adjust to the workday
            if (result.TimeOfDay > workdayEnd || result.TimeOfDay < workdayStart)
            {
                // Calculate the part of fraction remaining to be subtracted from the previous working day
                var remainingFractionForPreviousWorkday = (result - workdayStart).TimeOfDay;
                // Take the result to the previous workdday's end and subtract the remainder
                result = result.Date.AddDays(-1).Date + workdayEnd - remainingFractionForPreviousWorkday;
            }
        }

        // 3. Subtract remaining full working days
        while (remainingDays >= 1)
        {
            result = result.AddDays(-1);
            if (IsWorkingDay(new DateOnly(result.Year, result.Month, result.Day)))
            {
                remainingDays--;
            }
        }

        return result;
    }

    // TODO: Write unit tests 
    public bool IsWorkingDay(DateOnly date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday &&
               date.DayOfWeek != DayOfWeek.Sunday &&
               !holidays.Contains(new DateOnly(date.Year, date.Month, date.Day)) &&
               !recurringHolidays.Any(h => h.Day == date.Day && h.Month == date.Month);
    }

    protected virtual double ConvertToMinutes(double partialWorkDays)
    {
        double workingDayTotalMinutes = (workdayEnd - workdayStart).TotalMinutes;
        // Convert using Math.Floor is not precise.
        // This shall be concidered a bug
        // Bug is fixed in the PreciseWorkdayCalculator class
        return Math.Floor(partialWorkDays * workingDayTotalMinutes);
    }
}
