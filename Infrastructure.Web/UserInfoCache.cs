using Domain.Application.Dto.Menus;
using Domain.Common;
using Infrastructure.Common;
using Infrastructure.Web.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Web
{
    public class UserInfoCache
    {
        readonly ICacheBase _cache;
        private readonly string MenuRoleCacheKey = "MenuRolePermission";
        public UserInfoCache(ICacheBase cache)
        {
            this._cache = cache;
        }

        private string GetUserKey(string userId)
        {
            return $"UserInfo:{userId}";
        }

        public UserInfo GetUser(string userId)
        {
            try
            {
                return _cache.Get<UserInfo>(GetUserKey(userId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetUser(UserInfo user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("Invalid Argument User");
            }
            _cache.Set<UserInfo>(GetUserKey(user.Id), user);
        }

        public void RemoveUser(string userId)
        {
            try
            {
                _cache.Remove(GetUserKey(userId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveMenuCaches()
        {
            try
            {
                _cache.Remove(MenuRoleCacheKey);
            }
            catch (Exception)
            {
                throw;
            }
        }

		public void UpdateMenuCaches(IEnumerable<CacheMenu> menus)
		{
            _cache.Set<List<CacheMenu>>(MenuRoleCacheKey, menus.ToList());
		}

		public List<CacheMenu> GetMenuCaches()
		{
			try
			{
				return _cache.Get<List<CacheMenu>>(MenuRoleCacheKey);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
