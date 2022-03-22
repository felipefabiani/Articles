namespace Author.Login;

public static class Data
{
    internal static Task<Author> GetAuthor(string userName)
    {
        return Task.FromResult(new Author
        {
            authorID = "1",
            fullName = "Felipe Test",
            passwordHash = "123"
        });
    }

    internal record struct Author(
        string authorID,
        string fullName,
        string passwordHash)
    { }
}