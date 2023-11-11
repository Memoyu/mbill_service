﻿global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Mbill.Service.Core.Auth.Input;
global using Mbill.Service.Core.User.Output;
global using Mbill.Core.AOP.Attributes;
global using Mbill.Core.Domains.Common;
global using Mbill.Core.Domains.Common.Enums.Base;
global using Mbill.Core.Domains.Entities.Core;
global using Mbill.Core.Domains.Entities.User;
global using Mbill.Core.Domains.Entities.Bill;
global using Mbill.Core.Domains.Entities.PreOrder;
global using Mbill.Core.Exceptions;
global using Mbill.Core.Extensions;
global using Mbill.Core.Interface.IRepositories.Core;
global using Mbill.Core.Interface.IRepositories.PreOrder;
global using Mbill.Service.Base;
global using Mbill.Service.Core.User;
global using Mbill.Service.Core.User.Input;
global using Mbill.ToolKits.Utils;
global using Mbill.Core.Domains.Common.Consts;
global using Mbill.Service.Core.Auth.Output;
global using Mbill.Service.Core.Wx;
global using Microsoft.Extensions.Logging;
global using Mbill.Service.Bill.Bill.Output;
global using Mbill.Core.Domains.Common.Enums;
global using Mbill.Core.Interface.IRepositories.Bill;
global using System.Linq.Expressions;
global using Mbill.Service.Bill.Bill.Input;
global using Microsoft.Extensions.DependencyInjection;
global using FreeSql;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.Extensions.Logging.Abstractions;
global using Mbill.Core.Security;
global using Mbill.Core.Domains.Common.Base;
global using Mbill.Core.Interface.IRepositories.Base;
global using System.ComponentModel.DataAnnotations;
global using Mbill.Service.Bill.Asset.Output;
global using Mbill.Service.Bill.Asset.Input;
global using Mbill.Service.Bill.Category.Output;
global using System.Data;
global using Mbill.Service.Bill.Category.Input;
global using Mbill.Service.Core.Logger.Output;
global using Mbill.Service.Core.Permission.Input;
global using Mbill.Service.Core.Permission.Output;
global using Mbill.Service.Core.Logger.Input;
global using Mbill.Core.Common.Configs;
global using DotNetCore.Security;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using Mbill.Core.Interface.IDependency;
global using Mbill.Service.Core.Files.Output;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Hosting;
global using Newtonsoft.Json;
global using Mbill.Service.Core.Wx.Output;
global using System.Net.Http;
global using Mbill.Service.PreOrder.Input;
global using Mbill.Service.PreOrder.Output;
global using Mbill.Service.Core.Files.Input;
global using static Mbill.Core.Domains.Common.Consts.SystemConst;