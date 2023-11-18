-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: mbill
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `mbill_asset`
--

DROP TABLE IF EXISTS `mbill_asset`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_asset` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `name` varchar(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT '资产分类名',
  `parent_b_id` bigint NOT NULL COMMENT '父级BId',
  `type` int NOT NULL COMMENT '资产分类类型：存款、负债',
  `amount` decimal(12,2) NOT NULL COMMENT '资产金额',
  `icon` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '图标地址',
  `sort` int NOT NULL COMMENT '排序',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_asset_on_amount` (`amount`),
  KEY `index_asset_on_sort` (`sort`),
  KEY `index_asset_on_parent_bid` (`parent_b_id`),
  KEY `index_asset_on_type` (`type`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_asset`
--

LOCK TABLES `mbill_asset` WRITE;
/*!40000 ALTER TABLE `mbill_asset` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_asset` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_bill`
--

DROP TABLE IF EXISTS `mbill_bill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_bill` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `category_b_id` bigint NOT NULL COMMENT '分类BId',
  `asset_b_id` bigint NOT NULL COMMENT '资产BId',
  `target_asset_b_id` bigint DEFAULT NULL COMMENT '目标资产BId',
  `amount` decimal(12,2) NOT NULL COMMENT '金额',
  `asset_residue` decimal(12,2) NOT NULL COMMENT '账户余额',
  `type` int NOT NULL COMMENT '记录类型：支出、收入、转账、还款',
  `description` varchar(40) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '说明',
  `address` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '地点',
  `time` datetime(3) NOT NULL COMMENT '记录日期',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_bill_on_type` (`type`),
  KEY `index_bill_on_create_user_id_and_type` (`create_user_b_id`,`type`),
  KEY `index_bill_on_create_user_id_and_category_bid` (`create_user_b_id`,`category_b_id`),
  KEY `index_bill_on_create_user_id_and_asset_bid` (`create_user_b_id`,`asset_b_id`),
  KEY `index_bill_on_time` (`time`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_bill`
--

LOCK TABLES `mbill_bill` WRITE;
/*!40000 ALTER TABLE `mbill_bill` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_bill` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_category`
--

DROP TABLE IF EXISTS `mbill_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_category` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `name` varchar(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT '分类名称',
  `parent_b_id` bigint NOT NULL COMMENT '父级BId',
  `type` int NOT NULL COMMENT '分类类型：0 支出，1 收入',
  `budget` decimal(12,2) NOT NULL COMMENT '预算金额',
  `icon` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '图标地址',
  `sort` int NOT NULL COMMENT '排序',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_category_on_parent_bid` (`parent_b_id`),
  KEY `index_category_on_type` (`type`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_category`
--

LOCK TABLES `mbill_category` WRITE;
/*!40000 ALTER TABLE `mbill_category` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_data_seed`
--

DROP TABLE IF EXISTS `mbill_data_seed`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_data_seed` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `type` int NOT NULL COMMENT '数据类型',
  `desc` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '描述',
  `data` json DEFAULT NULL COMMENT '种子数据',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_data_seed`
--

LOCK TABLES `mbill_data_seed` WRITE;
/*!40000 ALTER TABLE `mbill_data_seed` DISABLE KEYS */;
INSERT INTO `mbill_data_seed` VALUES (1,7724264536932357,0,'账单分类种子数据','[{\"icon\": null, \"name\": \"职业收入\", \"sort\": 0, \"type\": 1, \"childs\": [{\"icon\": \"category_icons/icon_wage_64.png\", \"name\": \"工资\", \"sort\": 0, \"type\": 1, \"childs\": null}, {\"icon\": \"category_icons/icon_work_64.png\", \"name\": \"加班\", \"sort\": 1, \"type\": 1, \"childs\": null}]}, {\"icon\": null, \"name\": \"其他收入\", \"sort\": 1, \"type\": 1, \"childs\": [{\"icon\": \"category_icons/icon_investment_64.png\", \"name\": \"投资\", \"sort\": 2, \"type\": 1, \"childs\": null}, {\"icon\": \"category_icons/icon_red_envelope_64.png\", \"name\": \"红包\", \"sort\": 3, \"type\": 1, \"childs\": null}, {\"icon\": \"category_icons/icon_cube_100.png\", \"name\": \"其他\", \"sort\": 4, \"type\": 1, \"childs\": null}]}, {\"icon\": null, \"name\": \"伙食餐饮\", \"sort\": 0, \"type\": 0, \"childs\": [{\"icon\": \"category_icons/icon_pizza_64.png\", \"name\": \"一日三餐\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_mushroom_64.png\", \"name\": \"做饭食材\", \"sort\": 2, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_tea_64.png\", \"name\": \"饮料\", \"sort\": 3, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_grape_64.png\", \"name\": \"水果\", \"sort\": 4, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_confetti_64.png\", \"name\": \"聚会\", \"sort\": 5, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_snack_64.png\", \"name\": \"零食\", \"sort\": 6, \"type\": 0, \"childs\": null}]}, {\"icon\": null, \"name\": \"交通出行\", \"sort\": 1, \"type\": 0, \"childs\": [{\"icon\": \"category_icons/icon_bus_64.png\", \"name\": \"公交\", \"sort\": 0, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_subway_96.png\", \"name\": \"地铁\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_taxi_64.png\", \"name\": \"出租车\", \"sort\": 2, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_subway_64.png\", \"name\": \"高铁/动车\", \"sort\": 3, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_car_64.png\", \"name\": \"私家车\", \"sort\": 4, \"type\": 0, \"childs\": null}]}, {\"icon\": null, \"name\": \"美容护肤\", \"sort\": 2, \"type\": 0, \"childs\": [{\"icon\": \"category_icons/icon_makeup_64.png\", \"name\": \"化妆品\", \"sort\": 0, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_facial_mask_64.png\", \"name\": \"护肤品\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_lipstick_64.png\", \"name\": \"口红\", \"sort\": 2, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_nail_polish_64.png\", \"name\": \"指甲\", \"sort\": 3, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_massage_80.png\", \"name\": \"按摩\", \"sort\": 4, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_treatment_64.png\", \"name\": \"美容/养生\", \"sort\": 5, \"type\": 0, \"childs\": null}]}, {\"icon\": null, \"name\": \"医疗保障\", \"sort\": 3, \"type\": 0, \"childs\": [{\"icon\": \"category_icons/icon_pill_64.png\", \"name\": \"药品\", \"sort\": 0, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_syringe_64.png\", \"name\": \"就诊\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_insurance_64.png\", \"name\": \"保险\", \"sort\": 2, \"type\": 0, \"childs\": null}, {\"icon\": \"category_icons/icon_physical_80.png\", \"name\": \"检查\", \"sort\": 3, \"type\": 0, \"childs\": null}]}]',0,'2023-11-15 14:02:05.253',_binary '\0',NULL,NULL,NULL,'2023-11-15 14:02:05.253'),(2,7724264536932358,1,'资产分类种子数据','[{\"icon\": null, \"name\": \"负债账户\", \"sort\": 0, \"type\": 1, \"childs\": [{\"icon\": \"asset_icons/icon_huabei_64.png\", \"name\": \"蚂蚁花呗\", \"sort\": 0, \"type\": 1, \"childs\": null}, {\"icon\": \"asset_icons/icon_jd_64.png\", \"name\": \"京东白条\", \"sort\": 1, \"type\": 1, \"childs\": null}, {\"icon\": \"asset_icons/icon_credit_card_64.png\", \"name\": \"信用卡\", \"sort\": 2, \"type\": 1, \"childs\": null}]}, {\"icon\": null, \"name\": \"投资账户\", \"sort\": 1, \"type\": 1, \"childs\": [{\"icon\": \"asset_icons/icon_fund_64.png\", \"name\": \"基金账户\", \"sort\": 0, \"type\": 0, \"childs\": null}, {\"icon\": \"asset_icons/icon_stock_64.png\", \"name\": \"股票账户\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"asset_icons/icon_yuerbao_64.png\", \"name\": \"余额宝\", \"sort\": 2, \"type\": 0, \"childs\": null}]}, {\"icon\": null, \"name\": \"现金账户\", \"sort\": 0, \"type\": 0, \"childs\": [{\"icon\": \"asset_icons/icon_cash_64.png\", \"name\": \"现金\", \"sort\": 0, \"type\": 0, \"childs\": null}, {\"icon\": \"asset_icons/icon_bank_card_64.png\", \"name\": \"银行卡\", \"sort\": 1, \"type\": 0, \"childs\": null}]}, {\"icon\": null, \"name\": \"虚拟账户\", \"sort\": 1, \"type\": 0, \"childs\": [{\"icon\": \"asset_icons/icon_alipay_64.png\", \"name\": \"支付宝\", \"sort\": 1, \"type\": 0, \"childs\": null}, {\"icon\": \"asset_icons/icon_wechat_64.png\", \"name\": \"微信钱包\", \"sort\": 0, \"type\": 0, \"childs\": null}]}]',0,'2023-11-15 14:02:05.253',_binary '\0',NULL,NULL,NULL,'2023-11-15 14:02:05.253');
/*!40000 ALTER TABLE `mbill_data_seed` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_file`
--

DROP TABLE IF EXISTS `mbill_file`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_file` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `extension` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '后缀',
  `md5` varchar(40) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '图片md5值，防止上传重复图片',
  `name` varchar(300) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '名称',
  `path` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '路径',
  `size` bigint DEFAULT NULL COMMENT '大小',
  `type` smallint DEFAULT NULL COMMENT '1 七牛',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_file`
--

LOCK TABLES `mbill_file` WRITE;
/*!40000 ALTER TABLE `mbill_file` DISABLE KEYS */;
INSERT INTO `mbill_file` VALUES (1,7724329208840197,'.png','e83a407fd5f43426eab67d46df13cb8c','icon_alipay_64.png','asset_icons/icon_alipay_64.png',3847,1,0,'2022-07-16 16:09:01.083',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.083'),(2,7724329209954309,'.png','d8dfb5c48da6b38f3311a6fb001c28cb','icon_bank_card_64.png','asset_icons/icon_bank_card_64.png',390,1,0,'2022-07-16 16:09:01.088',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.088'),(3,7724329211002885,'.png','2de6bf3fee1934d5125c55316e585bef','icon_cash_64.png','asset_icons/icon_cash_64.png',4262,1,0,'2022-07-16 16:09:01.092',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.092'),(4,7724329211854853,'.png','b7e6323cc5a48c181a27f0d550397314','icon_credit_card_64.png','asset_icons/icon_credit_card_64.png',403,1,0,'2022-07-16 16:09:01.095',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.095'),(5,7724329212706821,'.png','f9e7ff14c5036fc6c10e42f689a7b1a7','icon_fund_64.png','asset_icons/icon_fund_64.png',1703,1,0,'2022-07-16 16:09:01.098',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.098'),(6,7724329213558789,'.png','0714baae8f6fb92cdfa17f4c3927cfcc','icon_huabei_64.png','asset_icons/icon_huabei_64.png',4696,1,0,'2022-07-16 16:09:01.101',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.101'),(7,7724329214672901,'.png','fa32f05e593d52c4bc8c733749debaad','icon_jd_64.png','asset_icons/icon_jd_64.png',3218,1,0,'2022-07-16 16:09:01.104',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.104'),(8,7724329216049157,'.png','2315f1090f3c04679a2c1f8c7901be27','icon_stock_64.png','asset_icons/icon_stock_64.png',1779,1,0,'2022-07-16 16:09:01.108',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.108'),(9,7724329217949701,'.png','3322f9de61a8bceb81b1d47520ddac21','icon_wechat_64.png','asset_icons/icon_wechat_64.png',3899,1,0,'2022-07-16 16:09:01.111',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.111'),(10,7724329220046853,'.png','ac45e9262ce0297ce6c3d7a754a90800','icon_yuerbao_64.png','asset_icons/icon_yuerbao_64.png',2505,1,0,'2022-07-16 16:09:01.114',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.114'),(11,7724329222995973,'.png','e1cdc7ea51fc03db242482ca6da487a7','icon_bill2_64.png','category_icons/icon_bill2_64.png',3554,1,0,'2022-07-16 16:09:01.118',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.118'),(12,7724329225945093,'.png','4720a4cd74a1b9af429f272e2884d1fa','icon_bill_64.png','category_icons/icon_bill_64.png',2727,1,0,'2022-07-16 16:09:01.121',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.121'),(13,7724329228959749,'.png','cbee6ab6132c78dcda7e8350e4872df0','icon_book_64.png','category_icons/icon_book_64.png',1597,1,0,'2022-07-16 16:09:01.124',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.124'),(14,7724329230925829,'.png','2355270b8f229e6c408b13c8ac2b600a','icon_browser_64.png','category_icons/icon_browser_64.png',2130,1,0,'2022-07-16 16:09:01.127',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.127'),(15,7724329232826373,'.png','c1fcdb145aa688ff5f2e9a1c1ab6d9ac','icon_bus_64.png','category_icons/icon_bus_64.png',4145,1,0,'2022-07-16 16:09:01.130',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.130'),(16,7724329234530309,'.png','b39b3210a9a9895e7f1bcda134222a7c','icon_cap_64.png','category_icons/icon_cap_64.png',1791,1,0,'2022-07-16 16:09:01.160',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.160'),(17,7724329235644421,'.png','301f1f05a29e7e396f175944107f9c98','icon_car_64.png','category_icons/icon_car_64.png',2770,1,0,'2022-07-16 16:09:01.163',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.163'),(18,7724329236627461,'.png','92299ddffc4ee2ee6486698877cebc04','icon_certificate_96.png','category_icons/icon_certificate_96.png',1442,1,0,'2022-07-16 16:09:01.167',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.167'),(19,7724329237413893,'.png','95dea0f89898d776d6c8ffb949a556cc','icon_champagne_Bottle_64.png','category_icons/icon_champagne_Bottle_64.png',2393,1,0,'2022-07-16 16:09:01.170',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.170'),(20,7724329238396933,'.png','5c33ee3e73c72403006e9c9f53c54d51','icon_confetti_64.png','category_icons/icon_confetti_64.png',2685,1,0,'2022-07-16 16:09:01.174',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.174'),(21,7724329239445509,'.png','4d4e03683a1e5575d0782ae4d7f4386c','icon_cook_64.png','category_icons/icon_cook_64.png',1673,1,0,'2022-07-16 16:09:01.178',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.178'),(22,7724329240625157,'.png','41132cbd7417351df2aecfb462f9a8e6','icon_cube_100.png','category_icons/icon_cube_100.png',3227,1,0,'2022-07-16 16:09:01.181',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.181'),(23,7724329241477125,'.png','2268778eb3bccb5ccd3d08b5e3745383','icon_cycling_64.png','category_icons/icon_cycling_64.png',4865,1,0,'2022-07-16 16:09:01.184',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.184'),(24,7724329242329093,'.png','8dd3c6de6acbe5826b698b5348dc5bea','icon_earring_64.png','category_icons/icon_earring_64.png',3566,1,0,'2022-07-16 16:09:01.188',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.188'),(25,7724329243181061,'.png','a56910569b20e16d18bd43c3a06ec159','icon_egg_64.png','category_icons/icon_egg_64.png',2913,1,0,'2022-07-16 16:09:01.191',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.191'),(26,7724329244098565,'.png','f06e1a87fcfdd66ead9e3dcd55a723f0','icon_facial_mask_64.png','category_icons/icon_facial_mask_64.png',2756,1,0,'2022-07-16 16:09:01.194',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.194'),(27,7724329244884997,'.png','ca07127f097475016db7e6c02e0febd3','icon_game_64.png','category_icons/icon_game_64.png',1858,1,0,'2022-07-16 16:09:01.197',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.197'),(28,7724329245802501,'.png','52ffe1564707c3f93360da7cf0b2bf99','icon_gift_65.png','category_icons/icon_gift_65.png',2545,1,0,'2022-07-16 16:09:01.200',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.200'),(29,7724329246654469,'.png','480d552be9b6c34438a12c775493663b','icon_grape_64.png','category_icons/icon_grape_64.png',2840,1,0,'2022-07-16 16:09:01.203',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.203'),(30,7724329247703045,'.png','3ec6ac4d193abefb092f2d5e5f639b59','icon_house_64.png','category_icons/icon_house_64.png',947,1,0,'2022-07-16 16:09:01.207',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.208'),(31,7724329248620549,'.png','6e06540b4f4b6a4812d125a9b62409b6','icon_imac_64.png','category_icons/icon_imac_64.png',1414,1,0,'2022-07-16 16:09:01.211',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.211'),(32,7724329249538053,'.png','76b790f7a8647731c457748e0c98138e','icon_insurance_64.png','category_icons/icon_insurance_64.png',3671,1,0,'2022-07-16 16:09:01.214',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.214'),(33,7724329250455557,'.png','d724e774c25d535d89361ec2d19f1869','icon_investment_64.png','category_icons/icon_investment_64.png',1200,1,0,'2022-07-16 16:09:01.218',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.218'),(34,7724329251307525,'.png','30727348906d3cb38eb576cc8da4d35c','icon_lipstick_64.png','category_icons/icon_lipstick_64.png',1459,1,0,'2022-07-16 16:09:01.221',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.221'),(35,7724329252290565,'.png','2430bf5eba6e73d4eb9a8b7fc2b7df11','icon_makeup_64.png','category_icons/icon_makeup_64.png',3120,1,0,'2022-07-16 16:09:01.224',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.224'),(36,7724329253208069,'.png','022102829cdcc798396ec1cb95535147','icon_massage_80.png','category_icons/icon_massage_80.png',2873,1,0,'2022-07-16 16:09:01.227',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.227'),(37,7724329254060037,'.png','9f6a534c46b8f6e2dcb65fbf901b0f9c','icon_mortgage_128.png','category_icons/icon_mortgage_128.png',5424,1,0,'2022-07-16 16:09:01.230',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.230'),(38,7724329254912005,'.png','1846ec83faef6d32683b0094c0552429','icon_movies_64.png','category_icons/icon_movies_64.png',1252,1,0,'2022-07-16 16:09:01.233',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.233'),(39,7724329255763973,'.png','a297bc9401636bda1c35c2c187f02259','icon_mushroom_64.png','category_icons/icon_mushroom_64.png',2153,1,0,'2022-07-16 16:09:01.237',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.237'),(40,7724329256812549,'.png','236b5892d8362c2cba2874c80d440b2e','icon_nail_polish_64.png','category_icons/icon_nail_polish_64.png',2285,1,0,'2022-07-16 16:09:01.240',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.240'),(41,7724329257664517,'.png','3aadea1e0c5f47217a2619dcce6a2e84','icon_perfume_64.png','category_icons/icon_perfume_64.png',1961,1,0,'2022-07-16 16:09:01.245',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.245'),(42,7724329258647557,'.png','7c77e1309ce71434239a8450e2bcc618','icon_physical_80.png','category_icons/icon_physical_80.png',3337,1,0,'2022-07-16 16:09:01.249',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.249'),(43,7724329259565061,'.png','440a51a10019bac797a837fd97d743da','icon_pill_64.png','category_icons/icon_pill_64.png',2416,1,0,'2022-07-16 16:09:01.253',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.253'),(44,7724329260417029,'.png','05fa513299df62119adfa4e74b9cae7f','icon_pizza_64.png','category_icons/icon_pizza_64.png',3918,1,0,'2022-07-16 16:09:01.256',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.256'),(45,7724329261334533,'.png','722e31921969af55ce681d522b07af46','icon_purse_64.png','category_icons/icon_purse_64.png',2222,1,0,'2022-07-16 16:09:01.259',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.259'),(46,7724329262383109,'.png','9e39c3b8b31b5d4b117d02642650c033','icon_read_64.png','category_icons/icon_read_64.png',3988,1,0,'2022-07-16 16:09:01.262',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.262'),(47,7724329263693829,'.png','8676128878c611dd57b0498e10be1db3','icon_red_envelope_64.png','category_icons/icon_red_envelope_64.png',2182,1,0,'2022-07-16 16:09:01.266',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.266'),(48,7724329265856517,'.png','7972df824e080e6d24fab3b7b063e0dc','icon_repayment_64.png','category_icons/icon_repayment_64.png',4619,1,0,'2022-07-16 16:09:01.270',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.270'),(49,7724329267757061,'.png','c0524562569a0e4fd7a1762a5add6f95','icon_shoes_64.png','category_icons/icon_shoes_64.png',1610,1,0,'2022-07-16 16:09:01.273',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.273'),(50,7724329269854213,'.png','6ddf71237ba16fcf9f4241d811142729','icon_shorts_64.png','category_icons/icon_shorts_64.png',2071,1,0,'2022-07-16 16:09:01.276',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.276'),(51,7724329272147973,'.png','2b3c739f540569beeb6640012dd3da00','icon_snack_64.png','category_icons/icon_snack_64.png',2802,1,0,'2022-07-16 16:09:01.280',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.280'),(52,7724329275359237,'.png','d1dba7faa0a75a944256dd77ef36e87e','icon_subway_64.png','category_icons/icon_subway_64.png',2334,1,0,'2022-07-16 16:09:01.285',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.285'),(53,7724329279684613,'.png','dfedbaec062ca949d5fc786b4e55a70f','icon_subway_96.png','category_icons/icon_subway_96.png',2981,1,0,'2022-07-16 16:09:01.288',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.288'),(54,7724329283354629,'.png','5d89fb9acd8dc241ac232ffad07172c4','icon_syringe_64.png','category_icons/icon_syringe_64.png',1950,1,0,'2022-07-16 16:09:01.292',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.292'),(55,7724329286434821,'.png','3ad61c7534ed1ea737ff0c85cb982491','icon_taxi_64.png','category_icons/icon_taxi_64.png',3339,1,0,'2022-07-16 16:09:01.295',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.295'),(56,7724329290366981,'.png','f82b689fed6e4a820e4316f2e3194f67','icon_tea_64.png','category_icons/icon_tea_64.png',1449,1,0,'2022-07-16 16:09:01.298',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.298'),(57,7724329292988421,'.png','5e5158148caee193e37b09b11c7a2e75','icon_tissue_64.png','category_icons/icon_tissue_64.png',2765,1,0,'2022-07-16 16:09:01.302',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.302'),(58,7724329296461829,'.png','12d8292abb424b84d7c3182f6e939d95','icon_transfer_64.png','category_icons/icon_transfer_64.png',4129,1,0,'2022-07-16 16:09:01.305',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.305'),(59,7724329299542021,'.png','3463eac6598574fa061dd59b3916c4d5','icon_treatment_64.png','category_icons/icon_treatment_64.png',3300,1,0,'2022-07-16 16:09:01.309',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.309'),(60,7724329302425605,'.png','6f2846c2783e879a7e9ad76b2d5d5201','icon_t_shirt_64.png','category_icons/icon_t_shirt_64.png',1557,1,0,'2022-07-16 16:09:01.312',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.312'),(61,7724329305833477,'.png','37ce173387c3f62cfa9b3a840092591c','icon_wage_64.png','category_icons/icon_wage_64.png',4157,1,0,'2022-07-16 16:09:01.316',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.316'),(62,7724329308782597,'.png','471278a479709520e1aaa3939de7e89f','icon_work_64.png','category_icons/icon_work_64.png',7444,1,0,'2022-07-16 16:09:01.318',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.318'),(63,7724329308782598,'.png','f28dd1de8611fe6b7abcb1c30bb20ef3','default_avatar.png','avatar/default_avatar.png',8903,1,0,'2022-07-16 16:09:01.318',_binary '\0',0,NULL,0,'2022-07-16 16:09:01.318');
/*!40000 ALTER TABLE `mbill_file` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_ip_log`
--

DROP TABLE IF EXISTS `mbill_ip_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_ip_log` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `ip` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '客户端IP',
  `path` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '访问路径',
  `visit_time` datetime(3) NOT NULL COMMENT '访问时间',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_ip_log`
--

LOCK TABLES `mbill_ip_log` WRITE;
/*!40000 ALTER TABLE `mbill_ip_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_ip_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_item`
--

DROP TABLE IF EXISTS `mbill_item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_item` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `base_type_b_id` bigint NOT NULL COMMENT '字典项所属TypeBId',
  `item_code` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '字典项编码',
  `item_name` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '字典项名称',
  `status` bit(1) NOT NULL COMMENT '状态：0禁用，1启用',
  `sort` int DEFAULT NULL COMMENT '排序码',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_item`
--

LOCK TABLES `mbill_item` WRITE;
/*!40000 ALTER TABLE `mbill_item` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_media_image`
--

DROP TABLE IF EXISTS `mbill_media_image`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_media_image` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `file_b_id` bigint NOT NULL COMMENT '文件BId',
  `type` int NOT NULL COMMENT '图片类型：0 Icon, 1 background',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_media_image_on_file_bid` (`file_b_id`),
  KEY `index_media_image_on_type` (`type`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=63 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_media_image`
--

LOCK TABLES `mbill_media_image` WRITE;
/*!40000 ALTER TABLE `mbill_media_image` DISABLE KEYS */;
INSERT INTO `mbill_media_image` VALUES (1,7724333801078789,7724329208840197,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(2,7724333802258437,7724329209954309,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(3,7724333803307013,7724329211002885,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(4,7724333804158981,7724329211854853,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(5,7724333805010949,7724329212706821,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(6,7724333806190597,7724329213558789,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(7,7724333807435781,7724329214672901,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(8,7724333808287749,7724329216049157,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(9,7724333809074181,7724329217949701,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(10,7724333810450437,7724329220046853,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(11,7724333811564549,7724329222995973,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(12,7724333812482053,7724329225945093,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(13,7724333813399557,7724329228959749,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(14,7724333814317061,7724329230925829,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(15,7724333815234565,7724329232826373,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(16,7724333816086533,7724329234530309,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(17,7724333817004037,7724329235644421,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(18,7724333817856005,7724329236627461,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(19,7724333818839045,7724329237413893,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(20,7724333819822085,7724329238396933,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(21,7724333820608517,7724329239445509,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(22,7724333821984773,7724329240625157,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(23,7724333823426565,7724329241477125,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(24,7724333824606213,7724329242329093,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(25,7724333825982469,7724329243181061,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(26,7724333826965509,7724329244098565,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(27,7724333827817477,7724329244884997,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(28,7724333828997125,7724329245802501,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(29,7724333830897669,7724329246654469,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(30,7724333832798213,7724329247703045,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(31,7724333835026437,7724329248620549,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(32,7724333837189125,7724329249538053,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(33,7724333839941637,7724329250455557,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(34,7724333842366469,7724329251307525,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(35,7724333844791301,7724329252290565,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(36,7724333847871493,7724329253208069,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(37,7724333851672581,7724329254060037,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(38,7724333853704197,7724329254912005,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(39,7724333855932421,7724329255763973,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(40,7724333857112069,7724329256812549,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(41,7724333857964037,7724329257664517,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(42,7724333858750469,7724329258647557,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(43,7724333859667973,7724329259565061,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(44,7724333860519941,7724329260417029,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(45,7724333861306373,7724329261334533,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(46,7724333862223877,7724329262383109,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(47,7724333863141381,7724329263693829,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(48,7724333864058885,7724329265856517,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(49,7724333864910853,7724329267757061,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(50,7724333865828357,7724329269854213,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(51,7724333866745861,7724329272147973,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(52,7724333867532293,7724329275359237,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(53,7724333868843013,7724329279684613,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(54,7724333869826053,7724329283354629,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(55,7724333870743557,7724329286434821,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(56,7724333871726597,7724329290366981,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(57,7724333872971781,7724329292988421,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(58,7724333873889285,7724329296461829,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(59,7724333874675717,7724329299542021,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(60,7724333875593221,7724329302425605,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(61,7724333876379653,7724329305833477,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL),(62,7724333877231621,7724329308782597,0,0,'2023-09-01 00:46:16.000',_binary '\0',0,NULL,0,NULL);
/*!40000 ALTER TABLE `mbill_media_image` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_notice`
--

DROP TABLE IF EXISTS `mbill_notice`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_notice` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `content` varchar(500) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '内容',
  `range` json DEFAULT NULL COMMENT '可见范围',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_notice`
--

LOCK TABLES `mbill_notice` WRITE;
/*!40000 ALTER TABLE `mbill_notice` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_notice` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_permission`
--

DROP TABLE IF EXISTS `mbill_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_permission` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `name` varchar(60) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '所属权限、权限名称，例如：访问首页',
  `module` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '权限所属模块，例如：人员管理',
  `router` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '后台路由',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=69 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_permission`
--

LOCK TABLES `mbill_permission` WRITE;
/*!40000 ALTER TABLE `mbill_permission` DISABLE KEYS */;
INSERT INTO `mbill_permission` VALUES (1,7742270790631429,'新增','预购','api/pre-order POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(2,7742270790631430,'详情','预购','api/pre-order GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(3,7742270790631431,'删除','预购','api/pre-order DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(4,7742270790631432,'更新','预购','api/pre-order PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(5,7742270790631433,'更新状态','预购','api/pre-order/status PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(6,7742270790631434,'获取指定分组分页预购','预购','api/pre-order/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(7,7742270790631435,'获取预购清单首页统计','预购','api/pre-order/index/stat GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(8,7742270790631436,'新增','预购分组','api/pre-order/group POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(9,7742270790631437,'详情','预购分组','api/pre-order/group GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(10,7742270790631438,'详情带预购分组总金额','预购分组','api/pre-order/group/amount GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(11,7742270790631439,'分组转入账单','预购分组','api/pre-order/group/to-bill POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(12,7742270790631440,'删除','预购分组','api/pre-order/group DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(13,7742270790631441,'更新','预购分组','api/pre-order/group PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(14,7742270790631442,'获取指定月份分页预购分组','预购分组','api/pre-order/group/month/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(15,7742270790631443,'获取指定分组预购清单统计','预购分组','api/pre-order/group/order/stat GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(16,7742270790631444,'新增','资产分类','api/asset POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(17,7742270790631445,'删除','资产分类','api/asset DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(18,7742270790631446,'更新','资产分类','api/asset PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(19,7742270790631447,'获取详情','资产分类','api/asset GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(20,7742270790631448,'获取父项详情','资产分类','api/asset/parent GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(21,7742270790631449,'获取父项集合','资产分类','api/asset/parents GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(22,7742270790631450,'获取分组','资产分类','api/asset/groups GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(23,7742270790631451,'获取分页','管理员','api/asset/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(24,7742270790631452,'分类排序','资产分类','api/asset/sort POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(25,7742270790631453,'新增','账单','api/bill POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(26,7742270790631454,'详情','账单','api/bill GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(27,7742270790631455,'删除','账单','api/bill DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(28,7742270790631456,'更新','账单','api/bill PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(29,7742270790631457,'检索指定条件账单','账单','api/bill/search POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(30,7742270790631458,'获取账单检索记录','账单','api/bill/search/records GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(31,7742270790631459,'获取指定条件分页账单','账单','api/bill/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(32,7742270790631460,'获取指定月份分组分页账单','账单','api/bill/month/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(33,7742270790631461,'获取指定日期账单','账单','api/bill/day GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(34,7742270790631462,'获取日期范围内存在账单的日期','账单','api/bill/date/has-bill-days GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(35,7742270790631463,'获取指定月份账单总金额','账单','api/bill/stat/month-total GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(36,7742270790631464,'获取指定年份账单总金额','账单','api/bill/stat/year-total GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(37,7742270790631465,'获取指定年份收支结余统计','账单','api/bill/stat/year-surplus GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(38,7742270790631466,'获取指定月份账单金额趋势','账单','api/bill/stat/trend/month-total GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(39,7742270790631467,'获取指定年份账单金额趋势','账单','api/bill/stat/trend/year-total GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(40,7742270790631468,'获取日期内分类占比','账单','api/bill/stat/percent/category GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(41,7742270790631469,'获取日期内分类占比分组','账单','api/bill/stat/percent/category/group GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(42,7742270790631470,'获取账单排行列表','账单','api/bill/stat/ranking GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(43,7742270790631471,'新增','账单分类','api/category POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(44,7742270790631472,'删除','账单分类','api/category DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(45,7742270790631473,'更新','账单分类','api/category PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(46,7742270790631474,'获取详情','账单分类','api/category GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(47,7742270790631475,'获取全部','账单分类','api/category/list GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(48,7742270790631476,'获取父项详情','账单分类','api/category/parent GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(49,7742270790631477,'获取父项集合','账单分类','api/category/parents GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(50,7742270790631478,'获取分组','账单分类','api/category/groups GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(51,7742270790631479,'获取分页','管理员','api/category/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(52,7742270790631480,'分类排序','账单分类','api/category/sort POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(53,7742270790631481,'获取用户信息','用户','api/account/user GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(54,7742270790631482,'更新用户信息','用户','api/account PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(55,7742270790631483,'上传单个文件','文件管理','api/file/upload POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(56,7742270790631484,'获取分页媒体图片','文件管理','api/file/media-image/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(57,7742270790631485,'新增公告','管理员','api/notice POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(58,7742270790631486,'获取最新公告','公告','api/notice/latest GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(59,7742270790631487,'查询权限信息（树形结构）','管理员','api/admin/permission/tree GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(60,7742270790631488,'查询所有可分配的权限','管理员','api/admin/permission/module GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(61,7742270790631489,'配置角色权限','管理员','api/admin/permission/dispatch POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(62,7742270790631490,'新增角色','管理员','api/admin/role POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(63,7742270790631491,'删除角色','管理员','api/admin/role DELETE',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(64,7742270790631492,'更新角色','管理员','api/admin/role PUT',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(65,7742270790631493,'获取全部角色','管理员','api/admin/role/all GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(66,7742270790631494,'角色详情','管理员','api/admin/role/{bId} GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(67,7742270790631495,'新增用户','管理员','api/admin/user POST',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879'),(68,7742270790631496,'获取用户分页数据','管理员','api/admin/user/pages GET',0,'2023-11-18 18:21:18.879',_binary '\0',NULL,NULL,NULL,'2023-11-18 18:21:18.879');
/*!40000 ALTER TABLE `mbill_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_pre_order`
--

DROP TABLE IF EXISTS `mbill_pre_order`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_pre_order` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `group_b_id` bigint NOT NULL COMMENT '分组BId',
  `pre_amount` decimal(12,2) NOT NULL COMMENT '预购金额',
  `real_amount` decimal(12,2) NOT NULL COMMENT '实际金额',
  `description` varchar(40) COLLATE utf8mb4_general_ci NOT NULL COMMENT '说明',
  `time` datetime(3) NOT NULL COMMENT '记录日期',
  `status` int NOT NULL COMMENT '状态 0:未购买；1：已购买',
  `color` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '图标颜色',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_preorder_on_group_bid` (`group_b_id`),
  KEY `index_preorder_on_status` (`status`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_pre_order`
--

LOCK TABLES `mbill_pre_order` WRITE;
/*!40000 ALTER TABLE `mbill_pre_order` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_pre_order` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_pre_order_group`
--

DROP TABLE IF EXISTS `mbill_pre_order_group`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_pre_order_group` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `bill_b_id` bigint NOT NULL COMMENT '账单BId',
  `name` varchar(10) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '分组名',
  `description` varchar(40) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '说明',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_preorder_group_on_bill_bid` (`bill_b_id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_pre_order_group`
--

LOCK TABLES `mbill_pre_order_group` WRITE;
/*!40000 ALTER TABLE `mbill_pre_order_group` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_pre_order_group` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_role`
--

DROP TABLE IF EXISTS `mbill_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_role` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `name` varchar(60) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '角色唯一标识字符',
  `type` int NOT NULL COMMENT '角色类型',
  `info` varchar(100) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '角色描述',
  `is_static` bit(1) NOT NULL COMMENT '是否是静态分组,是静态时无法删除此分组',
  `sort` int NOT NULL COMMENT '排序码，升序',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_role`
--

LOCK TABLES `mbill_role` WRITE;
/*!40000 ALTER TABLE `mbill_role` DISABLE KEYS */;
INSERT INTO `mbill_role` VALUES (1,7724334002733061,'Administrator',1,'超级管理员',_binary '',0,0,'2021-01-14 23:46:04.980',_binary '\0',0,NULL,0,NULL),(2,7724334002733062,'Admin',2,'普通管理员',_binary '',0,0,'2021-01-14 23:46:04.980',_binary '\0',0,NULL,0,NULL),(3,7724334002733063,'User',0,'普通用户',_binary '',0,0,'2021-01-14 23:46:04.980',_binary '\0',0,NULL,0,NULL);
/*!40000 ALTER TABLE `mbill_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_role_permission`
--

DROP TABLE IF EXISTS `mbill_role_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_role_permission` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `role_b_id` bigint NOT NULL COMMENT '角色BId',
  `permission_b_id` bigint NOT NULL COMMENT '权限BId',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_role_permission`
--

LOCK TABLES `mbill_role_permission` WRITE;
/*!40000 ALTER TABLE `mbill_role_permission` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_role_permission` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_type`
--

DROP TABLE IF EXISTS `mbill_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_type` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `type_code` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '字典类型编码',
  `full_name` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '字典类型名',
  `sort` int DEFAULT NULL COMMENT '排序码',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_type`
--

LOCK TABLES `mbill_type` WRITE;
/*!40000 ALTER TABLE `mbill_type` DISABLE KEYS */;
/*!40000 ALTER TABLE `mbill_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_user`
--

DROP TABLE IF EXISTS `mbill_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_user` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `username` varchar(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT '用户名',
  `nickname` varchar(20) COLLATE utf8mb4_general_ci NOT NULL COMMENT '昵称',
  `gender` int NOT NULL COMMENT '性别，0：未知，1：男，2：女',
  `email` varchar(60) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '邮箱',
  `phone` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '电话',
  `province` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '省',
  `city` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '市',
  `district` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '区',
  `street` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '街道',
  `avatar_url` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '头像地址',
  `last_login_time` datetime(3) NOT NULL COMMENT '最后一次登录的时间',
  `refresh_token` varchar(200) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT 'JWT 登录，保存生成的随机token值。',
  `is_enable` bit(1) NOT NULL COMMENT '是否启用',
  `is_init` bit(1) NOT NULL COMMENT '是否初始化用户数据',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_user`
--

LOCK TABLES `mbill_user` WRITE;
/*!40000 ALTER TABLE `mbill_user` DISABLE KEYS */;
INSERT INTO `mbill_user` VALUES (1,7724334252621829,'administrator','超级管理员',0,'','','','','','','avatar/default_avatar.png','2023-11-15 15:22:22.307','',_binary '',_binary '',0,'2021-01-14 23:46:04.975',_binary '\0',0,NULL,NULL,'2023-11-15 15:22:22.307'),(2,7724334252621830,'admin','管理员',0,'','','','','','','avatar/default_avatar.png','2022-02-24 08:44:01.013','',_binary '',_binary '',0,'2021-01-14 23:46:04.976',_binary '\0',0,NULL,0,'2022-02-24 08:44:01.013'),(3,7724334252621831,'user','普通用户',1,'','','','','','','avatar/default_avatar.png','2023-11-18 08:33:59.706','',_binary '',_binary '',0,'2021-01-14 23:47:51.047',_binary '\0',0,NULL,NULL,'2023-11-18 08:33:59.706');
/*!40000 ALTER TABLE `mbill_user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_user_identity`
--

DROP TABLE IF EXISTS `mbill_user_identity`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_user_identity` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `user_b_id` bigint NOT NULL COMMENT '用户Id',
  `identity_type` varchar(20) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '认证类型， Password，GitHub、QQ、WeiXin等',
  `identifier` varchar(24) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '认证者，例如 用户名,手机号，邮件等，',
  `credential` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '凭证，例如 密码,存OpenId、Id，同一IdentityType的OpenId的值是唯一的',
  `extra_properties` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL COMMENT '扩展属性',
  `create_user_b_id` bigint NOT NULL COMMENT '创建人BId',
  `create_time` datetime(3) NOT NULL COMMENT '创建时间',
  `is_deleted` bit(1) NOT NULL COMMENT '是否删除 0 未删除，1 已删除',
  `delete_user_b_id` bigint DEFAULT NULL COMMENT '删除人BId',
  `delete_time` datetime(3) DEFAULT NULL COMMENT '删除时间',
  `update_user_b_id` bigint DEFAULT NULL COMMENT '修改人BId',
  `update_time` datetime(3) DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`),
  KEY `index_user_identity_on_user_bid` (`user_b_id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_user_identity`
--

LOCK TABLES `mbill_user_identity` WRITE;
/*!40000 ALTER TABLE `mbill_user_identity` DISABLE KEYS */;
INSERT INTO `mbill_user_identity` VALUES (1,7724334392737797,7724334252621829,'Password','administrator','0CE1FE485411E6D38A08DCA6FA551226',NULL,0,'2021-01-14 23:46:04.975',_binary '\0',0,NULL,0,NULL),(2,7724334392737798,7724334252621830,'Password','admin','0CE1FE485411E6D38A08DCA6FA551226',NULL,0,'2021-01-14 23:46:04.976',_binary '\0',0,NULL,0,NULL),(3,7724334392737799,7724334252621831,'Password','user','0CE1FE485411E6D38A08DCA6FA551226',NULL,0,'2021-01-14 23:47:51.047',_binary '\0',0,NULL,0,NULL);
/*!40000 ALTER TABLE `mbill_user_identity` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mbill_user_role`
--

DROP TABLE IF EXISTS `mbill_user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mbill_user_role` (
  `id` bigint NOT NULL AUTO_INCREMENT,
  `b_id` bigint NOT NULL,
  `user_b_id` bigint NOT NULL COMMENT '用户BId',
  `role_b_id` bigint NOT NULL COMMENT '角色BId',
  PRIMARY KEY (`id`),
  KEY `index_user_role_on_user_bid` (`user_b_id`),
  KEY `index_on_bid` (`b_id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mbill_user_role`
--

LOCK TABLES `mbill_user_role` WRITE;
/*!40000 ALTER TABLE `mbill_user_role` DISABLE KEYS */;
INSERT INTO `mbill_user_role` VALUES (1,7724334516862981,7724334252621829,7724334002733061),(2,7724334516862982,7724334252621830,7724334002733062),(3,7724334516862983,7724334252621831,7724334002733063);
/*!40000 ALTER TABLE `mbill_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'mbill'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-11-18 18:25:14
