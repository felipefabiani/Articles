using Articles.Models.Feature.Articles.SaveArticle;
using Microsoft.Extensions.Logging;

namespace Articles.Database.Entities;

public class ArticleEntity :
    Entity<ArticleEntity>,
    ITrackableEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;
    public string? RejectionReason { get; set; }
    public List<CommentEntity>? Comments { get; set; }
    public int AuthorId { get; set; }
    public UserEntity Author { get; set; } = null!;
    public DateTimeOffset CreatedOn { get; set; } = default;
    public DateTimeOffset LastModifyedOn { get; set; }

    public ArticleEntity() : base()
    { }

    public ArticleEntity(IServiceProvider service)
        : base(service)
    { }

    public async Task<ArticleEntity?> Save(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Saving article, id [{id}], title [{title}].", Id, Title);

        if (Id == 0)
        {
            _logger.LogDebug("Add article.");
            await DbSetWriteOnly.AddAsync(this);
        }
        else if (DbSetReadOnly.Any(art => art.Id == Id && art.AuthorId == AuthorId))
        {
            _logger.LogDebug("Update article.");

            var article = await DbSetWriteOnly.FindAsync(new object[] { Id }, cancellationToken);
            if (article is null)
            {
                _logger.LogDebug("Article with id {id} not found.", Id);
                return null;
            }

            DbSetWriteOnly.Update(this);
        }
        else
        {
            _logger.LogDebug("Author is not the article owner.");
            return null;
        }

        await SaveAsync(cancellationToken);

        _logger.LogDebug("Article saved, id [{id}], title [{title}].", Id, Title);
        return this;
    }

    public async Task<List<ArticleEntity>> GetPendingApprovals(PendingApprovalArticleRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("GetPendingApprovals article, request {request}.", request);

        var db = DbSetReadOnly
            .Include(x => x.Author)
            .Where(x => x.IsApproved == false)
            .Where(x => x.RejectionReason == null || x.RejectionReason == string.Empty)
            .AsQueryable();

        if (request.Ids?.Length > 0)
        {
            db = db.Where(x => request.Ids.Contains(x.AuthorId));
        }

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            db = db.Where(x =>
                x.CreatedOn >= request.StartDate.Value.Date &&
                x.CreatedOn < request.EndDate.Value.Date.AddDays(1));
        }

        var list = await db
            .ToListAsync(cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _logger.LogDebug("GetPendingApprovals return {list}.", list);
        return list ?? new List<ArticleEntity>();
    }
}
