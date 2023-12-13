using Xunit;
using System.IO;
using Student_Planner.Services.Implementations;

public class ReadTxtFileTests
{
    [Fact]
    public void ReadTextFile_ShouldReturnFileContent_WhenFileExists()
    {
        // Arrange
        var filePath = Path.GetTempFileName();
        var fileContent = "Hello, World!";
        File.WriteAllText(filePath, fileContent);

        // Act
        var result = ReadTxtFile.ReadTextFile(filePath);

        // Assert
        Assert.Equal(fileContent, result);

        // Clean up
        File.Delete(filePath);
    }

    [Fact]
    public void ReadTextFile_ShouldReturnErrorMessage_WhenFileDoesNotExist()
    {
        // Arrange
        var filePath = "nonexistent.txt";

        // Act
        var result = ReadTxtFile.ReadTextFile(filePath);

        // Assert
        Assert.Equal("The file does not exist.", result);
    }
}