﻿using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.DTO.Response.User;
using JS.Base.WS.API.Global;
using JS.Base.WS.API.Models.Permission;
using JS.Base.WS.API.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JS.Base.WS.API.Services
{
    public class UserRoleService : IUserRoleService
    {
        MyDBcontext db = new MyDBcontext();

        public List<UserRoleDto> GetAll()
        {
            var result = db.UserRoles
                        .Where(x => x.IsActive == true)
                        .Select(x => new UserRoleDto
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            UserName = x.User.UserName,
                            FullName = x.User.Person == null ? null : x.User.Person.FullName,
                            RoleId = x.RoleId,
                            RoleName = x.Role.Description,
                            CreationTime = x.CreationTime,
                            CreatorUserId = x.CreatorUserId,
                            LastModificationTime = x.LastModificationTime,
                            LastModifierUserId = x.LastModifierUserId,
                            DeletionTime = x.DeletionTime,
                            DeleterUserId = x.DeleterUserId,
                            IsActive = x.IsActive,
                            IsDeleted = x.IsDeleted,
                        })
                        .OrderByDescending(x => x.Id)
                        .ToList();

            return result;
        }

        public UserRoleDto GetById(long Id)
        {
            var result = db.UserRoles
                        .Where(x => x.Id == Id && x.IsActive == true)
                        .Select(x => new UserRoleDto
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            UserName = x.User.UserName,
                            FullName = x.User.Person == null ? null : x.User.Person.FullName,
                            RoleId = x.RoleId,
                            RoleName = x.Role.Description,
                            CreationTime = x.CreationTime,
                            CreatorUserId = x.CreatorUserId,
                            LastModificationTime = x.LastModificationTime,
                            LastModifierUserId = x.LastModifierUserId,
                            DeletionTime = x.DeletionTime,
                            DeleterUserId = x.DeleterUserId,
                            IsActive = x.IsActive,
                            IsDeleted = x.IsDeleted,
                        }).FirstOrDefault();

            return result;
        }

        public bool CreateUserRol(long UserId, string code)
        {
            bool Result = false;
            int RoleId = 0;

            if (string.IsNullOrEmpty(code))
            {
                string RoleShortName = Constants.ConfigurationParameter.RoleExternalUser;
                RoleId = db.Roles.Where(x => x.ShortName == RoleShortName).Select(x => x.Id).FirstOrDefault();
            }
            else
            {
                RoleId = db.Roles.Where(x => x.Code == code).Select(x => x.Id).FirstOrDefault();
                if (RoleId == 0)
                {
                    return Result;
                }
            }

            var systemUser = db.Users.Where(x => x.UserName == "system").FirstOrDefault();

            var Role = new UserRole
            {
                Id = 0,
                UserId = UserId,
                RoleId = RoleId,
                CreationTime = DateTime.Now,
                CreatorUserId = systemUser.Id,
                IsActive = true,
            };

            db.UserRoles.Add(Role);
            db.SaveChanges();
            return Result;
        }

        public bool ValidateSecurityCode(string code)
        {
            bool Result = true;

            if (!string.IsNullOrEmpty(code))
            {
                int RoleId = db.Roles.Where(x => x.Code == code).Select(x => x.Id).FirstOrDefault();
                if (RoleId > 0)
                {
                    Result = true;
                }
                else
                {
                    Result = false;
                }

            }

            return Result;
        }
    }
}