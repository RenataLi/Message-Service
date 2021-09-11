using Message_Service.Models;
using Message_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Message_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private static List<UserInfo> users = new List<UserInfo>();
        private Storage _storage;
        public UserController(Storage storage)
        {
            _storage = storage;
        }
        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("create-user")]
        public IActionResult CreateUser([FromBody] CreateUserRequest req)
        {
            try
            {
                var user = _storage.CreateUser(req.Email, req.UserName);
                return Ok(user);

            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                }); ;
            }
        }
        /// <summary>
        /// Initializing a list of users and messages using Random.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpPost("get-random-users")]
        public IActionResult GetRandomUsers()
        {
            try
            {
                if (users.Count != 0)
                    users.Clear();
                Random rnd = new Random();
                int n = rnd.Next(1, 100);
                for (int i = 0; i < n; i++)
                {
                    _storage.CreateUser($"User{i}", $"User{i}");
                }
                for (int i = 0; i < n; i++)
                {
                    _storage.SendMessage($"User{i}", $"User{rnd.Next(1, n - 1)}", $"Subject{i}", $"message{i}");
                }
                return Ok(_storage.GetAllUsesrs());
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = $"{ex.Message}"
                }); ;
            }
        }
        /// <summary>
        /// Find the user by ID.
        /// </summary>
        /// <param name="id">Id of user.</param>
        /// <returns>User.</returns>
        [HttpGet("get-user-by-id")]
        public IActionResult GetUserById([FromQuery] string id)
        {
            try
            {
                var res = _storage.GetById(id);
                if (res == null)
                {
                    return NotFound(new
                    {
                        Message = $"Пользователь с Id = {id} не найден"
                    });
                }
                return Ok(res);
            }
            catch (Exception)
            {
                return NotFound(new
                {
                    Message = $"Пользователь с Id = {id} не найден"
                }); ;
            }
        }
        /// <summary>
        /// Getting all users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet("get-all-users")]
        public IActionResult GetAllUsers()
        {
            return Ok(_storage.GetAllUsesrs());
        }
        /// <summary>
        /// Getting users by limit and offset.
        /// </summary>
        /// <param name="Limit">Linit.</param>
        /// <param name="Offset">Offset.</param>
        /// <returns></returns>
        [HttpGet("get-all-users-by-LimitAndOffset")]
        public IActionResult GetAllUsers(int Limit, int Offset)
        {
            try
            {
                if (Limit <= 0 || Offset <= 0)
                {
                    return NotFound(new
                    {
                        Message = $"Значение Linit или Offset неположительно!"
                    }); ;
                }

                return Ok(_storage.GetAllUsesrs(Limit, Offset));
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = $"{ex.Message}"
                }); ;
            }
        }
    }
}
