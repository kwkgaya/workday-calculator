using System;

namespace WorkdayCalculate;

public class WorkdayCalculator
    : IWorkdayCalculatorConfigurer, IWorkdayCalculator
{
    private TimeSpan workdayStart;
    private TimeSpan workdayEnd;
    private readonly HashSet<DateOnly> Holidays = new();
    private readonly HashSet<DateOnly> RecurringHolidays = new();

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
        Holidays.Add(date);
    }

    public void AddRecurringHoliday(DateOnly date)
    {
        var recurringHolyday = new DateOnly(1, date.Month, date.Day);
        RecurringHolidays.Add(recurringHolyday);
    }

    public void RemoveHoliday(DateOnly date)
    {
        Holidays.Remove(date);
    }

    public void RemoveRecurringHoliday(DateOnly date)
    {
        var recurringHolyday = new DateOnly(1, date.Month, date.Day);
        RecurringHolidays.Remove(recurringHolyday);
    }

    public DateTime AddWorkingDays(DateTime start, double workingDays)
    {
        double remainingDays = Math.Abs(workingDays);
        bool isForward = workingDays > 0;

        DateTime result = start;

        // 1. In case start date does not fall within the workday,
        // Adjust the result to fall within the work day
        if (isForward)
        {
            if (start.TimeOfDay < workdayStart)
            {
                result = start.Date + workdayStart;
            }
            else if (start.TimeOfDay > workdayEnd)
            {
                result = start.Date.AddDays(1).Date + workdayStart;
            }
        }
        else
        {
            if (start.TimeOfDay < workdayStart)
            {
                result = start.AddDays(-1).Date + workdayEnd;
            }
            else if (start.TimeOfDay > workdayEnd)
            {
                result = start.Date + workdayEnd;
            }
        }

        // 2. Add or subtract the partialDays
        var partialDays = (remainingDays - Math.Floor(remainingDays));
        if (partialDays > 0)
        {
            // Calculate the fractional timespan to add or subtract
            TimeSpan workingDaySpan = workdayEnd - workdayStart;
            TimeSpan fraction = TimeSpan.FromMinutes(partialDays * workingDaySpan.TotalMinutes);

            if (isForward)
            {
                result = result + fraction;

                // Adjust to the workday
                if (result.TimeOfDay > workdayEnd || result.TimeOfDay < workdayStart)
                {
                    // Calculate the part of fraction remaining to be added to the next working day
                    var remainingFractionForNextWorkday = (result - workdayEnd).TimeOfDay;
                    // Take it to the next day's workday start and add the remainder
                    result = result.Date.AddDays(1).Date + workdayStart + remainingFractionForNextWorkday;
                }
            }
            else
            {
                result = result - fraction;

                // Adjust to the workday
                if (result.TimeOfDay > workdayEnd || result.TimeOfDay < workdayStart)
                {
                    // Calculate the part of fraction remaining to be subtracted from the previous working day
                    var remainingFractionForPreviousWorkday = (result - workdayStart).TimeOfDay;
                    // Take it to the next day's workday start and add the remainder
                    result = result.Date.AddDays(-1).Date + workdayEnd - remainingFractionForPreviousWorkday;
                }
            }
        }


        // 3. Add or subtract remaining full working days
        while (remainingDays >= 1)
        {
            result = isForward ? result.AddDays(1) : result.AddDays(-1);

            if (IsWorkingDay(new DateOnly(result.Year, result.Month, result.Day)))
            {
                remainingDays--;
            }
        }

        // 4. Round to the nearest minute
        result = result.Date + TimeSpan.FromMinutes(Math.Round(result.TimeOfDay.TotalMinutes));

        return result;
    }

    public bool IsWorkingDay(DateOnly date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday &&
               date.DayOfWeek != DayOfWeek.Sunday &&
               !Holidays.Contains(new DateOnly(date.Year, date.Month, date.Day)) &&
               !RecurringHolidays.Any(h => h.Day == date.Day && h.Month == date.Month);
    }
}
