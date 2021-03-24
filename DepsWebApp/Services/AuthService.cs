using DepsWebApp.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DepsWebApp.Authentication;
using DepsWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DepsWebApp.Services
{
#pragma warning disable CS1591
    public class AuthService : IAuthService
    {
        private readonly DatabaseContext _context;

        public AuthService(DatabaseContext context)
        {
            _context = context;
        }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public async Task<string> RegisterAsync(string login, string password)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var release = await _semaphore.WaitAsync(1000);
            try
            {
                var id = await _context.Accounts.CountAsync() + 1;
                var encrypted = Base64Encryption.Encode($"{login}:{password}");
                if (await _context.Accounts.AnyAsync(account => account.Login == login)) throw new ArgumentException(" Account with this login already exists.");
                await _context.Accounts.AddAsync(new Account(id, login, password));
                return encrypted;
            }
            finally
            {
                if (release) _semaphore.Release();
            }
        }

        public Task<bool> GetUserAccount(string encryptedAccount)
        {
            if (encryptedAccount == null)
                throw new ArgumentNullException("Encrypted account must not be null.");

            if (string.IsNullOrEmpty(encryptedAccount.Trim()))
                throw new NotSupportedException(" This type is not supported.");

            var decryptedAccount = Base64Encryption.Decode(encryptedAccount).Split(':');

            return _context.Accounts.AnyAsync(account => account.Login == decryptedAccount[0] && account.Password == decryptedAccount[1]);
        }
    }
#pragma warning restore CS1591
}