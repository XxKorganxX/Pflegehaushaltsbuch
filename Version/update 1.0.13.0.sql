SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'company'
      AND COLUMN_NAME = 'currency_code'
);
SET @sql := IF(@column_exists = 0, 'ALTER TABLE `company` ADD COLUMN `currency_code` varchar(3) DEFAULT ''EUR''', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

UPDATE `company`
SET `currency_code` = 'EUR'
WHERE `currency_code` IS NULL OR TRIM(`currency_code`) = '';

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'users'
      AND COLUMN_NAME = 'failed_login_attempts'
);
SET @sql := IF(@column_exists = 0, 'ALTER TABLE `users` ADD COLUMN `failed_login_attempts` int(11) NOT NULL DEFAULT 0', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'users'
      AND COLUMN_NAME = 'last_failed_login'
);
SET @sql := IF(@column_exists = 0, 'ALTER TABLE `users` ADD COLUMN `last_failed_login` DATETIME NULL', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

SET @column_exists := (
    SELECT COUNT(*)
    FROM information_schema.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
      AND TABLE_NAME = 'users'
      AND COLUMN_NAME = 'locked_until'
);
SET @sql := IF(@column_exists = 0, 'ALTER TABLE `users` ADD COLUMN `locked_until` DATETIME NULL', 'SELECT 1');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
