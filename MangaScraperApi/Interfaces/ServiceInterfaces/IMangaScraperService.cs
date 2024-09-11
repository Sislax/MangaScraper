namespace MangaScraperApi.Interfaces.ServiceInterfaces
{
    public interface IMangaScraperService
    {
        public Task Operate(int nPages);
        public void Update();
    }
}
