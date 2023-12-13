using System.ComponentModel.DataAnnotations;
using Student_Planner.Models;
using Xunit;

public class UserTests
{
    [Fact]
    public void User_Validation_Successful()
    {
        // Arrange
        var user = new User
        {
            UserName = "JohnDoe",
            Password = "SecurePassword",
            Email = "john.doe@example.com"
        };

        // Act
        var validationContext = new ValidationContext(user, null, null);
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(user, validationContext, result, true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(result);
    }

    [Fact]
    public void User_Validation_Fails_WhenUserNameMissing()
    {
        // Arrange
        var user = new User
        {
            // UserName is intentionally left null, which should trigger validation failure
            Password = "SecurePassword",
            Email = "john.doe@example.com"
        };

        // Act
        var validationContext = new ValidationContext(user, null, null);
        var result = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(user, validationContext, result, true);

        // Assert
        Assert.False(isValid);
        Assert.NotEmpty(result);
        Assert.Contains(result, r => r.MemberNames.Contains(nameof(User.UserName)));
    }
}