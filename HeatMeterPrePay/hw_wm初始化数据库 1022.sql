/*
 Navicat Premium Data Transfer

 Source Server         : 本机3307
 Source Server Type    : MySQL
 Source Server Version : 50729
 Source Host           : localhost:3307
 Source Schema         : hw_wm

 Target Server Type    : MySQL
 Target Server Version : 50729
 File Encoding         : 65001

 Date: 22/10/2020 11:24:46
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for carddata
-- ----------------------------
DROP TABLE IF EXISTS `carddata`;
CREATE TABLE `carddata`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NULL DEFAULT NULL,
  `cardId` int(11) NULL DEFAULT NULL,
  `operator` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE INDEX `userId`(`userId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of carddata
-- ----------------------------

-- ----------------------------
-- Table structure for companylog
-- ----------------------------
DROP TABLE IF EXISTS `companylog`;
CREATE TABLE `companylog`  (
  `companyId` int(11) NOT NULL AUTO_INCREMENT,
  `companyName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `companyCharger` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `contactNum` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `companyAdd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `email` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`companyId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 3001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of companylog
-- ----------------------------

-- ----------------------------
-- Table structure for dbinfo
-- ----------------------------
DROP TABLE IF EXISTS `dbinfo`;
CREATE TABLE `dbinfo`  (
  `key` int(11) NOT NULL DEFAULT 1,
  `dbVersion` int(11) NULL DEFAULT 0,
  PRIMARY KEY (`key`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of dbinfo
-- ----------------------------

-- ----------------------------
-- Table structure for hardwaresetting
-- ----------------------------
DROP TABLE IF EXISTS `hardwaresetting`;
CREATE TABLE `hardwaresetting`  (
  `paraId` int(11) NOT NULL AUTO_INCREMENT,
  `metername` int(11) NULL DEFAULT NULL,
  `hardwareInfo` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `setTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`paraId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of hardwaresetting
-- ----------------------------

-- ----------------------------
-- Table structure for histrgtable
-- ----------------------------
DROP TABLE IF EXISTS `histrgtable`;
CREATE TABLE `histrgtable`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `key` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `d` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of histrgtable
-- ----------------------------

-- ----------------------------
-- Table structure for meterstable
-- ----------------------------
DROP TABLE IF EXISTS `meterstable`;
CREATE TABLE `meterstable`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `meterId` int(11) NULL DEFAULT NULL,
  `permanentUserId` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of meterstable
-- ----------------------------

-- ----------------------------
-- Table structure for operationlog
-- ----------------------------
DROP TABLE IF EXISTS `operationlog`;
CREATE TABLE `operationlog`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NULL DEFAULT NULL,
  `cardType` int(11) NULL DEFAULT NULL,
  `operationId` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `time` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of operationlog
-- ----------------------------

-- ----------------------------
-- Table structure for paylogtable
-- ----------------------------
DROP TABLE IF EXISTS `paylogtable`;
CREATE TABLE `paylogtable`  (
  `payLogId` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NULL DEFAULT NULL,
  `userName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `pursuitNum` int(11) NULL DEFAULT NULL,
  `unitPrice` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `totalPrice` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `payType` int(11) NULL DEFAULT NULL,
  `dealType` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `operateTime` int(11) NULL DEFAULT NULL,
  `userCardLogId` int(11) NULL DEFAULT NULL,
  `permanentUserId` int(11) NULL DEFAULT NULL,
  `realPayNum` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`payLogId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 7 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of paylogtable
-- ----------------------------

-- ----------------------------
-- Table structure for priceconsistdetailtable
-- ----------------------------
DROP TABLE IF EXISTS `priceconsistdetailtable`;
CREATE TABLE `priceconsistdetailtable`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `priceConsistId` int(11) NULL DEFAULT NULL,
  `priceFactorId` int(11) NULL DEFAULT NULL,
  `unitPriceId` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of priceconsistdetailtable
-- ----------------------------

-- ----------------------------
-- Table structure for priceconsisttable
-- ----------------------------
DROP TABLE IF EXISTS `priceconsisttable`;
CREATE TABLE `priceconsisttable`  (
  `priceConsistId` int(11) NOT NULL AUTO_INCREMENT,
  `priceConstistName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceConstistValue` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceConstistStartTime` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceConstistEndTime` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceConstistStatus` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `calAsArea` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`priceConsistId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of priceconsisttable
-- ----------------------------

-- ----------------------------
-- Table structure for pricefactortable
-- ----------------------------
DROP TABLE IF EXISTS `pricefactortable`;
CREATE TABLE `pricefactortable`  (
  `priceFactorId` int(11) NOT NULL AUTO_INCREMENT,
  `priceFactorName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceFactorValue` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceFactorStartTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceFactorEndTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `priceFactorStatus` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`priceFactorId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of pricefactortable
-- ----------------------------

-- ----------------------------
-- Table structure for priceitemtable
-- ----------------------------
DROP TABLE IF EXISTS `priceitemtable`;
CREATE TABLE `priceitemtable`  (
  `priceItemId` int(11) NOT NULL AUTO_INCREMENT,
  `priceItemName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceStartTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceEndTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceItemStatus` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`priceItemId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of priceitemtable
-- ----------------------------

-- ----------------------------
-- Table structure for repairmeterlog
-- ----------------------------
DROP TABLE IF EXISTS `repairmeterlog`;
CREATE TABLE `repairmeterlog`  (
  `logId` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NULL DEFAULT NULL,
  `reasonType` int(11) NULL DEFAULT NULL,
  `time` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`logId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of repairmeterlog
-- ----------------------------

-- ----------------------------
-- Table structure for rgtable
-- ----------------------------
DROP TABLE IF EXISTS `rgtable`;
CREATE TABLE `rgtable`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `key` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `d` int(11) NULL DEFAULT NULL,
  `ivd` int(11) NULL DEFAULT NULL,
  `lud` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of rgtable
-- ----------------------------

-- ----------------------------
-- Table structure for settings
-- ----------------------------
DROP TABLE IF EXISTS `settings`;
CREATE TABLE `settings`  (
  `key` int(11) NOT NULL DEFAULT 1,
  `areaId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `versionNum` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `cardHardWareType` int(11) NULL DEFAULT NULL,
  `userIdBaseIndex` int(11) NULL DEFAULT NULL,
  `createFee` int(11) NULL DEFAULT NULL,
  `replaceFee` int(11) NULL DEFAULT NULL,
  `totalRegTime` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`key`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of settings
-- ----------------------------
INSERT INTO `settings` VALUES (1, '0', '1', NULL, NULL, NULL, NULL, NULL);

-- ----------------------------
-- Table structure for stafftable
-- ----------------------------
DROP TABLE IF EXISTS `stafftable`;
CREATE TABLE `stafftable`  (
  `staffId` int(11) NOT NULL AUTO_INCREMENT,
  `staffStatus` int(11) NULL DEFAULT NULL,
  `staffRank` int(11) NULL DEFAULT NULL,
  `staffName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `staffGender` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `staffPost` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `staffPhone` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `staffEmail` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `staffPwd` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `createTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `permissions` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT '0',
  PRIMARY KEY (`staffId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1002 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of stafftable
-- ----------------------------
INSERT INTO `stafftable` VALUES (1000, 0, 0, NULL, NULL, NULL, NULL, NULL, 'cebfd1559b68d67688884d7a3d3e8c', NULL, '18446744073709551615');

-- ----------------------------
-- Table structure for submeter
-- ----------------------------
DROP TABLE IF EXISTS `submeter`;
CREATE TABLE `submeter`  (
  `meterId` int(11) NOT NULL,
  `metername` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `setTime` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`meterId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of submeter
-- ----------------------------

-- ----------------------------
-- Table structure for unitpricetable
-- ----------------------------
DROP TABLE IF EXISTS `unitpricetable`;
CREATE TABLE `unitpricetable`  (
  `unitPriceId` int(11) NOT NULL AUTO_INCREMENT,
  `unitPriceName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `priceItemId` int(11) NULL DEFAULT NULL,
  `priceItemName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `unitPriceValue` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `unitPriceStartTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `unitPriceEndTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `unitPriceStatus` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`unitPriceId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of unitpricetable
-- ----------------------------

-- ----------------------------
-- Table structure for usercardlog
-- ----------------------------
DROP TABLE IF EXISTS `usercardlog`;
CREATE TABLE `usercardlog`  (
  `operationId` bigint(11) NOT NULL AUTO_INCREMENT,
  `time` int(11) NULL DEFAULT NULL,
  `userHead` int(11) NULL DEFAULT NULL,
  `deviceHead` int(11) NULL DEFAULT NULL,
  `userId` int(11) NULL DEFAULT NULL,
  `pursuitNum` int(11) NULL DEFAULT NULL,
  `totalNum` int(11) NULL DEFAULT NULL,
  `consumeTimes` int(11) NULL DEFAULT NULL,
  `operator` int(11) NULL DEFAULT NULL,
  `operateType` int(11) NULL DEFAULT NULL,
  `totalPayNum` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `unitPrice` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `lastReadInfo` int(11) UNSIGNED ZEROFILL NULL DEFAULT 00000000000,
  `permanentUserId` int(11) NULL DEFAULT NULL,
  `isRefund` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`operationId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of usercardlog
-- ----------------------------

-- ----------------------------
-- Table structure for userstable
-- ----------------------------
DROP TABLE IF EXISTS `userstable`;
CREATE TABLE `userstable`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NULL DEFAULT NULL,
  `username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `phoneNum` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `identityId` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `address` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `isActive` int(11) NULL DEFAULT NULL,
  `operator` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `permanentUserId` int(11) NULL DEFAULT NULL,
  `userArea` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `userPersons` int(11) NULL DEFAULT NULL,
  `userTypeId` int(11) NULL DEFAULT NULL,
  `userPriceConsistId` int(11) NULL DEFAULT NULL,
  `userBalance` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `createTime` bigint(20) NULL DEFAULT NULL,
  `totalPursuitNum` varchar(32) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of userstable
-- ----------------------------

-- ----------------------------
-- Table structure for usertypetable
-- ----------------------------
DROP TABLE IF EXISTS `usertypetable`;
CREATE TABLE `usertypetable`  (
  `typeId` int(11) NOT NULL AUTO_INCREMENT,
  `hardwareInfo` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `userType` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `alertValue` int(11) NULL DEFAULT NULL,
  `closeValue` int(11) NULL DEFAULT NULL,
  `limitValue` int(11) NULL DEFAULT NULL,
  `setValue` int(11) NULL DEFAULT NULL,
  `operator` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `setTime` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL,
  `onoffOneDayValue` int(11) NULL DEFAULT NULL,
  `overZeroValue` int(11) NULL DEFAULT NULL,
  `powerDownFlag` int(11) NULL DEFAULT NULL,
  `intervalTime` int(11) NULL DEFAULT NULL,
  PRIMARY KEY (`typeId`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 1001 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of usertypetable
-- ----------------------------

SET FOREIGN_KEY_CHECKS = 1;
