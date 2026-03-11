global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;

global using Microsoft.Extensions.Configuration;

global using Memo.Bill.Application.Common.Attributes;
global using Memo.Bill.Application.Common.Security;
global using Memo.Bill.Application.Common.Utils;
global using Memo.Bill.Application.Common.Interfaces.Security;
global using Memo.Bill.Application.Common.Models;
global using Memo.Bill.Application.Common.Request;
global using Memo.Bill.Application.Common.Interfaces.Persistence.Repositories;

global using Memo.Bill.Domain.Common;
global using Memo.Bill.Domain.Constants;
global using Memo.Bill.Domain.Enums;
global using Memo.Bill.Domain.Entities;

global using Memo.Bill.Infrastructure.Security.CurrentUserProvider;
global using Memo.Bill.Infrastructure.Persistence;
global using Memo.Bill.Infrastructure.Security.TokenValidation;
global using Memo.Bill.Infrastructure.Security;
global using Memo.Bill.Infrastructure.Persistence.Repositories;
