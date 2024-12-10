using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.Core.Helpers
{
    public enum statusConsulting
    {
        // The user has requested the consultation but hasn't paid yet
        UserRequestedNotPaid ,

        // The User is currently in Waiting 
        Waiting,

        // The consultation is currently in progress (the scheduled time has started)
        InProgress ,

        // The consultation has been completed
        Completed,

        // The consultation has been cancelled
        Cancelled

    }
}
