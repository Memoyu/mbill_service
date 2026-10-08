
# 账户
# TRUNCATE table memo_bill.account;

insert into memo_bill.account ( account_id, parent_id, name, icon, sort, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, `default`, top )
select b_id, IF(parent_b_id = 0, NULL, parent_b_id), name, '', 0, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time, 0, 0  FROM mbill.mbill_asset;

# 分类
# TRUNCATE table memo_bill.category;

insert into memo_bill.category ( category_id, parent_id, name, `type`, icon, sort, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, `default`, top )
select b_id, IF(parent_b_id = 0, NULL, parent_b_id), name, `type`, '', 0, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time, 0, 0  FROM mbill.mbill_category;

# 账本
# TRUNCATE table memo_bill.ledger;

insert into memo_bill.ledger ( ledger_id, name, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, `default` )
select b_id + 1, '日常账本', b_id, current_timestamp(), b_id, current_timestamp(), 0, NULL, NULL, 1 FROM mbill.mbill_user;

# 账本成员
# TRUNCATE table memo_bill.ledger_user;

insert into memo_bill.ledger_user ( user_id, ledger_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, sort, color )
select create_user_id , ledger_id, create_user_id, current_timestamp(), update_user_id, current_timestamp(), 0, NULL, NULL, 0, 0 FROM memo_bill.ledger;


# 账单
# TRUNCATE table memo_bill.billing;

insert into memo_bill.billing ( bill_id, category_id, account_id, amount, `type`, remark, location, address, `date`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, ledger_id )
select b_id, category_b_id, asset_b_id, amount, `type`, description, '', IFNULL(address, ''), `time`, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time, (select ledger_id FROM memo_bill.ledger ld where ld.create_user_id = create_user_b_id limit 1 ) FROM mbill.mbill_bill;

# 用户
# TRUNCATE table memo_bill.user;

insert into memo_bill.user ( user_id, username, nickname, avatar, email, last_login_time, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, status, mobile )
select b_id, username, nickname, avatar_url, email, last_login_time, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time, 0, phone FROM mbill.mbill_user;

# 用户认证
# TRUNCATE table memo_bill.user_identity;

insert into memo_bill.user_identity ( identity_id, user_id, identity_type, identifier, credential, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time )
select b_id, user_b_id, IF(identity_type = 'WeiXin', 1, 0), identifier, credential, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time  FROM mbill.mbill_user_identity;

# 用户角色
# TRUNCATE table memo_bill.user_role;

insert into memo_bill.user_role ( user_id, role_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time )
select b_id, 3, create_user_b_id, create_time, update_user_b_id, update_time, is_deleted, delete_user_b_id, delete_time FROM mbill.mbill_user;










