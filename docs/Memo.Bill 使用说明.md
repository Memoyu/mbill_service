# Memo.Bill 项目模板使用



## 配置

全部配置于项目`src/xx.Api`下的`appsettings.json`中管理，不同环境下调整对应环境的`appsettings.{Env}.json`配置：

- IP限流：IpRateLimiting
- 数据库：ConnectionStrings:MySql
- Redis：ConnectionStrings:Redis
- MongoDB：Mongo
- 跨域：CorsOrigins
- 应用授权认证信息：Authorization:Jwt
- 第三方认证信息：Authorization:JQiniu

根据项目情况自行配置；



## 初始化数据

项目提供用户、角色等数据库初始化脚本；

> 需要先成功启动项目，生成表结构（code first）,再执行数据初始化脚本

脚本文件：`docs/db/init_db.sql`

