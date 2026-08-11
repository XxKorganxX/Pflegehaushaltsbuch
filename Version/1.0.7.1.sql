-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: verwahrgeld
-- ------------------------------------------------------
-- Server version	5.6.21-log

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
-- Table structure for table `advisors`
--

DROP TABLE IF EXISTS `advisors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `advisors` (
`id` int(11) unsigned NOT NULL,
`title` varchar(64) NOT NULL,
`name` varchar(255) NOT NULL,
`email` varchar(128) DEFAULT NULL,
`co` varchar(128) DEFAULT NULL,
`street` varchar(128) NOT NULL,
`zipcode` varchar(45) NOT NULL,
`city` varchar(128) NOT NULL,
`date` date NOT NULL DEFAULT '2014-01-03',
`handsign` varchar(64) NOT NULL DEFAULT 'Anna Gottof',
PRIMARY KEY (`id`),
UNIQUE KEY `name_UNIQUE` (`name`),
UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `assistants`
--

DROP TABLE IF EXISTS `assistants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `assistants` (
`id` int(10) unsigned NOT NULL,
`name` varchar(255) NOT NULL,
`account_transfer` decimal(10,2) NOT NULL DEFAULT '0.00',
`amount_payout` decimal(10,2) NOT NULL DEFAULT '0.00',
`amount_payback` decimal(10,2) NOT NULL DEFAULT '0.00',
`amount_payback_type` int(10) unsigned NOT NULL DEFAULT '0',
`date` date NOT NULL,
`active` bit(1) NOT NULL DEFAULT b'1',
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `bank`
--

DROP TABLE IF EXISTS `bank`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `bank` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`date` date NOT NULL,
`note` varchar(255) NOT NULL,
`amount` decimal(10,2) NOT NULL,
`account` varchar(64) NOT NULL,
`book_to` int(10) unsigned NOT NULL DEFAULT '0',
`book_cat` int(10) unsigned NOT NULL DEFAULT '0',
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `barge`
--

DROP TABLE IF EXISTS `barge`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `barge` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`date` date NOT NULL,
`note` varchar(255) NOT NULL,
`book_cat` int(11) NOT NULL DEFAULT '0',
`book_to` int(11) NOT NULL DEFAULT '0',
`amount` decimal(10,2) NOT NULL,
`account` varchar(64) NOT NULL,
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `books`
--

DROP TABLE IF EXISTS `books`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `books` (
`index` int(10) unsigned NOT NULL AUTO_INCREMENT,
`id` int(10) unsigned NOT NULL,
`document_id` int(10) unsigned NOT NULL,
`date` date NOT NULL,
`note` varchar(255) NOT NULL,
`book_cat` int(10) unsigned NOT NULL DEFAULT '0',
`book_to` int(10) unsigned NOT NULL DEFAULT '0',
`amount` decimal(10,2) NOT NULL,
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`index`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `books_AINS` AFTER INSERT ON `books` FOR EACH ROW



begin



Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from books where NEW.id=id) where NEW.id=clients.id;



Update clients SET lastbook=(Select max(date) from books where NEW.id=id) where NEW.id=clients.id;



end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `books_AUPD` AFTER UPDATE ON `books` FOR EACH ROW



begin



Update clients SET amount=account_transfer + (Select COALESCE(sum(amount),0) from books where NEW.id=id) where NEW.id=clients.id;



Update clients SET lastbook=(Select max(date) from books where NEW.id=id) where NEW.id=clients.id;



end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `books_ADEL` AFTER DELETE ON `books` FOR EACH ROW



begin



Update clients SET amount=account_transfer +(Select COALESCE(sum(amount),0) from books where OLD.id=id) where OLD.id=clients.id;



Update clients SET lastbook=(Select max(date) from books where OLD.id=id) where OLD.id=clients.id;



end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
`id` int(10) unsigned NOT NULL,
`title` varchar(8) NOT NULL,
`name` varchar(255) NOT NULL,
`street` varchar(128) NOT NULL,
`zipcode` varchar(45) NOT NULL,
`city` varchar(128) NOT NULL,
`born` date NOT NULL,
`date` date NOT NULL,
`account_transfer` decimal(10,2) NOT NULL DEFAULT '0.00',
`amount` decimal(10,2) NOT NULL DEFAULT '0.00',
`lastbook` date DEFAULT NULL,
`active` int(10) NOT NULL DEFAULT '0',
`info` int(10) unsigned DEFAULT NULL,
`note` varchar(255) DEFAULT NULL,
`advisor_id` int(10) unsigned DEFAULT NULL,
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`id`),
UNIQUE KEY `name_UNIQUE` (`name`),
UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `clients_BUPD` BEFORE UPDATE ON `clients` FOR EACH ROW



begin



Set New.amount=New.account_transfer + (Select COALESCE(sum(amount),0) from books where NEW.id=id);



end */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `company`
--

