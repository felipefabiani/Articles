namespace Articles.Client;

public class BadRequestResponse
{
    public int statusCode { get; set; }
    public string message { get; set; }
    public Errors errors { get; set; }
}

public class Errors
{
    public string[] GeneralErrors { get; set; }
}