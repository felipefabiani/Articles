namespace Articles.Database.Entities;

public interface IEntity
{
    int Id { get; set; }
}

public interface ITrackableEntity
{
    DateTimeOffset CreatedOn { get; set; }
    DateTimeOffset LastModifyedOn { get; set; }
}