DROP TABLE IF EXISTS `company`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `company` (
`name` varchar(255) NOT NULL,
`secretary` varchar(128) DEFAULT NULL,
`phone` varchar(45) NOT NULL,
`fax` varchar(45) NOT NULL,
`email` varchar(128) NOT NULL,
`street` varchar(255) NOT NULL,
`zipcode` varchar(45) NOT NULL,
`city` varchar(255) NOT NULL,
`language` varchar(64) DEFAULT NULL,
`web` varchar(256) DEFAULT NULL,
`local_court` varchar(128) DEFAULT NULL,
`hrb` varchar(64) DEFAULT NULL,
`ik` varchar(64) DEFAULT NULL,
`smtp_host` varchar(256) DEFAULT NULL,
`smtp_user` varchar(256) DEFAULT NULL,
`smtp_key` varchar(256) DEFAULT NULL,
PRIMARY KEY (`name`),
UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `company_bank`
--

DROP TABLE IF EXISTS `company_bank`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `company_bank` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`name` varchar(128) DEFAULT NULL,
`code` varchar(64) DEFAULT NULL,
`account_no` varchar(64) DEFAULT NULL,
`iban` varchar(128) DEFAULT NULL,
`bic` varchar(64) DEFAULT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `deadlines`
--

DROP TABLE IF EXISTS `deadlines`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `deadlines` (
`no` int(11) unsigned NOT NULL AUTO_INCREMENT,
`id` int(11) NOT NULL,
`date` date NOT NULL,
`note` varchar(512) NOT NULL,
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`no`),
UNIQUE KEY `no_UNIQUE` (`no`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hard_cash`
--

DROP TABLE IF EXISTS `hard_cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hard_cash` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`001` int(11) unsigned NOT NULL DEFAULT '0',
`002` int(11) unsigned NOT NULL DEFAULT '0',
`005` int(11) unsigned NOT NULL DEFAULT '0',
`010` int(11) unsigned NOT NULL DEFAULT '0',
`020` int(11) unsigned NOT NULL DEFAULT '0',
`050` int(11) unsigned NOT NULL DEFAULT '0',
`1` int(11) unsigned NOT NULL DEFAULT '0',
`2` int(11) unsigned NOT NULL DEFAULT '0',
`5` int(11) unsigned NOT NULL DEFAULT '0',
`10` int(11) unsigned NOT NULL DEFAULT '0',
`20` int(11) unsigned NOT NULL DEFAULT '0',
`50` int(11) unsigned NOT NULL DEFAULT '0',
`100` int(11) unsigned NOT NULL DEFAULT '0',
`200` int(11) unsigned NOT NULL DEFAULT '0',
`500` int(11) unsigned NOT NULL DEFAULT '0',
PRIMARY KEY (`id`),
UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `layouts`
--

DROP TABLE IF EXISTS `layouts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `layouts` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`accounts` mediumblob,
`advisors` mediumblob,
`assistants` mediumblob,
`bank` mediumblob,
`cash` mediumblob,
`cashaudit` mediumblob,
`clients` mediumblob,
`quittance` mediumblob,
`officecash` mediumblob,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `license`
--

DROP TABLE IF EXISTS `license`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `license` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`grade` int(11) DEFAULT NULL,
`begin` date DEFAULT NULL,
`expired` date DEFAULT NULL,
`key` varbinary(2048) DEFAULT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `office_cash`
--

DROP TABLE IF EXISTS `office_cash`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `office_cash` (
`id` int(11) unsigned NOT NULL AUTO_INCREMENT,
`date` date DEFAULT NULL,
`note` varchar(512) DEFAULT NULL,
`account` int(11) DEFAULT NULL,
`book_cat` int(10) unsigned DEFAULT NULL,
`amount` decimal(10,2) DEFAULT NULL,
`handsign` varchar(64) DEFAULT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `record`
--

DROP TABLE IF EXISTS `record`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `record` (
`index` int(10) unsigned NOT NULL AUTO_INCREMENT,
`id` int(11) unsigned NOT NULL,
`date` date NOT NULL,
`note` varchar(255) NOT NULL,
`filename` varchar(255) NOT NULL,
`file` mediumblob NOT NULL,
`handsign` varchar(64) NOT NULL,
PRIMARY KEY (`index`,`id`),
UNIQUE KEY `id_UNIQUE` (`index`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `users` (
`name` varchar(255) NOT NULL,
`login` varchar(255) NOT NULL,
`pw` varchar(255) NOT NULL,
`phone` varchar(90) NOT NULL,
`fax` varchar(90) NOT NULL,
`email` varchar(255) NOT NULL,
`access` int(11) NOT NULL,
`admin` tinyint(1) NOT NULL DEFAULT '0',
PRIMARY KEY (`name`,`login`),
UNIQUE KEY `name_UNIQUE` (`name`),
UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `version`
--

DROP TABLE IF EXISTS `version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `version` (
`id` int(11) NOT NULL AUTO_INCREMENT,
`main` varchar(64) NOT NULL,
PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping events for database 'verwahrgeld'
--

--
-- Dumping routines for database 'verwahrgeld'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-07-02  8:38:12
