-- MySQL dump 10.13  Distrib 5.7.23, for Win64 (x86_64)
--
-- Host: localhost    Database: db_layui_simple
-- ------------------------------------------------------
-- Server version	5.7.23

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `sw_good_type`
--

DROP TABLE IF EXISTS `sw_good_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sw_good_type` (
  `ID` decimal(28,0) NOT NULL,
  `Title` varchar(100) DEFAULT NULL,
  `SubTitle` varchar(100) DEFAULT NULL,
  `KeyWord` varchar(100) DEFAULT NULL,
  `Remarks` longtext,
  `Sort` int(11) DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sw_good_type`
--

LOCK TABLES `sw_good_type` WRITE;
/*!40000 ALTER TABLE `sw_good_type` DISABLE KEYS */;
INSERT INTO `sw_good_type` VALUES (2018071911696975951892068518,'新闻资讯',NULL,'100001',NULL,0);
/*!40000 ALTER TABLE `sw_good_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sw_goods`
--

DROP TABLE IF EXISTS `sw_goods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sw_goods` (
  `ID` decimal(28,0) NOT NULL,
  `Title` varchar(100) DEFAULT NULL COMMENT '文章标题',
  `TypeName` varchar(100) DEFAULT NULL COMMENT '类别名称',
  `TypeID` decimal(28,0) DEFAULT NULL COMMENT '类别ID',
  `GoodsKeys` varchar(100) DEFAULT NULL COMMENT '关键字',
  `Sort` int(5) DEFAULT '0' COMMENT '权重',
  `Content` longtext COMMENT '文章内容',
  `IsTop` bit(1) DEFAULT b'0' COMMENT '是否置顶',
  `IsHot` bit(1) DEFAULT b'0' COMMENT '是否热门',
  `CreateTime` datetime DEFAULT NULL,
  `Status` int(5) DEFAULT NULL COMMENT '1:未发布，2：已发布，3：已下架，4：已删除',
  `Hits` int(11) DEFAULT NULL,
  `TopSort` int(11) DEFAULT NULL COMMENT '置顶权重',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;



--
-- Table structure for table `sys_log`
--

DROP TABLE IF EXISTS `sys_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_log` (
  `ID` decimal(28,0) NOT NULL COMMENT '自动递增',
  `UserID` decimal(28,0) DEFAULT NULL COMMENT '操作人ID',
  `UserName` varchar(50) DEFAULT NULL COMMENT '操作人',
  `Title` varchar(100) DEFAULT NULL COMMENT '日志内容',
  `IP` varchar(50) DEFAULT NULL COMMENT 'IP地址',
  `LogType` int(11) NOT NULL DEFAULT '0' COMMENT '日志类型',
  `LogGrade` varchar(255) NOT NULL COMMENT '日志级别',
  `RequestUrl` varchar(255) DEFAULT NULL COMMENT '请求地址',
  `AddDate` datetime DEFAULT NULL COMMENT '添加时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `sys_message`
--

DROP TABLE IF EXISTS `sys_message`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_message` (
  `ID` decimal(28,0) NOT NULL,
  `SenderID` decimal(28,0) DEFAULT NULL COMMENT '发送人',
  `RecipientID` decimal(28,0) DEFAULT NULL COMMENT '接收人',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `Title` varchar(500) DEFAULT NULL COMMENT '标题',
  `Content` longtext COMMENT '消息内容',
  `ParentID` decimal(28,0) DEFAULT NULL COMMENT '如果回复，要回复的消息ID',
  `FirstID` decimal(28,0) DEFAULT NULL COMMENT '如果回复，则为第一条的ID',
  `Type` int(11) DEFAULT NULL COMMENT '消息类型，1：通知，2：私信',
  `Status` int(11) DEFAULT NULL COMMENT '0：未读，1：已读，',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_message`
--

LOCK TABLES `sys_message` WRITE;
/*!40000 ALTER TABLE `sys_message` DISABLE KEYS */;
INSERT INTO `sys_message` VALUES (1,NULL,NULL,'2018-07-12 14:51:22','test','test',NULL,NULL,1,0);
/*!40000 ALTER TABLE `sys_message` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_module`
--

DROP TABLE IF EXISTS `sys_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_module` (
  `ID` decimal(28,0) NOT NULL COMMENT '菜单id',
  `Name` varchar(100) DEFAULT NULL COMMENT '菜单名称',
  `Icon` varchar(30) DEFAULT NULL COMMENT '菜单图标',
  `Href` varchar(255) DEFAULT NULL COMMENT '菜单地址',
  `ParentID` decimal(28,0) DEFAULT NULL COMMENT '父级菜单Id',
  `Sort` int(11) DEFAULT NULL COMMENT '权重',
  `Remarks` varchar(1000) DEFAULT NULL COMMENT '备注',
  `CreateTime` datetime DEFAULT NULL,
  `Type` int(11) DEFAULT NULL,
  `Status` bit(1) DEFAULT NULL COMMENT '状态:启用/不启用',
  `Business` varchar(255) DEFAULT NULL COMMENT '业务权限',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_module`
--

LOCK TABLES `sys_module` WRITE;
/*!40000 ALTER TABLE `sys_module` DISABLE KEYS */;
INSERT INTO `sys_module` VALUES (2018062815980557969864649036,'会员管理','fa-users',NULL,0,0,NULL,'2018-07-16 16:57:11',1,0x01,NULL),(2018062815981957969031234232,'会员列表','fa-address-card-o','/Admin/GuestUser/Index',2018062815980557969864649036,0,NULL,'2018-07-16 16:57:12',1,0x01,NULL),(2018062815987357969262392159,'会员删除','fa-address-card','/Admin/GuestUser/DelIndex',2018062815980557969864649036,0,NULL,'2018-07-16 16:57:13',1,0x01,NULL),(2018062815997257970221688814,'权限管理','fa-users',NULL,0,99,NULL,'2018-07-16 16:57:16',1,0x01,NULL),(2018062816009457972568398324,'模块管理','fa-users','/SysAdmin/SysModule/Index',2018062815997257970221688814,7,NULL,'2018-07-16 16:57:17',1,0x01,NULL),(2018062816859057999420635628,'管理员列表','fa-users','/SysAdmin/SysUser/Index',2018062815997257970221688814,5,NULL,'2018-07-16 16:57:18',1,0x01,NULL),(2018062817150358015577110370,'角色管理','fa-users','/SysAdmin/SysRole/Index',2018062815997257970221688814,6,NULL,'2018-07-16 16:57:19',1,0x01,NULL),(2018070212881061312508571927,'我的工作台','fa-gears',NULL,0,100,NULL,'2018-07-16 16:57:20',1,0x01,NULL),(2018070212902161312528898025,'系统设置','fa-gear',NULL,2018070212881061312508571927,100,NULL,'2018-07-16 16:57:21',1,0x01,NULL),(2018070217780961485245413800,'基本设置','fa-gear','/SysAdmin/SysBasic/Index',2018070212902161312528898025,0,NULL,'2018-07-16 16:57:22',1,0x01,NULL),(2018070316871862319542568875,'数据库设置','fa-database',NULL,2018070212881061312508571927,99,NULL,'2018-07-16 16:57:23',1,0x01,NULL),(2018070316888262321082386501,'数据库备份','fa-cube','/SysAdmin/SysData/Index',2018070316871862319542568875,0,NULL,'2018-07-16 16:57:24',1,0x01,NULL),(2018070316890362319428115424,'备份文件列表','fa-cubes','/SysAdmin/SysData/Downs',2018070316871862319542568875,1,NULL,'2018-07-16 16:57:25',1,0x01,NULL),(2018071211996769922417363891,'消息中心','fa-comments-o','/SysAdmin/SysMessage/Index',2018070212881061312508571927,0,NULL,'2018-07-16 16:57:26',1,0x01,NULL),(2018071816798575274411015719,'文章管理','fa-book',NULL,0,0,NULL,'2018-07-16 16:57:33',1,0x01,NULL),(2018071816800575274124279678,'文章列表','fa-server','/Admin/Goods/Index',2018071816798575274411015719,0,NULL,'2018-07-16 16:57:35',1,0x01,NULL),(2018110914081273679717654206,'添加',NULL,NULL,2018071816800575274124279678,0,NULL,'2018-11-09 13:44:45',3,0x01,'admin:goods:add'),(2018111411889277915948230704,'工作流设计','fa-wikipedia-w','/SysAdmin/Workflow/Index',2018062815997257970221688814,0,NULL,'2018-11-14 11:25:26',1,0x01,NULL);
/*!40000 ALTER TABLE `sys_module` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_role`
--

DROP TABLE IF EXISTS `sys_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_role` (
  `RoleID` decimal(28,0) NOT NULL,
  `RoleName` varchar(50) DEFAULT NULL,
  `Remarks` varchar(255) DEFAULT NULL,
  `Status` bit(1) DEFAULT NULL,
  `CreateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`RoleID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role`
--

LOCK TABLES `sys_role` WRITE;
/*!40000 ALTER TABLE `sys_role` DISABLE KEYS */;
INSERT INTO `sys_role` VALUES (2018062817652558022317097561,'超级管理员','超级管理员',0x01,'2018-06-28 17:01:59'),(2018071710079674176088294023,'平台管理员',NULL,0x01,'2018-07-17 09:44:31');
/*!40000 ALTER TABLE `sys_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sys_role_module`
--

DROP TABLE IF EXISTS `sys_role_module`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_role_module` (
  `RoleID` decimal(28,0) NOT NULL,
  `ModuleID` decimal(28,0) NOT NULL,
  PRIMARY KEY (`RoleID`,`ModuleID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_role_module`
--

LOCK TABLES `sys_role_module` WRITE;
/*!40000 ALTER TABLE `sys_role_module` DISABLE KEYS */;
INSERT INTO `sys_role_module` VALUES (2018062817652558022317097561,2018062815980557969864649036),(2018062817652558022317097561,2018062815981957969031234232),(2018062817652558022317097561,2018062815987357969262392159),(2018062817652558022317097561,2018062815997257970221688814),(2018062817652558022317097561,2018062816009457972568398324),(2018062817652558022317097561,2018062816859057999420635628),(2018062817652558022317097561,2018062817150358015577110370),(2018062817652558022317097561,2018070212881061312508571927),(2018062817652558022317097561,2018070212902161312528898025),(2018062817652558022317097561,2018070217780961485245413800),(2018062817652558022317097561,2018070316871862319542568875),(2018062817652558022317097561,2018070316888262321082386501),(2018062817652558022317097561,2018070316890362319428115424),(2018062817652558022317097561,2018071211996769922417363891),(2018062817652558022317097561,2018071816798575274411015719),(2018062817652558022317097561,2018071816800575274124279678),(2018062817652558022317097561,2018110914081273679717654206),(2018062817652558022317097561,2018111411889277915948230704),(2018062911939058686897077222,2018062815997257970221688814),(2018062911939058686897077222,2018062816009457972568398324),(2018071614737273472108454573,2018062815988857971235984656),(2018071614737273472108454573,2018062815996657971680189850),(2018071614737273472108454573,2018070212881061312508571927),(2018071614737273472108454573,2018071211996769922417363891),(2018071710079674176088294023,2018071314830670885393270145),(2018071710079674176088294023,2018071314850270886418120683),(2018071710079674176088294023,2018071615831973513402656230),(2018071710079674176088294023,2018071615839973512529408971),(2018071710079674176088294023,2018071816798575274411015719),(2018071710079674176088294023,2018071816799475274724579641),(2018071710079674176088294023,2018071816800575274124279678),(2018071710079674176088294023,2018080316667789090672148354),(2018071710079674176088294023,2018080814186793333362200646),(2018071710079674176088294023,2018080814188493333607543405),(2018071710079674176088294023,2018080816636693409823947928),(2018071710079674176088294023,2018080816637693409312085344);
/*!40000 ALTER TABLE `sys_role_module` ENABLE KEYS */;
UNLOCK TABLES;


--
-- Table structure for table `sys_user`
--

DROP TABLE IF EXISTS `sys_user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_user` (
  `SysUserID` decimal(28,0) NOT NULL COMMENT 'ID',
  `SysUserName` varchar(100) NOT NULL COMMENT '登陆账号',
  `SysUserPwd` varchar(128) NOT NULL COMMENT '密码',
  `SysNickName` varchar(100) NOT NULL COMMENT '昵称',
  `Status` int(1) DEFAULT NULL COMMENT '0停用，1启用，2删除',
  `CreateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`SysUserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user`
--

LOCK TABLES `sys_user` WRITE;
/*!40000 ALTER TABLE `sys_user` DISABLE KEYS */;
INSERT INTO `sys_user` VALUES (2018071811688075088252067188,'admin','lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=','admin',2,'2018-07-18 11:05:14'),(2018071811692375089135144937,'admin','lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=','admin',2,'2018-07-18 11:05:57'),(2018071811709175090462154596,'admin','y8M77TUwUB3OZdb9ZcZpIioO2lIaVuO5BInnH7PuFC4=','admin',1,'2018-07-18 11:07:25'),(2023425697843432000925721593,'superAdmin','lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=','超级管理员',1,'2018-06-28 16:24:13');
/*!40000 ALTER TABLE `sys_user` ENABLE KEYS */;
UNLOCK TABLES;



DROP TABLE IF EXISTS `sys_user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sys_user_role` (
  `RoleID` decimal(28,0) NOT NULL,
  `UserID` decimal(28,0) NOT NULL,
  PRIMARY KEY (`RoleID`,`UserID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_role`
--

LOCK TABLES `sys_user_role` WRITE;
/*!40000 ALTER TABLE `sys_user_role` DISABLE KEYS */;
INSERT INTO `sys_user_role` VALUES (2018062817652558022317097561,2023425697843432000925721593),(2018071710079674176088294023,2018071811688075088252067188),(2018071710079674176088294023,2018071811692375089135144937),(2018071710079674176088294023,2018071811709175090462154596);
/*!40000 ALTER TABLE `sys_user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_info`
--

DROP TABLE IF EXISTS `user_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_info` (
  `UserID` decimal(28,0) NOT NULL COMMENT 'ID',
  `UserAccount` varchar(100) DEFAULT NULL COMMENT '账号',
  `Mobile` varchar(20) DEFAULT NULL COMMENT '手机号',
  `Email` varchar(100) DEFAULT NULL COMMENT 'Email',
  `WriteTime` datetime DEFAULT NULL COMMENT '创建时间',
  `UserPassword` varchar(128) DEFAULT NULL COMMENT '密码',
  `NickName` varchar(100) DEFAULT NULL COMMENT '昵称',
  `FullName` varchar(100) DEFAULT NULL COMMENT '姓名',
  `EnglishName` varchar(200) DEFAULT NULL COMMENT '英文名',
  `Gender` tinyint(4) DEFAULT NULL COMMENT '性别',
  `Birthday` date DEFAULT NULL COMMENT '出生日期',
  `UserOrgan` varchar(60) DEFAULT NULL COMMENT '所在单位',
  `OrganType` varchar(60) DEFAULT NULL COMMENT '单位类型',
  `OrganAddress` varchar(100) DEFAULT NULL COMMENT '单位所在地',
  `Department` varchar(60) DEFAULT NULL COMMENT '所在部门',
  `Position` varchar(60) DEFAULT NULL COMMENT '职位',
  `PostalAddress` varchar(100) DEFAULT NULL COMMENT '通信地址',
  `Remark` varchar(100) DEFAULT NULL COMMENT '备注',
  `CreateTime` datetime DEFAULT NULL COMMENT '创建时间',
  `Number` varchar(20) DEFAULT NULL COMMENT '学号',
  `SchoolName` varchar(60) DEFAULT NULL COMMENT '学校名称',
  `DepartmentName` varchar(60) DEFAULT NULL COMMENT '院系名称',
  `MajorName` varchar(60) DEFAULT NULL COMMENT '专业名称',
  `IsAnswerd` bit(1) DEFAULT NULL COMMENT '是否答题,0：否，1：是',
  `UserIdentity` varchar(100) DEFAULT NULL COMMENT '用户身份',
  `FirstDiscipline` varchar(100) DEFAULT NULL COMMENT '所属一级学科',
  `ResearchField` varchar(100) DEFAULT NULL COMMENT '研究领域',
  `Portrait` varchar(100) DEFAULT NULL COMMENT '头像',
  `Type` varchar(100) DEFAULT NULL COMMENT '注册类型',
  `isFirst` bit(1) DEFAULT NULL COMMENT '是否第一次登陆',
  `OverdueTime` datetime DEFAULT NULL COMMENT '账号过期时间（后台添加账户专用）',
  `PwdOverdueTime` datetime DEFAULT NULL COMMENT '密码过期时间（后台添加账户专用）',
  `IsAdopt` int(11) DEFAULT NULL COMMENT '是否通过  待审核1/通过2/不通过3',
  `Status` bit(1) DEFAULT NULL,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `UserID_UNIQUE` (`UserID`) USING BTREE,
  UNIQUE KEY `UserAccount_UNIQUE` (`UserAccount`) USING BTREE,
  UNIQUE KEY `Mobile_UNIQUE` (`Mobile`) USING BTREE,
  UNIQUE KEY `Email_UNIQUE` (`Email`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

DROP TABLE IF EXISTS `user_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_log` (
  `ID` decimal(28,0) NOT NULL,
  `UserID` decimal(28,0) NOT NULL COMMENT '登录ID',
  `NickName` varchar(100) DEFAULT NULL COMMENT '昵称',
  `LoginTime` datetime NOT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '登录时间',
  `LogoutTime` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP COMMENT '登出时间',
  `IP` varchar(100) NOT NULL COMMENT '登录电脑ip',
  `MaxAdress` varchar(100) NOT NULL COMMENT 'mac地址',
  `QuotaID` decimal(28,0) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `fk_user_info_UserID_user_log` (`UserID`) USING BTREE,
  CONSTRAINT `user_log_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `user_info` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

