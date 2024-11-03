
using WorkdayCalculate;

Console.WriteLine("Hello, workday calculator!");

WorkdayCalculator calculator = new WorkdayCalculator();

// Use a reference of IWorkdayCalculatorConfigurer to configure
var configurer = calculator as IWorkdayCalculatorConfigurer;
configurer.SetWorkday(
    workdayStart: new TimeSpan(8, 0, 0),
    workdayEnd: new TimeSpan(16, 0, 0)
);

// Setting up holidays
configurer.AddRecurringHoliday(new DateOnly(2000, 5, 17));  // Recurring on 17 May each year
configurer.AddHoliday(new DateOnly(2004, 5, 27));           // Single holiday on 27 May 2004

// Test cases
DateTime startDate1 = new DateTime(2004, 5, 24, 19, 3, 0);
double workdaysToAdd1 = 44.723656;
Console.WriteLine("Expected Result: 27-07-2004 13:47");
Console.WriteLine($"Calculated Result: {calculator.CalculateWorkingDays(startDate1, workdaysToAdd1).ToString("dd-MM-yyyy HH:mm")}");

DateTime startDate2 = new DateTime(2004, 5, 24, 18, 3, 0);
double workdaysToAdd2 = -6.7470217;
Console.WriteLine("Expected Result: 13-05-2004 10:02");
Console.WriteLine($"Calculated Result: {calculator.CalculateWorkingDays(startDate2, workdaysToAdd2).ToString("dd-MM-yyyy HH:mm")}");

DateTime startDate3 = new DateTime(2004, 5, 24, 8, 3, 0);
double workdaysToAdd3 = 12.782709;
Console.WriteLine("Expected Result: 10-06-2004 14:18");
Console.WriteLine($"Calculated Result: {calculator.CalculateWorkingDays(startDate3, workdaysToAdd3).ToString("dd-MM-yyyy HH:mm")}");

DateTime startDate4 = new DateTime(2004, 5, 24, 7, 3, 0);
double workdaysToAdd4 = 8.276628;
Console.WriteLine("Expected Result: 04-06-2004 10:12");
Console.WriteLine($"Calculated Result: {calculator.CalculateWorkingDays(startDate4, workdaysToAdd4).ToString("dd-MM-yyyy HH:mm")}");