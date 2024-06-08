using ITService.DataAccess.Context;
using System;
using System.Collections.Generic;
using ITService.Model;
using Microsoft.EntityFrameworkCore;

namespace ITService.UI.Data
{
    public interface IUserRoleDataProvider
    {
        Task<IEnumerable<UserRole>?> Get();
    }

    class UserRoleDataProvider : IUserRoleDataProvider
    {
        private ItServiceDb _context;

        public UserRoleDataProvider(ItServiceDb context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserRole>?> Get()
        {
            return await _context.UserRoles
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
