-- 初始化角色
INSERT INTO core_me.`role` (role_id, name, `type`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, description) VALUES(1, '管理', 1, 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL, '系统管理，拥有所有权限');
INSERT INTO core_me.`role` (role_id, name, `type`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, description) VALUES(2, '游客', 1, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL, '系统演示，仅能查看数据，不能增删改操作');

-- 初始化用户
INSERT INTO core_me.`user` (user_id, username, nickname, avatar, email, phone_number, last_login_time, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 'admin', '管理员', 'http://oss.blog.memoyu.com/account/avatar/fbca90ce-9197-4a00-8836-eaafef3e0fe2.png', '', '', '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
INSERT INTO core_me.`user` (user_id, username, nickname, avatar, email, phone_number, last_login_time, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 'visitor', '游客', 'http://oss.blog.memoyu.com/account/avatar/76711486-b137-4ba5-ab4b-8fe512bf7dc1.png', '', '', '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);

-- 初始化认证方式
-- admin/Admin123
INSERT INTO core_me.user_identity (identity_id, user_id, identity_type, identifier, credential, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 1, 0, '', 'E64B78FC3BC91BCBC7DC232BA8EC59E0', 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
-- visitor/Visitor123
INSERT INTO core_me.user_identity (identity_id, user_id, identity_type, identifier, credential, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 2, 0, '', '67EBA711489333F530147FA47DB88857', 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);

-- 初始化用户角色
INSERT INTO core_me.user_role (user_id, role_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 1, 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
INSERT INTO core_me.user_role (user_id, role_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 2, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);
