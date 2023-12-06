-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: heroforge.mysql.dbaas.com.br    Database: heroforge
-- ------------------------------------------------------
-- Server version	5.7.32-35-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Languages`
--

DROP TABLE IF EXISTS `Languages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Languages` (
  `LanguageID` int(11) NOT NULL AUTO_INCREMENT,
  `LanguageCode` varchar(10) COLLATE latin1_general_ci NOT NULL,
  `LanguageName` varchar(255) COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`LanguageID`),
  UNIQUE KEY `LanguageCode` (`LanguageCode`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Languages`
--

LOCK TABLES `Languages` WRITE;
/*!40000 ALTER TABLE `Languages` DISABLE KEYS */;
INSERT INTO `Languages` VALUES (1,'en','English'),(2,'pt-BR','Portuguese (Brazil)');
/*!40000 ALTER TABLE `Languages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ProfileTranslations`
--

DROP TABLE IF EXISTS `ProfileTranslations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ProfileTranslations` (
  `ProfileID` int(11) NOT NULL,
  `LanguageCode` varchar(10) COLLATE latin1_general_ci NOT NULL,
  `TranslatedName` varchar(255) COLLATE latin1_general_ci DEFAULT NULL,
  PRIMARY KEY (`ProfileID`,`LanguageCode`),
  KEY `LanguageCode` (`LanguageCode`),
  CONSTRAINT `ProfileTranslations_ibfk_1` FOREIGN KEY (`ProfileID`) REFERENCES `Profiles` (`ProfileID`),
  CONSTRAINT `ProfileTranslations_ibfk_2` FOREIGN KEY (`LanguageCode`) REFERENCES `Languages` (`LanguageCode`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ProfileTranslations`
--

LOCK TABLES `ProfileTranslations` WRITE;
/*!40000 ALTER TABLE `ProfileTranslations` DISABLE KEYS */;
INSERT INTO `ProfileTranslations` VALUES (1,'en','Admin'),(1,'pt-BR','Administrador'),(2,'en','Collaborator'),(2,'pt-BR','Colaborador'),(3,'en','User'),(3,'pt-BR','Usuário');
/*!40000 ALTER TABLE `ProfileTranslations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Profiles`
--

DROP TABLE IF EXISTS `Profiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Profiles` (
  `ProfileID` int(11) NOT NULL AUTO_INCREMENT,
  `ProfileName` varchar(255) COLLATE latin1_general_ci DEFAULT NULL,
  `ProfileDescription` varchar(255) COLLATE latin1_general_ci DEFAULT NULL,
  `LanguageCode` varchar(10) COLLATE latin1_general_ci DEFAULT 'en',
  PRIMARY KEY (`ProfileID`),
  KEY `LanguageCode` (`LanguageCode`),
  CONSTRAINT `Profiles_ibfk_1` FOREIGN KEY (`LanguageCode`) REFERENCES `Languages` (`LanguageCode`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Profiles`
--

LOCK TABLES `Profiles` WRITE;
/*!40000 ALTER TABLE `Profiles` DISABLE KEYS */;
INSERT INTO `Profiles` VALUES (1,'Admin','Administrative privileges','en'),(2,'Colaborador','Collaborator privileges','en'),(3,'Usuário','User privileges','en');
/*!40000 ALTER TABLE `Profiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `UserProfiles`
--

DROP TABLE IF EXISTS `UserProfiles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `UserProfiles` (
  `UserID` int(11) NOT NULL,
  `ProfileID` int(11) NOT NULL,
  PRIMARY KEY (`UserID`,`ProfileID`),
  KEY `fk2` (`ProfileID`),
  CONSTRAINT `fk1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`UserID`),
  CONSTRAINT `fk2` FOREIGN KEY (`ProfileID`) REFERENCES `Profiles` (`ProfileID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `UserProfiles`
--

LOCK TABLES `UserProfiles` WRITE;
/*!40000 ALTER TABLE `UserProfiles` DISABLE KEYS */;
/*!40000 ALTER TABLE `UserProfiles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Users`
--

DROP TABLE IF EXISTS `Users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Users` (
  `UserID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) COLLATE latin1_general_ci NOT NULL,
  `UserEmail` varchar(255) COLLATE latin1_general_ci NOT NULL,
  `UserPassword` varchar(255) COLLATE latin1_general_ci NOT NULL,
  `AuthMode` varchar(50) COLLATE latin1_general_ci NOT NULL,
  `DateRegistration` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastLogin` timestamp NULL DEFAULT NULL,
  `LoginAttempts` int(11) DEFAULT '0',
  `AccountLocked` tinyint(1) DEFAULT '0',
  `StatusAccount` varchar(255) COLLATE latin1_general_ci DEFAULT NULL,
  `DateUpdated` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `UserEmail` (`UserEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Users`
--

LOCK TABLES `Users` WRITE;
/*!40000 ALTER TABLE `Users` DISABLE KEYS */;
/*!40000 ALTER TABLE `Users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'heroforge'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-12-05 20:59:11
