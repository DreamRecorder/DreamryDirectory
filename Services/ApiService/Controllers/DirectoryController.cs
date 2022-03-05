using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.ToolBox.General;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DreamRecorder.Directory.Services.ApiService.Controllers
{

	[ApiController]
	public class DirectoryController : ControllerBase
	{

		private readonly ILogger<DirectoryController> _logger;

		private readonly IDirectoryService _directoryService;

		private IDirectoryService DirectoryService
		{
			get
			{
				lock (this)
				{
					if (_directoryService is IStartStop { IsRunning: false, } startStop)
					{
						startStop.Start();
					}

					return _directoryService;
				}

			}
		}

		public DirectoryController(ILogger<DirectoryController> logger, IDirectoryService directoryService)
		{
			_logger = logger;
			_directoryService = directoryService;

			if (DirectoryService is IStartStop startStop)
			{
				startStop.Start();
			}
		}

		[HttpPost("GetTime")]
		public ActionResult<DateTimeOffset> GetTime() => DirectoryService.GetTime();

		[HttpPost("GetStartupTime")]
		public ActionResult<DateTimeOffset> GetStartupTime() => DirectoryService.GetStartupTime();

		[HttpPost("GetVersion")]
		public ActionResult<Version> GetVersion()
			=> DirectoryService.GetType().Assembly.GetName().Version;

		[HttpPost("Login")]
		public ActionResult<EntityToken> Login([FromBody] LoginToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.Login(token);
		}

		[HttpPost(@"DisposeToken")]
		public ActionResult DisposeToken([FromBody] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.DisposeToken(token);

			return Ok();
		}

		[HttpPost(@"DisposeToken")]
		public ActionResult DisposeToken([FromBody] LoginToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.DisposeToken(token);

			return Ok();
		}

		[HttpPost(@"DisposeToken")]
		public ActionResult DisposeToken([FromBody] AccessToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.DisposeToken(token);

			return Ok();
		}

		[HttpPost(@"UpdateToken")]
		public ActionResult<EntityToken> UpdateToken([FromHeader] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.UpdateToken(token);
		}

		[HttpPost(@"Access/{target:guid}")]
		public ActionResult<AccessToken> Access([FromHeader] EntityToken token, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.Access(token, target);
		}

		[HttpPost(@"GetProperty/{target:guid}/{name}")]
		public ActionResult<string> GetProperty([FromHeader] EntityToken token, Guid target, string name)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.GetProperty(token, target, name);
		}

		[HttpPost(@"GetPropertyOwner/{target:guid}/{name}")]
		public ActionResult<Guid> GetPropertyOwner([FromHeader] EntityToken token, Guid target, string name)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.GetPropertyOwner(token, target, name);
		}


		[HttpPost("SetProperty/{target:guid}/{name}")]
		public ActionResult SetProperty(
			[FromHeader] EntityToken token,
			Guid target,
			string name,
			[FromBody] string value)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.SetProperty(token, target, name, value);

			return Ok();
		}

		[HttpPost("TransferProperty/{target:guid}/{name}/{newOwner:guid}")]
		public ActionResult TransferProperty(
			[FromHeader] EntityToken token,
			Guid target,
			string name,
			Guid newOwner)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.TransferProperty(token, target, name, newOwner);

			return Ok();
		}

		[HttpPost("AccessProperty/{target:guid}/{name}")]
		public ActionResult<AccessType> AccessProperty([FromHeader] EntityToken token, Guid target, string name)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.AccessProperty(token, target, name);
		}

		[HttpPost("SetPropertyPermission/{target:guid}/{name}/{permissionGroup:guid}")]
		public ActionResult SetPropertyPermission(
			[FromHeader] EntityToken token,
			Guid target,
			string name,
			Guid permissionGroup)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.SetPropertyPermission(token, target, name, permissionGroup);

			return Ok();
		}

		[HttpPost("GetPermissionGroup/{target:guid}")]
		public ActionResult<PermissionGroup> GetPermissionGroup([FromHeader] EntityToken token, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.GetPermissionGroup(token, target);
		}

		[HttpPost("UpdatePermissionGroup")]
		public ActionResult<PermissionGroup> UpdatePermissionGroup(
			[FromHeader] EntityToken token,
			[FromBody] PermissionGroup target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			if (target is null)
			{
				return BadRequest();
			}

			return DirectoryService.UpdatePermissionGroup(token, target);
		}

		[HttpPost("Contain/{group:guid}/{target:guid}")]
		public ActionResult<bool> Contain([FromHeader] EntityToken token, Guid group, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.Contain(token, group, target);
		}

		[HttpPost("ListGroup/{group:guid}")]
		public ActionResult<ICollection<Guid>> ListGroup([FromHeader] EntityToken token, Guid group)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.ListGroup(token, group).ToList();
		}

		[HttpPost("AddToGroup/{group:guid}/{target:guid}")]
		public ActionResult AddToGroup([FromHeader] EntityToken token, Guid group, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.AddToGroup(token, group, target);

			return Ok();
		}

		[HttpPost("RemoveFromGroup/{group:guid}/{target:guid}")]
		public ActionResult RemoveFromGroup([FromHeader] EntityToken token, Guid group, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.RemoveFromGroup(token, group, target);

			return Ok();
		}

		[HttpPost("CheckToken")]
		public ActionResult CheckToken([FromHeader] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			DirectoryService.CheckToken(token);

			return Ok();
		}

		[HttpPost("CheckToken")]
		public ActionResult CheckToken([FromHeader] EntityToken token, [FromBody] AccessToken tokenToCheck)
		{
			if (token is null)
			{
				return BadRequest();
			}

			if (tokenToCheck is null)
			{
				return BadRequest();
			}

			DirectoryService.CheckToken(token, tokenToCheck);

			return Ok();
		}

		[HttpPost("CheckToken")]
		public ActionResult CheckToken([FromHeader] EntityToken token, [FromBody] EntityToken tokenToCheck)
		{
			if (token is null)
			{
				return BadRequest();
			}

			if (tokenToCheck is null)
			{
				return BadRequest();
			}

			DirectoryService.CheckToken(token, tokenToCheck);

			return Ok();
		}

		[HttpPost(@"CreateUser")]
		public ActionResult<Guid> CreateUser([FromHeader] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.CreateUser(token);
		}

		[HttpPost(@"CreateGroup")]
		public ActionResult<Guid> CreateGroup([FromHeader] EntityToken token)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.CreateGroup(token);
		}

		[HttpPost(@"RegisterLogin")]
		public ActionResult RegisterLogin([FromHeader] EntityToken token, [FromBody] LoginToken targetToken)
		{
			if (token is null)
			{
				return BadRequest();
			}

			if (targetToken == null)
			{
				return BadRequest();
			}

			DirectoryService.RegisterLogin(token, targetToken);

			return Ok();
		}

		[HttpPost(@"GetLoginTokenLife/{target:guid}")]
		public ActionResult<TimeSpan> GetLoginTokenLife([FromHeader] EntityToken token, Guid target)
		{
			if (token is null)
			{
				return BadRequest();
			}

			return DirectoryService.GetLoginTokenLife(token, target);
		}

	}

}
