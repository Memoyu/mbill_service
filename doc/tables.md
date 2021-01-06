## Statements 流水记录

|     字段      |   类型   |  长度  | 可空 |                   说明                   |
| :-----------: | :------: | :----: | :--: | :--------------------------------------: |
|      Id       |   long   |        |  ×   |                 记录主键                 |
|  CategoryId   |   long   |        |  ×   |                  分类Id                  |
|    AssetId    |   long   |        |  ×   |                  账户Id                  |
| TargetAssetId |   long   |        |  √   | 目标账户Id<br />(Type为转账、还款时赋值) |
|    Amount     | decimal  | (12,2) |  ×   |                   金额                   |
| AssetResidue  | decimal  | (12,2) |  ×   |            账户余额，默认0.0             |
|     Type      |  string  |   10   |  ×   |     记录类型：支出、收入、转账、还款     |
|  Description  |  string  |  200   |  √   |                   说明                   |
|   Location    |  string  |  200   |  √   |                   地点                   |
|     Year      |   int    |        |  ×   |               记录日期：年               |
|     Month     |   int    |        |  ×   |               记录日期：月               |
|      Day      |   int    |        |  ×   |               记录日期：日               |
|     Time      |  string  |   10   |  ×   |            记录日期：时:分:秒            |
| CreateUserId  |   long   |        |  ×   |                 创建者Id                 |
|  CreateTime   | datetime |        |  ×   |                 创建时间                 |
| UpdateUserId  |   long   |        |  √   |               最后修改人Id               |
|  UpdateTime   | datetime |        |  √   |               最后修改时间               |
|   IsDeleted   |   bool   |        |  ×   |      是否删除，0:正常,1:删除，默认0      |
| DeleteUserId  |   long   |        |  √   |                 删除人Id                 |
|  DeleteTime   | datetime |        |  √   |                 删除时间                 |

**索引：**

```
index ["type"], name: "index_statements_on_type"
index ["user_id", "asset_id"], name: "index_statements_on_user_id_and_asset_id"
index ["user_id", "category_id"], name: "index_statements_on_user_id_and_category_id"
index ["user_id", "type"], name: "index_statements_on_user_id_and_type"
index ["year", "month", "day", "time"], name: "index_statements_on_year_and_month_and_day_and_time"
```



## Category  分类

|     字段     |   类型   |  长度  | 可空 |              说明              |
| :----------: | :------: | :----: | :--: | :----------------------------: |
|      Id      |   long   |        |  ×   |            分类主键            |
|     Name     |  string  |   20   |  ×   |            分类名称            |
|   ParentId   |   long   |        |  ×   |         父级Id，默认0          |
|     Type     |  string  |   20   |  ×   |      分类类型：支出、收入      |
|    Budget    | decimal  | (12,2) |  ×   |       预算金额，默认0.0        |
|   IconUrl    |  string  |  100   |  ×   |            图标路径            |
|     Sort     |   int    |        |  ×   |          排序，默认0           |
| CreateUserId |   long   |        |  ×   |            创建者Id            |
|  CreateTime  | datetime |        |  ×   |            创建时间            |
| UpdateUserId |   long   |        |  √   |          最后修改人Id          |
|  UpdateTime  | datetime |        |  √   |          最后修改时间          |
|  IsDeleted   |   bool   |        |  ×   | 是否删除，0:正常,1:删除，默认0 |
| DeleteUserId |   long   |        |  √   |            删除人Id            |
|  DeleteTime  | datetime |        |  √   |            删除时间            |

**索引：

```
index ["order"], name: "index_categories_on_order"
index ["parent_id"], name: "index_categories_on_parent_id"
index ["type"], name: "index_categories_on_type"
```



## Asset  资产

|     字段     |   类型   |  长度  | 可空 |              说明              |
| :----------: | :------: | :----: | :--: | :----------------------------: |
|      Id      |   long   |        |  ×   |            资产主键            |
|     Name     |  string  |   20   |  ×   |            资产名称            |
|   ParentId   |   long   |        |  ×   |         父级Id，默认0          |
|     Type     |  string  |   20   |  ×   |      资产类型：存款、负债      |
|    Amount    | decimal  | (12,2) |  ×   |       资产金额，默认0.0        |
|   IconUrl    |  string  |  100   |  ×   |            图标路径            |
|     Sort     |   int    |        |  ×   |          排序，默认0           |
| CreateUserId |   long   |        |  ×   |            创建者Id            |
|  CreateTime  | datetime |        |  ×   |            创建时间            |
| UpdateUserId |   long   |        |  √   |          最后修改人Id          |
|  UpdateTime  | datetime |        |  √   |          最后修改时间          |
|  IsDeleted   |   bool   |        |  ×   | 是否删除，0:正常,1:删除，默认0 |
| DeleteUserId |   long   |        |  √   |            删除人Id            |
|  DeleteTime  | datetime |        |  √   |            删除时间            |

**索引：**

    index ["amount"], name: "index_assets_on_amount"
    index ["order"], name: "index_assets_on_order"
    index ["parent_id"], name: "index_assets_on_parent_id"
    index ["type"], name: "index_assets_on_type"


## User  用户

|     字段     |   类型   | 长度 | 可空 |              说明              |
| :----------: | :------: | :--: | :--: | :----------------------------: |
|      Id      |   long   |      |  ×   |            用户主键            |
|     Name     |  string  |  20  |  ×   |             用户名             |
|   NickName   |  string  |  20  |  ×   |              昵称              |
|    Gender    |   int    |      |  ×   | 性别，0:未知,1:男,2:女，默认0  |
|    Email     |  string  |  60  |  √   |            邮箱地址            |
|    Phone     |  string  |  20  |  √   |            手机号码            |
|   Province   |  string  |  20  |  √   |               省               |
|     City     |  string  |  20  |  √   |               市               |
|   District   |  string  |  20  |  √   |               区               |
|    Street    |  string  |  50  |  √   |              街道              |
|  AvatarUrl   |  string  | 100  |  √   |            头像地址            |
| CreateUserId |   long   |      |  ×   |            创建者Id            |
|  CreateTime  | datetime |      |  ×   |            创建时间            |
| UpdateUserId |   long   |      |  √   |          最后修改人Id          |
|  UpdateTime  | datetime |      |  √   |          最后修改时间          |
|  IsDeleted   |   bool   |      |  ×   | 是否删除，0:正常,1:删除，默认0 |
| DeleteUserId |   long   |      |  √   |            删除人Id            |
|  DeleteTime  | datetime |      |  √   |            删除时间            |

