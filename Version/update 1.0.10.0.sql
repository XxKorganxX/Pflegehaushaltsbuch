SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'office_cash'
      AND COLUMN_NAME = 'account_id'
);
SET @sql := IF(@column_exists = 0, 'ALTER TABLE `office_cash` ADD COLUMN `account_id` int(11) DEFAULT NULL', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'office_cash'
      AND COLUMN_NAME = 'account'
);
SET @sql := IF(@column_exists = 1, 'UPDATE `office_cash` SET `account_id` = `account` WHERE `account_id` IS NULL', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'cash_books'
      AND COLUMN_NAME = 'account'
);
SET @sql := IF(@column_exists = 1, 'ALTER TABLE `cash_books` DROP COLUMN `account`', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'bank_books'
      AND COLUMN_NAME = 'account'
);
SET @sql := IF(@column_exists = 1, 'ALTER TABLE `bank_books` DROP COLUMN `account`', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'office_cash'
      AND COLUMN_NAME = 'account'
);
SET @sql := IF(@column_exists = 1, 'ALTER TABLE `office_cash` DROP COLUMN `account`', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
