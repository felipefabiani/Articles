using Articles.Test.Helper.Bases;
using Articles.Test.Helper.Extensions;

namespace Articles.Test.Helper.TheoryData;
public record SaveArticleTheoryModel(
    string? Title,
    string? Content,
    int AuthorId,
    string TitleMessage = "",
    string ContentMessage = "",
    string AuthorMessage = "")
{

}
public class SaveArticleInvalidParamTheoryData : TheoryData<SaveArticleTheoryModel>
{
    public SaveArticleInvalidParamTheoryData()
    {
        Add(new SaveArticleTheoryModel(null, null, 0, "Title is required!", "Content is required!", "AuthorID is required!"));
        Add(new SaveArticleTheoryModel("", "", -1, "Title is required!", "Content is required!", "AuthorID is required!"));
        Add(new SaveArticleTheoryModel(1.RandomString(), 1.RandomString(), -1, "Title is too short!", "Content is too short!", "AuthorID is required!"));
        Add(new SaveArticleTheoryModel(9.RandomString(), 9.RandomString(), -1, "Title is too short!", "Content is too short!", "AuthorID is required!"));
    }
}

public class SaveArticleValidParamTheoryData : TheoryData<SaveArticleTheoryModel>
{
    public SaveArticleValidParamTheoryData()
    {
        Add(new SaveArticleTheoryModel(10.RandomString(), 10.RandomString(), 1));
        Add(new SaveArticleTheoryModel(100.RandomString(), 100.RandomString(), 999));
    }
}