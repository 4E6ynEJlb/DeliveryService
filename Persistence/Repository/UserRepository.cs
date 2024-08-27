using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities.SQLEntities;
using Persistence.Exceptions;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class UserRepository : IUserStore
    {
        private readonly SQLContext _context;
        private readonly IUserCacheService _cacheService;
        public UserRepository(SQLContext context, IUserCacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<User> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            return user;
        }

        public async Task<User> GetUserByAuthAsync(AuthModel authModel, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == authModel.Login && u.Password == authModel.Password, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));            
            await _cacheService.SaveAsync(user, cancellationToken);
            return user;
        }

        public async Task<List<string>> GetBirthdayPeopleTelegramAsync(CancellationToken cancellationToken)
        {
            return (await _context.Users.AsNoTracking().Where(u => u.TelegramId != null && u.BirthDate.HasValue && u.BirthDate.Value == DateOnly.FromDateTime(DateTime.Now)).Select(u => u.TelegramId).ToListAsync(cancellationToken))!;
        }

        public async Task AddUserAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.SaveAsync(user, cancellationToken);
        }

        public async Task RemoveUserAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await _cacheService.RemoveAsync(id, cancellationToken);
        }

        public async Task<bool> EditUserTelegramAsync(Guid id, string newTelegramId, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            bool hadAlreadyTelegramId = false;
            if (user.TelegramId != null)
                hadAlreadyTelegramId = true;
            user.TelegramId = newTelegramId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
            return hadAlreadyTelegramId;
        }
        public async Task EditUserAuthAsync(Guid id, AuthModel newAuth, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Login = newAuth.Login;
            user.Password = newAuth.Password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
        }
        public async Task AssignAsAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
        }
        public async Task UnassignAsAdminAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.IsAdmin = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
        }

        public async Task DebitBonusesAsync(Guid id, decimal amount, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            user.Bonuses -= amount;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
        }

        public async Task AddUserBirthDateAsync(Guid id, DateOnly birthDate, CancellationToken cancellationToken)
        {
            User? user = await _cacheService.GetAsync(id, cancellationToken);
            if (user == null)
                user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
                throw new DoesNotExistException(typeof(User));
            if (user.BirthDate != null)
                throw new WasAlreadySetException("Birthdate");
            user.BirthDate = birthDate;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
            await _cacheService.UpdateAsync(user, cancellationToken);
        }
    }
}
