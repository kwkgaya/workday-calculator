
namespace WorkdayCalculate;

public interface IWorkdayCalculatorConfigurer
{
    /// <summary>
    /// Sets the workday.
    /// </summary>
    /// <param name="workdayStart">Start time of the workday</param>
    /// <param name="workdayEnd">End time of the workday</param>
    /// <exception cref="ArgumentException" if <paramref name="workdayStart"/> is not before <paramref name="workdayEnd/>
    void SetWorkday(TimeSpan workdayStart, TimeSpan workdayEnd);

    /// <summary>
    /// Adds a holyday
    /// </summary>
    /// <param name="date">The holyday</param>
    void AddHoliday(DateOnly date);
    
    /// <summary>
    /// Add a recurring holyday
    /// </summary>
    /// <param name="date"></param>
    void AddRecurringHoliday(DateOnly date);
    
    /// <summary>
    /// Remove a holyday if it is already added, otherwise throws <<see cref="InvalidOperationException"/>
    /// </summary>
    /// <param name="date">The holyday to remove</param>
    void RemoveHoliday(DateOnly date);

    /// <summary>
    /// Remove a recurring holyday if it is already added, otherwise throws <<see cref="InvalidOperationException"/>
    /// </summary>
    /// <param name="date">The holyday to remove</param>
    void RemoveRecurringHoliday(DateOnly date);

    /// <summary>
    /// Returns true if the given date is a working day
    /// </summary>
    /// <param name="date">The date</param>
    bool IsWorkingDay(DateOnly date);
}
