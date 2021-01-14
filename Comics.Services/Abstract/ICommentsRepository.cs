using System;
using System.Collections.Generic;
using System.Text;
using Comics.Domain;

namespace Comics.Services.Abstract
{
    public interface ICommentsRepository
    {
        public void AddComm(Comment comm);
        public void RemoveComm(Comment comm);
    }
}
