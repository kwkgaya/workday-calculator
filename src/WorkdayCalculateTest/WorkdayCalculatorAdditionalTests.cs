using FluentAssertions;
using NUnit.Framework.Internal;
using WorkdayCalculate;


namespace WorkdayCalculateTest
{
    public class WorkdayCalculatorAdditionalTests
    {
        #region Substraction tests
        [Test]
        public void AddWorkingDays_SubstractPartialWorkdays_ShouldCalculate()
        {
            var workdayCalculator = new WorkdayCalculator(
                workdayStart: new TimeSpan(8, 0, 0),
                workdayEnd: new TimeSpan(16, 0, 0)
            );
            workdayCalculator.AddRecurringHoliday(new DateTime(2024, 5, 17));  // Recurring on 17 May each year
            workdayCalculator.AddHoliday(new DateTime(2004, 5, 27));           // Single holiday on 27 May 2004

            var result = workdayCalculator.AddWorkingDays(new DateTime(2004, 5, 24, 18, 05, 0), -0.125);

            result.Should().Be(new DateTime(2004, 5, 24, 15, 0, 0));
        }
        #endregion Substraction tests
    }
}
