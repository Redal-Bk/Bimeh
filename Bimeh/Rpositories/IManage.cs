using Bimeh.Domain.DTOs;
using Bimeh.Domain.Entities;

namespace Bimeh.Rpositories
{
    public interface IManage
    {
        Task<bool> Login(UserDTO user);
        Task<bool> AcceptRequest(RequestDTO requestDTO, int userId);
        Task<List<Request>> GetAllRequest(int id);
        Task<Request> GetSelectedRequest(int trackingNumber);
    }
}
