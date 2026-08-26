namespace VyacheslavsMegaWebApp.Data.Repositories.Base
{
    public abstract class RepositoryBase
    {
        private readonly AppDbContext _db;

        public RepositoryBase()
        {
            _db = new AppDbContext();
        }   

        protected AppDbContext DB => _db;
    }
}
