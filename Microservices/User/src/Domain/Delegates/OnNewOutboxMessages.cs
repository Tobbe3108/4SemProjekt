using System;
using System.Collections.Generic;

namespace User.Domain.Delegates
{
    public delegate void OnNewOutboxMessages(IEnumerable<Guid> messageIds);
}