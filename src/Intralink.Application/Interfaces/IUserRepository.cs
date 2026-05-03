using Application.Users;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(UserAccount user);
    Task<UserAccount?> GetByEmailAsync(string email);
    Task<List<UserAccount>> GetAllAsync();
    Task<UserAccount?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(UserAccount user);
    Task<bool> DeleteAsync(int id);
}
