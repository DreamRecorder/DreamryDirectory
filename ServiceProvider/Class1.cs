using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using System;
using System.Collections.Generic;

namespace ServiceProvider
{
    public class DirectoryProvider : IDirectoryProvider
    {
        public AccessToken Access(EntityToken token, Guid target) => throw new NotImplementedException();
        public AccessType AccessProperty(EntityToken token, Guid target, string name) => throw new NotImplementedException();
        public void AddToGroup(EntityToken token, Guid group, Guid target) => throw new NotImplementedException();
        public EntityToken ChangeToken(EntityToken token, Guid target) => throw new NotImplementedException();
        public bool CheckToken(EntityToken token, AccessToken tokenToCheck) => throw new NotImplementedException();
        public bool Contain(EntityToken token, Guid group, Guid user) => throw new NotImplementedException();
        public Guid CreateGroup(EntityToken token) => throw new NotImplementedException();
        public Guid CreateUser(EntityToken token) => throw new NotImplementedException();
        public string GetProperty(EntityToken token, Guid target, string name) => throw new NotImplementedException();
        public ICollection<Guid> ListGroup(EntityToken token, Guid group) => throw new NotImplementedException();
        public EntityToken Login(LoginToken token) => throw new NotImplementedException();
        public void RegisterLogin(EntityToken loginServiceToken, EntityToken targetToken) => throw new NotImplementedException();
        public void RemoveFromGroup(EntityToken token, Guid group, Guid target) => throw new NotImplementedException();
        public void SetProperty(EntityToken token, Guid target, string name, string value) => throw new NotImplementedException();
    }
}
