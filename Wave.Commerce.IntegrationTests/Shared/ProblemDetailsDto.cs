namespace Wave.Commerce.IntegrationTests.Shared;

public class ProblemDetailsDto
{
    public string Title { get; set; } = "";
    public int Status { get; set; }
    public string Detail { get; set; } = "";
    public List<ProblemDetailsErrorsDto> Errors { get; set; } = new();
}

public class ProblemDetailsErrorsDto
{
    public string Message { get; set; } = "";
}
