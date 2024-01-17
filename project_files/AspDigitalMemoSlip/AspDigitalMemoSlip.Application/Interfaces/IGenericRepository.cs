namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IGenericRepository<T, TId>
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAll(int PageNr, int PageSize);
        Task<T> GetById(TId Id);
        Task<T> Create(T Item);
        T Update(T Modified);
        void Delete(T Item);
    }
}
