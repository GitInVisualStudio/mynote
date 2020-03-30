-- DBs for MyNote
CREATE DATABASE IF NOT EXISTS `mynote` CHARACTER SET utf8mb4;
USE mynote;

-- TABLE USER

CREATE TABLE IF NOT EXISTS `mynote`.`user` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`email` VARCHAR(50) NOT NULL,
	`password` CHAR(64) NOT NULL,
	`salt` INT NOT NULL,
	`auth` CHAR(32) NULL,
	`auth_since` DATETIME NULL,
	PRIMARY KEY (`id`)
)
ENGINE=InnoDB;

-- TABLE Semester

CREATE TABLE IF NOT EXISTS `mynote`.`semester` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`name` VARCHAR(255) NULL,
	`created` DATETIME NOT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Kurs

CREATE TABLE IF NOT EXISTS `mynote`.`course` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`semester_id` BIGINT NOT NULL,
	`name` VARCHAR(255) NULL,
	`created` DATETIME NOT NULL,
	`path` VARCHAR(511) NOT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`semester_id`) REFERENCES `mynote`.`semester`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Canvas

CREATE TABLE IF NOT EXISTS `mynote`.`canvas`(
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`course_id` BIGINT NOT NULL,
	`name` VARCHAR(255) NULL,
	`created` DATETIME NOT NULL,
	`path` VARCHAR(511) NOT NULL,
	`type` ENUM("note", "vocabulary_listing", "excercise"),
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE User_in_Course

CREATE TABLE IF NOT EXISTS `mynote`.`user_in_course`(
	`user_id` BIGINT NOT NULL,
	`course_id` BIGINT NOT NULL,
	`admin` BOOLEAN NOT NULL,
	PRIMARY KEY (`user_id`, `course_id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)
ENGINE = InnoDB;

-- TABLE Test

CREATE TABLE IF NOT EXISTS `mynote`.`test`(
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`user_id` BIGINT NULL,
	`course_id` BIGINT NOT NULL,
	`topic` VARCHAR(255) NULL,
	`date` DATE NOT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`user_id`) REFERENCES `mynote`.`user`(`id`) ON DELETE SET NULL ON UPDATE CASCADE,
	FOREIGN KEY (`course_id`) REFERENCES `mynote`.`course`(`id`) ON DELETE CASCADE ON UPDATE CASCADE
)

