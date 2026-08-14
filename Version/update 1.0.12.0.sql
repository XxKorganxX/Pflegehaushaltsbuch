SET @office_cash_exists := (
    SELECT COUNT(*)
    FROM information_schema.TABLES
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'office_cash'
);
SET @petty_cash_exists := (
    SELECT COUNT(*)
    FROM information_schema.TABLES
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'petty_cash'
);
SET @sql := IF(@office_cash_exists = 1 AND @petty_cash_exists = 0, 'RENAME TABLE `office_cash` TO `petty_cash`', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

DROP VIEW IF EXISTS `office_total_amount`;
CREATE VIEW `office_total_amount` AS Select COALESCE(SUM(amount),0) from `petty_cash`;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'users'
      AND COLUMN_NAME = 'name'
);
SET @handsign_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'users'
      AND COLUMN_NAME = 'handsign'
);
SET @sql := IF(@column_exists = 1 AND @handsign_exists = 0, 'ALTER TABLE `users` CHANGE COLUMN `name` `handsign` varchar(255) NOT NULL', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

UPDATE `users`
SET `login` = `handsign`
WHERE `login` IS NULL OR TRIM(`login`) = '';
