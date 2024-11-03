using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.XPath;

namespace WorkdayCalculate;

public class WorkdayCalculator(TimeSpan workdayStart, TimeSpan workdayEnd)
{
    private readonly TimeSpan workdayStart = workdayStart;
    private readonly TimeSpan workdayEnd = workdayEnd;
    private readonly List<DateTime> Holidays = new List<DateTime>();
    private readonly List<DateTime> RecurringHolidays = new List<DateTime>();

    public void AddHoliday(DateTime date)
    {
        Holidays.Add(date.Date);
    }

    public void AddRecurringHoliday(DateTime date)
    {
        RecurringHolidays.Add(date.Date);
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

        // 2. Add or substract the partialDays
        var partialDays = (remainingDays - Math.Floor(remainingDays));
        if (partialDays > 0)
        {
            // Calculate the fractional timespan to add or substract
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
                    // Calculate the part of fraction remaining to be substracted from the previous working day
                    var remainingFractionForPreviousWorkday = (result - workdayStart).TimeOfDay;
                    // Take it to the next day's workday start and add the remainder
                    result = result.Date.AddDays(-1).Date + workdayEnd - remainingFractionForPreviousWorkday;
                }
            }
        }


        // 3. Add or substact remaining full working days
        while (remainingDays >= 1) 
        {
            result = isForward ? result.AddDays(1) : result.AddDays(-1);

            if (IsWorkingDay(result))
            {
                remainingDays--;
            }
        }

        // 4. Round to the nearest minute
        result = result.Date + TimeSpan.FromMinutes(Math.Round(result.TimeOfDay.TotalMinutes));

        return result;
    }

    private bool IsWorkingDay(DateTime date)
    {
        return date.DayOfWeek != DayOfWeek.Saturday &&
               date.DayOfWeek != DayOfWeek.Sunday &&
               !Holidays.Contains(date.Date) &&
               !RecurringHolidays.Any(h => h.Day == date.Day && h.Month == date.Month);
    }
}
