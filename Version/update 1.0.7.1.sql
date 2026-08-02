ALTER TABLE layouts ADD quittance MEDIUMBLOB;
ALTER TABLE layouts ADD officecash MEDIUMBLOB;

ALTER TABLE company ADD web varchar(256) after email;
ALTER TABLE company ADD secretary varchar(128) after name;
ALTER TABLE company ADD local_court varchar(128);
ALTER TABLE company ADD hrb varchar(64);
ALTER TABLE company ADD ik varchar(64);
ALTER TABLE company ADD smtp_host varchar(512);
ALTER TABLE company ADD smtp_user varchar(512);
ALTER TABLE company ADD smtp_key varchar(512);

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