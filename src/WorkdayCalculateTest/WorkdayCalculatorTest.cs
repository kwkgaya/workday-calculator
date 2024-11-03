using FluentAssertions;
using NUnit.Framework.Internal;
using WorkdayCalculate;

namespace WorkdayCalculatorTest
{
    public class Tests
    {
        private WorkdayCalculator workdayCalculator;

        [SetUp]
        public void Setup()
        {
            workdayCalculator = new WorkdayCalculator(
                workdayStart: new TimeSpan(8, 0, 0),
                workdayEnd: new TimeSpan(16, 0, 0)
            );
            workdayCalculator.AddRecurringHoliday(new DateTime(2024, 5, 17)); // Recurring on 17 May each year
            workdayCalculator.AddHoliday(new DateTime(2004, 5, 27));           // Single holiday on 27 May 2004
        }

        [Test]
        public void AddWorkingDays_SubstractWorkdays_ShouldCalculate()
        {
            var result = workdayCalculator.AddWorkingDays(new DateTime(2004, 5, 24, 18, 05, 0), -5.5);

            result.Should().Be(new DateTime(2004, 5, 14, 12, 0, 0));
        }
    }
}