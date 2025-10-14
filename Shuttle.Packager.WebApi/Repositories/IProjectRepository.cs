namespace Shuttle.Packager.WebApi.Repositories;

public interface IProjectRepository
{
    Task<IEnumerable<Project>> GetAsync();
    Task<Project> GetAsync(Guid id);
    Task SaveAsync(IEnumerable<Project> projects);
}