namespace Shuttle.Packager.WebApi.Repositories
{
    public class InMemoryProjectRepository : IProjectRepository
    {
        private readonly Dictionary<Guid, Project> _projects = new Dictionary<Guid, Project>();

        public async Task<Project> GetAsync(Guid id)
        {
            if (!_projects.TryGetValue(id, out var project))
            {
                throw new ApplicationException($"Could not find project with id '{id}'.");
            }

            return await Task.FromResult(project);
        }

        public async Task SaveAsync(IEnumerable<Project> projects)
        {
            _projects.Clear();
            
            foreach (var project in projects)
            {
                _projects.Add(project.Id, project);
            }
            
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Project>> GetAsync()
        {
            return await Task.FromResult<IEnumerable<Project>>(_projects.Values);
        }
    }
}