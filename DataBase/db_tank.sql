/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : db_tank

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2018-01-15 21:11:36
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `account`
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `acc_id` int(11) NOT NULL AUTO_INCREMENT,
  `acc_name` varchar(30) NOT NULL,
  `acc_pwd` varchar(30) NOT NULL,
  `acc_role` int(11) DEFAULT '-1',
  `acc_nick` varchar(30) DEFAULT '',
  `acc_level` int(11) DEFAULT '1',
  `acc_coin` int(11) DEFAULT '500',
  `acc_diamond` int(11) DEFAULT '0',
  `acc_email` varchar(255) DEFAULT '',
  `acc_mobile` varchar(255) DEFAULT '',
  `acc_regdate` timestamp NULL DEFAULT NULL,
  `acc_lastlogin` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`acc_id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of account
-- ----------------------------
INSERT INTO `account` VALUES ('1', 'frank', '123', '1', '周测试', '1', '500', '0', '', '', '2018-01-10 19:36:26', '2018-01-10 21:19:33');
INSERT INTO `account` VALUES ('2', 'abc', '123', '3', 'test', '1', '500', '0', '', '', '2018-01-10 19:36:31', '2018-01-13 23:04:48');
INSERT INTO `account` VALUES ('3', 'frank1', '1232', '-1', '', '1', '500', '0', '', '', '2018-01-10 19:35:41', '2018-01-10 19:35:41');
INSERT INTO `account` VALUES ('4', 'aaa', 'bbb', '-1', '', '1', '500', '0', '', '', '2018-01-10 19:35:42', '2018-01-10 19:35:42');
INSERT INTO `account` VALUES ('5', 'bbbb', 'aaaad', '-1', '', '1', '500', '0', '', '', '2018-01-10 19:35:43', '2018-01-10 19:35:43');
