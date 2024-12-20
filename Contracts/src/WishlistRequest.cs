namespace Contracts;

public record WishlistRequest(int EmployeeId, string EmployeeName, string Role, int[] DesiredEmployees);

public record HackathonStarted(string Message);

