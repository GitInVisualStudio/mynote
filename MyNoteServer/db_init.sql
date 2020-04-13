-- DBs for MyNote
CREATE DATABASE IF NOT EXISTS `mynote` CHARACTER SET utf8mb4;
USE mynote;

-- TABLE USER

CREATE TABLE IF NOT EXISTS `mynote`.`user` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`email` VARCHAR(64) NOT NULL,
	`username` VARCHAR(32) NOT NULL,
	`password` CHAR(64) NOT NULL,
	`salt` INT NOT NULL,
	`auth` CHAR(64) NULL,
	`auth_since` DATETIME NULL,
	PRIMARY KEY (`id`)
)
ENGINE=InnoDB;

-- TABLE Semester

CREATE TABLE IF NOT EXISTS `mynote`.`semester` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`name` VARCHAR(256) NULL,
	`created` DATETIME NOT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Icon

CREATE TABLE IF NOT EXISTS `mynote`.`icon` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(64) NULL,
	`path` VARCHAR(256) NOT NULL,
	PRIMARY KEY (`id`)
)
ENGINE = InnoDB;
INSERT IGNORE INTO `icon` VALUES (1, 'default', '');

-- TABLE Kurs

CREATE TABLE IF NOT EXISTS `mynote`.`course` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`semester_id` BIGINT NOT NULL,
	`name` VARCHAR(256) NULL,
	`created` DATETIME NOT NULL,
	`color` VARCHAR(64) NULL,
	`icon_id` BIGINT NULL,
	`password` CHAR(64) NULL,
	`salt` INT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`semester_id`) REFERENCES `mynote`.`semester`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (`icon_id`) REFERENCES `mynote`.`icon`(`id`) ON DELETE SET NULL ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Canvas

CREATE TABLE IF NOT EXISTS `mynote`.`canvas`(
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`course_id` BIGINT NOT NULL,
	`name` VARCHAR(256) NULL,
	`created` DATETIME NOT NULL,
	`path` VARCHAR(512) NOT NULL,
	`type` ENUM("note", "vocabulary_listing", "excercise"),
	`public` BOOLEAN NOT NULL DEFAULT FALSE,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE interpretation

CREATE TABLE IF NOT EXISTS `mynote`.`interpretation`(
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`name` VARCHAR(256) NULL,
	`color` VARCHAR(256) NULL,
	`icon_id` BIGINT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`icon_id`) REFERENCES `mynote`.`icon`(`id`) ON DELETE SET NULL ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE user_in_course

CREATE TABLE IF NOT EXISTS `mynote`.`user_in_course`(
	`user_id` BIGINT NOT NULL,
	`course_id` BIGINT NOT NULL,
	`admin` BOOLEAN NOT NULL,
	`member_since` DATETIME NOT NULL,
	`interpretation_id` BIGINT NULL,
	PRIMARY KEY (`user_id`, `course_id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (`interpretation_id`) REFERENCES `mynote`.`interpretation`(`id`) ON DELETE SET NULL ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Test

CREATE TABLE IF NOT EXISTS `mynote`.`test`(
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`course_id` BIGINT NOT NULL,
	`topic` VARCHAR(256) NULL,
	`date` DATE NOT NULL,
	`type` ENUM("vocabulary_test", "exam", "test") NOT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)

