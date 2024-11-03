using FluentAssertions;
using NUnit.Framework.Internal;
using WorkdayCalculate;

namespace WorkdayCalculatorTest;

public class WorkdayCalculatorTest
{
    private WorkdayCalculator workdayCalculator;

    [SetUp]
    public void Setup()
    {
        workdayCalculator = new WorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0)
        );
        workdayCalculator.AddRecurringHoliday(new DateOnly(2024, 5, 17));  // Recurring on 17 May each year
        workdayCalculator.AddHoliday(new DateOnly(2004, 5, 27));           // Single holiday on 27 May 2004
    }

    [Test]
    public void SubtractWorkingDays_WhenStartIsAfterTheWorkday()
    {
        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2004, 5, 24, 18, 05, 0), -5.5);

        result.Should().Be(new DateTime(2004, 5, 14, 12, 0, 0));
    }

    [Test]
    public void AddWorkingDays_WhenStartIsAfterTheWorkday()
    {
        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2004, 5, 24, 19, 03, 0), 44.723656);

        result.Should().Be(new DateTime(2004, 7, 27, 13, 47, 0));
    }

    [Test]
    public void SubtractWorkingDays_WhenStartIsAfterTheWorkday_2()
    {
        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2004, 5, 24, 18, 03, 0), -6.7470217);

        result.Should().Be(new DateTime(2004, 5, 13, 10, 01, 0));

        // Result given in the example is incorrect.
        // (1 - 0.7470217) * 8 * 60 = 121.43 = 2 hour 1 minutes 26 seconds into the workday => should be 10:01

        // result.Should().Be(new DateTime(2004, 5, 13, 10, 02, 0)); // This is incorrect
    }

    [Test]
    public void AddWorkingDays_WhenStartIsWithinTheWorkday()
    {
        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2004, 5, 24, 08, 03, 0), 12.782709);

        result.Should().Be(new DateTime(2004, 6, 10, 14, 19, 0));

        // Result given in the example is incorrect.
        // 0.782709 * 8 * 60 + 3 = 378.7 = 6 hours 18 minutes 42 seconds into the workday => should be 14:19
    }

    [Test]
    public void AddWorkingDays_WhenStartIsBeforeTheWorkday()
    {
        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2004, 5, 24, 07, 03, 0), 8.276628);

        result.Should().Be(new DateTime(2004, 6, 4, 10, 13, 0));

        // Result given in the example is incorrect.
        // 0.276628 * 8 * 60 = 132.78 = 2 hours 12 minutes 47 seconds into the workday => should be 10:13
    }
}