RENAME TABLE `assistants` TO `employees`;
RENAME TABLE `barge` TO `cash_books`;
RENAME TABLE `bank` TO `bank_books`;
RENAME TABLE `books` TO `client_books`;
ALTER TABLE `layouts` CHANGE COLUMN `assistants` `employees` MEDIUMBLOB;
CREATE TABLE IF NOT EXISTS `accounts` (
`id` int(11) NOT NULL,
`type` varchar(64) NOT NULL,
`active` int(11) NOT NULL DEFAULT '1',
`created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
PRIMARY KEY (`id`)
);
INSERT IGNORE INTO `accounts` (`id`, `type`, `active`, `created_at`) VALUES
(0, 'Cash', 1, CURRENT_TIMESTAMP),
(1, 'Bank', 1, CURRENT_TIMESTAMP);
ALTER TABLE `clients` ADD COLUMN `account_id` int(11) DEFAULT NULL;
ALTER TABLE `employees` ADD COLUMN `account_id` int(11) DEFAULT NULL;
SET @next_account_id := (SELECT COALESCE(MAX(`id`), 1) + 1 FROM `accounts`);
UPDATE `clients` SET `account_id` = (@next_account_id := @next_account_id + 1) - 1 WHERE `account_id` IS NULL ORDER BY `id`;
SET @next_account_id := (SELECT COALESCE(MAX(`account_id`), 1) + 1 FROM `clients`);
UPDATE `employees` SET `account_id` = (@next_account_id := @next_account_id + 1) - 1 WHERE `account_id` IS NULL ORDER BY `id`;
INSERT IGNORE INTO `accounts` (`id`, `type`, `active`, `created_at`)
SELECT `account_id`, 'Client', COALESCE(`active`, 1), CURRENT_TIMESTAMP FROM `clients` WHERE `account_id` IS NOT NULL;
INSERT IGNORE INTO `accounts` (`id`, `type`, `active`, `created_at`)
SELECT `account_id`, 'Employee', COALESCE(`active`, 1), CURRENT_TIMESTAMP FROM `employees` WHERE `account_id` IS NOT NULL;
ALTER TABLE `cash_books` ADD COLUMN `account_id` int(11) DEFAULT NULL;
ALTER TABLE `bank_books` ADD COLUMN `account_id` int(11) DEFAULT NULL;
DROP TRIGGER IF EXISTS `books_AUPD`;
DROP TRIGGER IF EXISTS `books_AINS`;
DROP TRIGGER IF EXISTS `books_ADEL`;
DROP TRIGGER IF EXISTS `clients_BUPD`;
DROP TRIGGER IF EXISTS `clients_AUPD`;
DROP TRIGGER IF EXISTS `client_books_AUPD`;
DROP TRIGGER IF EXISTS `client_books_AINS`;
DROP TRIGGER IF EXISTS `client_books_ADEL`;
DROP VIEW IF EXISTS `bank_total_amount`;
DROP VIEW IF EXISTS `barge_total_amount`;
DROP VIEW IF EXISTS `cash_total_amount`;
DROP VIEW IF EXISTS `office_total_amount`;
