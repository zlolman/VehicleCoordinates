using System.Collections.Generic;
using VehicleCoordinates.Models;
using System.Threading.Tasks;

namespace VehicleCoordinates.Interfaces
{
    public interface IRepository
    {
        Task<List<Coordinate>> GetAll();
        Task<Coordinate> Get(int id);
        void Create(Coordinate coordinate);
        void Update(Coordinate coordinate);
        void Delete(int id);
        //void Close();
    }
}
