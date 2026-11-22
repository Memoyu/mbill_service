-- 初始化角色
INSERT INTO memo_bill.`role` (role_id, name, `type`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, description) VALUES(1, '管理', 1, 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL, '系统管理，拥有所有权限');
INSERT INTO memo_bill.`role` (role_id, name, `type`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, description) VALUES(2, '游客', 1, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL, '系统演示，仅能查看数据，不能增删改操作');
INSERT INTO memo_bill.`role` (role_id, name, `type`, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time, description) VALUES(3, '用户', 1, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL, '系统用户，部分权限');

-- 初始化用户
INSERT INTO memo_bill.`user` (user_id, username, nickname, avatar, email, mobile, last_login_time, status, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 'admin', '管理员', 'http://oss.blog.memoyu.com/account/avatar/fbca90ce-9197-4a00-8836-eaafef3e0fe2.png', '', '', '2024-02-01 19:01:00', 0, 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
INSERT INTO memo_bill.`user` (user_id, username, nickname, avatar, email, mobile, last_login_time, status, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 'visitor', '游客', 'http://oss.blog.memoyu.com/account/avatar/76711486-b137-4ba5-ab4b-8fe512bf7dc1.png', '', '', '2024-02-01 19:01:01', 0, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);

-- 初始化认证方式
-- admin/Admin123
INSERT INTO memo_bill.user_identity (identity_id, user_id, identity_type, identifier, credential, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 1, 0, '', 'E64B78FC3BC91BCBC7DC232BA8EC59E0', 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
-- visitor/Visitor123
INSERT INTO memo_bill.user_identity (identity_id, user_id, identity_type, identifier, credential, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 2, 0, '', '67EBA711489333F530147FA47DB88857', 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);

-- 初始化用户角色
INSERT INTO memo_bill.user_role (user_id, role_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(1, 1, 0, '2024-02-01 19:01:00', 0, '2024-02-01 19:01:00', 0, NULL, NULL);
INSERT INTO memo_bill.user_role (user_id, role_id, create_user_id, create_time, update_user_id, update_time, is_deleted, delete_user_id, delete_time) VALUES(2, 2, 0, '2024-02-01 19:01:01', 0, '2024-02-01 19:01:01', 0, NULL, NULL);

-- 初始化默认账户数据
INSERT INTO `memo_bill`.`account` (`account_id`, `parent_id`, `name`, `icon`, `sort`, `create_user_id`, `create_time`, `update_user_id`, `update_time`, `is_deleted`, `delete_user_id`, `delete_time`, `default`, `top`) VALUES
(13800656210427901, NULL, '现金', 'https://oss.memoyu.com/icons/finance/21.png', 0, 0, '2026-10-30 18:14:20.658', 0, '2026-11-05 16:41:14.303', 0, NULL, NULL, 1, 0),
(13800656210427902, NULL, '虚拟账户', 'https://oss.memoyu.com/icons/pay/7.png', 1, 0, '2026-10-30 18:13:07.678', 0, '2026-11-05 16:41:14.303', 0, NULL, NULL, 1, 0),
(13800656210427903, 13800656210427902, '微信', 'https://oss.memoyu.com/icons/pay/18.png', 0, 0, '2026-10-30 18:13:15.781', 0, '2026-10-30 18:13:48.059', 0, NULL, NULL, 1, 0),
(13800656210427904, 13800656210427902, '支付宝', 'https://oss.memoyu.com/icons/pay/20.png', 1, 0, '2026-10-30 18:13:24.373', 0, '2026-10-30 18:13:24.373', 0, NULL, NULL, 1, 0),
(13800656210427905, NULL, '储蓄卡', 'https://oss.memoyu.com/icons/finance/28.png', 2, 0, '2026-10-30 18:14:30.555', 0, '2026-11-05 16:41:14.303', 0, NULL, NULL, 1, 0),
(13800656210427906, 13800656210427905, '中行', 'https://oss.memoyu.com/icons/bank/2.png', 0, 0, '2026-10-30 18:14:47.042', 0, '2026-10-30 18:15:55.748', 0, NULL, NULL, 1, 0),
(13800656210427907, 13800656210427905, '招商', 'https://oss.memoyu.com/icons/bank/12.png', 1, 0, '2026-10-30 18:15:02.050', 0, '2026-10-30 18:15:49.908', 0, NULL, NULL, 1, 0),
(13800656210427908, 13800656210427905, '农行', 'https://oss.memoyu.com/icons/bank/6.png', 2, 0, '2026-10-30 18:16:03.151', 0, '2026-10-30 18:18:39.951', 0, NULL, NULL, 1, 0),
(13800656210427909, 13800656210427905, '工行', 'https://oss.memoyu.com/icons/bank/8.png', 3, 0, '2026-10-30 18:16:45.184', 0, '2026-10-30 18:19:33.372', 0, NULL, NULL, 1, 0),
(13800656210427910, 13800656210427905, '邮政', 'https://oss.memoyu.com/icons/bank/14.png', 4, 0, '2026-10-30 18:18:06.067', 0, '2026-10-30 18:19:08.516', 0, NULL, NULL, 1, 0),
(13800656210427911, NULL, '信用卡', 'https://oss.memoyu.com/icons/finance/29.png', 3, 0, '2026-10-30 18:26:01.937', 0, '2026-11-05 16:41:14.303', 0, NULL, NULL, 1, 0),
(13800656210427912, 13800656210427911, '中行', 'https://oss.memoyu.com/icons/bank/2.png', 0, 0, '2026-10-30 18:31:47.718', 0, '2026-10-30 18:31:47.718', 0, NULL, NULL, 1, 0),
(13800656210427913, 13800656210427911, '招商', 'https://oss.memoyu.com/icons/bank/12.png', 1, 0, '2026-10-30 18:32:03.355', 0, '2026-10-30 18:32:03.355', 0, NULL, NULL, 1, 0),
(13800656210427914, 13800656210427911, '农行', 'https://oss.memoyu.com/icons/bank/6.png', 2, 0, '2026-10-30 18:32:09.318', 0, '2026-10-30 18:32:09.318', 0, NULL, NULL, 1, 0),
(13800656210427915, 13800656210427911, '工行', 'https://oss.memoyu.com/icons/bank/8.png', 3, 0, '2026-10-30 18:32:16.664', 0, '2026-10-30 18:32:16.664', 0, NULL, NULL, 1, 0),
(13800656210427916, 13800656210427911, '邮政', 'https://oss.memoyu.com/icons/bank/14.png', 4, 0, '2026-10-30 18:32:22.300', 0, '2026-10-30 18:32:22.300', 0, NULL, NULL, 1, 0);

-- 初始化默认分类数据
INSERT INTO `memo_bill`.`category` (`category_id`, `parent_id`, `name`, `type`, `icon`, `sort`, `create_user_id`, `create_time`, `update_user_id`, `update_time`, `is_deleted`, `delete_user_id`, `delete_time`, `default`, `top`) VALUES
(13800656210427917, NULL, '餐饮', 0, 'https://oss.memoyu.com/icons/foot/68.png', 0, 0, '2026-10-30 16:14:51.155', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427918, 13800656210427917, '早餐', 0, 'https://oss.memoyu.com/icons/foot/2.png', 0, 0, '2026-10-30 16:15:05.325', 0, '2026-10-30 16:15:05.325', 0, NULL, NULL, 1, 0),
(13800656210427919, 13800656210427917, '午餐', 0, 'https://oss.memoyu.com/icons/foot/22.png', 1, 0, '2026-10-30 16:15:22.966', 0, '2026-10-30 16:15:22.966', 0, NULL, NULL, 1, 0),
(13800656210427920, 13800656210427917, '晚餐', 0, 'https://oss.memoyu.com/icons/foot/9.png', 2, 0, '2026-10-30 16:15:34.483', 0, '2026-10-30 16:15:34.483', 0, NULL, NULL, 1, 0),
(13800656210427921, 13800656210427917, '饮料', 0, 'https://oss.memoyu.com/icons/foot/52.png', 3, 0, '2026-10-30 16:16:00.039', 0, '2026-10-30 16:16:00.039', 0, NULL, NULL, 1, 0),
(13800656210427922, 13800656210427917, '水果', 0, 'https://oss.memoyu.com/icons/foot/10.png', 4, 0, '2026-10-30 16:20:46.072', 0, '2026-10-30 16:20:46.072', 0, NULL, NULL, 1, 0),
(13800656210427923, 13800656210427917, '零食', 0, 'https://oss.memoyu.com/icons/foot/18.png', 5, 0, '2026-10-30 16:20:56.182', 0, '2026-10-30 16:20:56.182', 0, NULL, NULL, 1, 0),
(13800656210427924, NULL, '交通', 0, 'https://oss.memoyu.com/icons/transport/1.png', 3, 0, '2026-10-30 16:21:47.889', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427925, 13800656210427924, '地铁/公交', 0, 'https://oss.memoyu.com/icons/transport/4.png', 0, 0, '2026-10-30 16:22:14.473', 0, '2026-10-30 16:22:14.473', 0, NULL, NULL, 1, 0),
(13800656210427926, 13800656210427924, '打车', 0, 'https://oss.memoyu.com/icons/transport/5.png', 1, 0, '2026-10-30 16:22:27.678', 0, '2026-10-30 16:22:27.678', 0, NULL, NULL, 1, 0),
(13800656210427927, 13800656210427924, '充电', 0, 'https://oss.memoyu.com/icons/transport/3.png', 2, 0, '2026-10-30 16:23:03.556', 0, '2026-10-30 16:23:03.556', 0, NULL, NULL, 1, 0),
(13800656210427928, 13800656210427924, '加油', 0, 'https://oss.memoyu.com/icons/transport/6.png', 3, 0, '2026-10-30 16:23:10.908', 0, '2026-10-30 16:23:10.908', 0, NULL, NULL, 1, 0),
(13800656210427929, 13800656210427924, '停车/车位', 0, 'https://oss.memoyu.com/icons/transport/25.png', 4, 0, '2026-10-30 16:29:40.165', 0, '2026-10-30 16:38:37.473', 0, NULL, NULL, 1, 0),
(13800656210427930, 13800656210427924, '高速费', 0, 'https://oss.memoyu.com/icons/transport/26.png', 5, 0, '2026-10-30 16:34:44.132', 0, '2026-10-30 16:34:44.132', 0, NULL, NULL, 1, 0),
(13800656210427931, NULL, '生活', 0, 'https://oss.memoyu.com/icons/home/49.png', 1, 0, '2026-10-30 16:42:53.074', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427932, 13800656210427931, '房贷', 0, 'https://oss.memoyu.com/icons/finance/3.png', 0, 0, '2026-10-30 16:52:45.480', 0, '2026-10-30 16:53:11.875', 0, NULL, NULL, 1, 0),
(13800656210427933, 13800656210427931, '车贷', 0, 'https://oss.memoyu.com/icons/finance/4.png', 1, 0, '2026-10-30 16:53:16.997', 0, '2026-10-30 16:53:16.997', 0, NULL, NULL, 1, 0),
(13800656210427934, 13800656210427931, '房租', 0, 'https://oss.memoyu.com/icons/finance/23.png', 2, 0, '2026-10-30 16:53:28.193', 0, '2026-10-30 16:53:28.193', 0, NULL, NULL, 1, 0),
(13800656210427935, 13800656210427931, '水电燃气', 0, 'https://oss.memoyu.com/icons/home/50.png', 3, 0, '2026-10-30 17:00:57.248', 0, '2026-10-30 17:00:57.248', 0, NULL, NULL, 1, 0),
(13800656210427936, 13800656210427931, '会员订阅', 0, 'https://oss.memoyu.com/icons/home/6.png', 4, 0, '2026-10-30 17:01:37.800', 0, '2026-10-30 17:01:37.800', 0, NULL, NULL, 1, 0),
(13800656210427937, 13800656210427931, '物业费', 0, 'https://oss.memoyu.com/icons/home/51.png', 5, 0, '2026-10-30 17:11:36.853', 0, '2026-10-30 17:11:36.853', 0, NULL, NULL, 1, 0),
(13800656210427938, 13800656210427931, '维修维护', 0, 'https://oss.memoyu.com/icons/home/52.png', 6, 0, '2026-10-30 17:16:24.130', 0, '2026-10-30 17:16:24.130', 0, NULL, NULL, 1, 0),
(13800656210427939, 13800656210427931, '服饰', 0, 'https://oss.memoyu.com/icons/clothe/14.png', 7, 0, '2026-10-30 17:22:24.837', 0, '2026-10-30 17:22:24.837', 0, NULL, NULL, 1, 0),
(13800656210427940, 13800656210427931, '日用百货', 0, 'https://oss.memoyu.com/icons/home/37.png', 8, 0, '2026-10-30 17:22:43.249', 0, '2026-10-30 17:22:43.249', 0, NULL, NULL, 1, 0),
(13800656210427941, 13800656210427931, '美妆护肤', 0, 'https://oss.memoyu.com/icons/home/42.png', 9, 0, '2026-10-30 17:23:02.133', 0, '2026-10-30 17:23:02.133', 0, NULL, NULL, 1, 0),
(13800656210427942, 13800656210427931, '理发', 0, 'https://oss.memoyu.com/icons/beauty/21.png', 10, 0, '2026-10-30 17:26:36.915', 0, '2026-10-30 17:26:36.915', 0, NULL, NULL, 1, 0),
(13800656210427943, 13800656210427931, '话费/宽带', 0, 'https://oss.memoyu.com/icons/3c/21.png', 11, 0, '2026-10-30 17:35:27.626', 0, '2026-10-30 17:35:27.626', 0, NULL, NULL, 1, 0),
(13800656210427944, NULL, '娱乐', 0, 'https://oss.memoyu.com/icons/3c/13.png', 2, 0, '2026-10-30 17:18:18.661', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427945, 13800656210427944, '电影', 0, 'https://oss.memoyu.com/icons/3c/7.png', 0, 0, '2026-10-30 17:19:33.124', 0, '2026-10-30 17:19:33.124', 0, NULL, NULL, 1, 0),
(13800656210427946, 13800656210427944, '游戏', 0, 'https://oss.memoyu.com/icons/3c/13.png', 1, 0, '2026-10-30 17:19:55.044', 0, '2026-10-30 17:19:55.044', 0, NULL, NULL, 1, 0),
(13800656210427947, 13800656210427944, '旅游', 0, 'https://oss.memoyu.com/icons/home/19.png', 2, 0, '2026-10-30 17:20:19.013', 0, '2026-10-30 17:20:57.670', 0, NULL, NULL, 1, 0),
(13800656210427948, 13800656210427944, '运动健身', 0, 'https://oss.memoyu.com/icons/home/9.png', 3, 0, '2026-10-30 17:20:32.326', 0, '2026-10-30 17:21:29.598', 0, NULL, NULL, 1, 0),
(13800656210427949, 13800656210427944, 'KTV', 0, 'https://oss.memoyu.com/icons/3c/19.png', 4, 0, '2026-10-30 17:21:16.714', 0, '2026-10-30 17:21:16.714', 0, NULL, NULL, 1, 0),
(13800656210427950, NULL, '人情', 0, 'https://oss.memoyu.com/icons/home/54.png', 6, 0, '2026-10-30 17:43:24.666', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427951, 13800656210427950, '礼物', 0, 'https://oss.memoyu.com/icons/home/54.png', 0, 0, '2026-10-30 17:45:30.881', 0, '2026-10-30 17:45:30.881', 0, NULL, NULL, 1, 0),
(13800656210427952, 13800656210427950, '红包', 0, 'https://oss.memoyu.com/icons/home/53.png', 1, 0, '2026-10-30 17:45:38.616', 0, '2026-10-30 17:45:38.616', 0, NULL, NULL, 1, 0),
(13800656210427953, NULL, '医疗', 0, 'https://oss.memoyu.com/icons/medical/9.png', 4, 0, '2026-10-30 17:46:35.665', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427954, 13800656210427953, '门诊挂号', 0, 'https://oss.memoyu.com/icons/medical/17.png', 0, 0, '2026-10-30 17:46:54.600', 0, '2026-10-30 17:46:54.600', 0, NULL, NULL, 1, 0),
(13800656210427955, 13800656210427953, '药品', 0, 'https://oss.memoyu.com/icons/medical/19.png', 1, 0, '2026-10-30 17:47:11.233', 0, '2026-10-30 17:47:11.233', 0, NULL, NULL, 1, 0),
(13800656210427956, 13800656210427953, '体检保健', 0, 'https://oss.memoyu.com/icons/medical/1.png', 2, 0, '2026-10-30 17:47:22.264', 0, '2026-10-30 17:47:22.264', 0, NULL, NULL, 1, 0),
(13800656210427957, 13800656210427953, '治疗', 0, 'https://oss.memoyu.com/icons/medical/10.png', 3, 0, '2026-10-30 17:47:42.574', 0, '2026-10-30 17:47:42.574', 0, NULL, NULL, 1, 0),
(13800656210427958, NULL, '教育', 0, 'https://oss.memoyu.com/icons/stationery/12.png', 5, 0, '2026-10-30 17:48:28.360', 0, '2026-11-05 16:39:40.051', 0, NULL, NULL, 1, 0),
(13800656210427959, 13800656210427958, '培训课程', 0, 'https://oss.memoyu.com/icons/stationery/9.png', 0, 0, '2026-10-30 17:49:09.397', 0, '2026-10-30 17:49:09.397', 0, NULL, NULL, 1, 0),
(13800656210427960, 13800656210427958, '书籍', 0, 'https://oss.memoyu.com/icons/stationery/12.png', 1, 0, '2026-10-30 17:49:27.788', 0, '2026-10-30 17:49:27.788', 0, NULL, NULL, 1, 0),
(13800656210427961, NULL, '工资', 1, 'https://oss.memoyu.com/icons/finance/25.png', 7, 0, '2026-10-30 18:05:01.229', 0, '2026-10-30 18:05:01.229', 0, NULL, NULL, 1, 0),
(13800656210427962, 13800656210427961, '奖金', 1, 'https://oss.memoyu.com/icons/finance/26.png', 0, 0, '2026-10-30 18:05:08.815', 0, '2026-10-30 18:05:08.815', 0, NULL, NULL, 1, 0),
(13800656210427963, 13800656210427961, '副业', 1, 'https://oss.memoyu.com/icons/finance/27.png', 1, 0, '2026-10-30 18:11:01.855', 0, '2026-10-30 18:11:01.855', 0, NULL, NULL, 1, 0),
(13800656210427964, NULL, '理财', 1, 'https://oss.memoyu.com/icons/finance/9.png', 8, 0, '2026-10-30 18:09:19.393', 0, '2026-10-30 18:10:39.003', 0, NULL, NULL, 1, 0),
(13800656210427965, 13800656210427964, '炒股', 1, 'https://oss.memoyu.com/icons/finance/13.png', 0, 0, '2026-10-30 18:09:39.569', 0, '2026-10-30 18:09:39.569', 0, NULL, NULL, 1, 0),
(13800656210427966, 13800656210427964, '定存', 1, 'https://oss.memoyu.com/icons/finance/5.png', 1, 0, '2026-10-30 18:09:56.925', 0, '2026-10-30 18:09:56.925', 0, NULL, NULL, 1, 0);
