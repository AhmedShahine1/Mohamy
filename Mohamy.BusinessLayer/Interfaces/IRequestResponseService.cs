﻿using Mohamy.Core.DTO.AuthViewModel.RequesrLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IRequestResponseService
    {
        Task<Task> AddLog(RequestResponseLog log);
        Task<List<RequestResponseLog>> GetAllLogsAsync();
    }
}
