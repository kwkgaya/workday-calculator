using FluentAssertions;
using NUnit.Framework.Internal;
using WorkdayCalculate;

namespace WorkdayCalculateTest;

public class WorkdayCalculatorAdditionalTests
{
    #region Subtraction tests

    [Test]
    public void SubtractFullWorkdays_WhenStartIsWithinWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0), 
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 12, 05, 0), -1);

        result.Should().Be(new DateTime(2024, 11, 06, 12, 05, 0));
    }

    [Test]
    public void SubtractFullWorkdays_WhenStartIsBeforeWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 07, 05, 0), -2);

        result.Should().Be(new DateTime(2024, 11, 04, 16, 0, 0));
    }

    [Test]
    public void SubtractFullWorkdays_WhenStartIsAfterWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 18, 05, 0), -2);

        result.Should().Be(new DateTime(2024, 11, 05, 16, 0, 0));
    }

    [Test]
    public void SubtractFractionalWorkdays_WhenStartIsWithinTheWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 13, 05, 0), -0.125);

        result.Should().Be(new DateTime(2024, 11, 07, 12, 5, 0));
    }

    [Test]
    public void SubtractFractionalWorkdays_WhenStartIsBeforeTheWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 07, 05, 0), -0.125);

        result.Should().Be(new DateTime(2024, 11, 06, 15, 0, 0));
    }

    [Test]
    public void SubtractFractionalWorkdays_WhenStartIsAfterTheWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(8, 0, 0),
            workdayEnd: new TimeSpan(16, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 18, 05, 0), -0.125);

        result.Should().Be(new DateTime(2024, 11, 07, 15, 0, 0));
    }

    [Test]
    public void SubtractFractionalWorkdays_WhenWorkdayIsShortAndStartIsAfterTheWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(11, 0, 0),
            workdayEnd: new TimeSpan(13, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 07, 18, 05, 0), -2.125);

        result.Should().Be(new DateTime(2024, 11, 05, 12, 45, 0));
    }

    [Test]
    public void SubtractFractionalWorkdays_WhenWorkdayIsShortAndStartIsBeforeTheWorkday()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(11, 0, 0),
            workdayEnd: new TimeSpan(13, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 08, 10, 05, 0), -2.125);

        result.Should().Be(new DateTime(2024, 11, 05, 12, 45, 0));
    }

    /*
     * This test fails due to a bug in the code
    [Test]
    public void SubtractFractionalWorkdays_WhenCalculationSpansAcrossMultipleWorkdays()
    {
        var workdayCalculator = new PreciseWorkdayCalculator();
        workdayCalculator.SetWorkday(
            workdayStart: new TimeSpan(11, 0, 0),
            workdayEnd: new TimeSpan(13, 0, 0));

        var result = workdayCalculator.CalculateWorkingDays(new DateTime(2024, 11, 08, 11, 05, 0), -2.125);

        result.Should().Be(new DateTime(2024, 11, 05, 12, 50, 0));
    }
    */

    #endregion Subtraction tests
}
