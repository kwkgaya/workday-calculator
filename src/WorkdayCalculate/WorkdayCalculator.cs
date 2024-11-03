using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        return DateTime.MinValue;
    }
}
