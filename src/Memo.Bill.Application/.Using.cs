global using System.Linq;

global using FluentValidation;
global using MediatR;
global using MapsterMapper;
global using Mapster;

global using Memo.Bill.Application.Common.Interfaces.Security;
global using Memo.Bill.Application.Common.Models;
global using Memo.Bill.Application.Common.Behaviours;
global using Memo.Bill.Domain.Constants;
global using Memo.Bill.Application.Common.Interfaces.Persistence.Repositories;
global using Memo.Bill.Application.Tokens.Common;
global using Memo.Bill.Domain.Entities;
global using Memo.Bill.Application.Common.Request;
global using Memo.Bill.Application.Common.Utils;
global using Memo.Bill.Domain.Enums;
global using Memo.Bill.Application.Common.Extensions;

global using ApiPermission = Memo.Bill.Domain.Constants.Security.Permissions.Permissions;
