SET NOCOUNT ON
GO

USE master
GO
if exists (select * from sysdatabases where name='Bulletin')
		drop database Bulletin
go

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE Bulletin
  ON PRIMARY (NAME = N''Bulletin'', FILENAME = N''' + @device_directory + N'bulletin.mdf'')
  LOG ON (NAME = N''Bulletin_log'',  FILENAME = N''' + @device_directory + N'bulletin.ldf'')')
go

set quoted_identifier on
GO

/* Set DATEFORMAT so that the date strings are interpreted correctly regardless of
   the default DATEFORMAT on the server.
*/
SET DATEFORMAT mdy
GO
use "Bulletin"
go
if exists (select * from sys.objects where name = 'Users' and objectproperty(object_id('dbo.Users'), 'IsUserTable')=1)
	drop table "dbo"."Users"
GO

if exists (select * from sys.objects where name = 'Promos' and objectproperty(object_id('dbo.Promos'), 'IsUserTable')=1)
	drop table "dbo"."Promos"
GO

if exists (select * from sys.objects where name = 'Categories' and objectproperty(object_id('dbo.Categories'), 'IsUserTable')=1)
	drop table "dbo"."Categories"
GO

if exists (select * from sys.objects where name = 'PromosCategories' and objectproperty(object_id('dbo.PromosCategories'), 'IsUserTable')=1)
	drop table "dbo"."PromosCategories"
GO


CREATE TABLE "Users" (
	"UserId" int IDENTITY (101, 2) NOT NULL ,
	"LastName" nvarchar (30) NOT NULL ,
	"FirstName" nvarchar (20) NOT NULL ,
	"PreferredName" nvarchar (20) NULL ,
	"SignUpDate" datetime NULL ,
	"Address" nvarchar (60) NULL ,
	"City" nvarchar (15) NULL ,
	"State" nvarchar (3) NULL,
	"PostalCode" nvarchar (10) NULL ,
	"SchoolEmail" nvarchar (50) NULL ,
	"Hyperlink" nvarchar (255) NULL ,
	"Phone" nvarchar (24) NULL ,
	"Photo" varbinary(max) NULL ,
	"Notes" nvarchar (300) NULL ,
	"PhotoPath" nvarchar (255) NULL
	CONSTRAINT "PK_Users" PRIMARY KEY  CLUSTERED 
	(
		"UserId"
	)
)
GO
 CREATE  INDEX "LastName" ON "dbo"."Users"("LastName")
GO
GO
 CREATE  INDEX "State" ON "dbo"."Users"("State")
GO


CREATE TABLE "Categories" (
	"CategoryId" int IDENTITY (1, 1) NOT NULL ,
	"CategoryName" nvarchar (30) NOT NULL UNIQUE ,
	"Description" nvarchar (300) NULL ,
	CONSTRAINT "PK_Categories" PRIMARY KEY  CLUSTERED 
	(
		"CategoryId"
	)
)
GO
 CREATE  INDEX "CategoryName" ON "dbo"."Categories"("CategoryName")
GO


CREATE TABLE "Promos" (
	"PromoId" int IDENTITY (1,1) NOT NULL,
	"UserId" int NULL,     	/*Mark NULL for first version, so these can be made independent of the User table*/
	"ContactName" nvarchar (40) NULL ,
	"PromoType" nvarchar (10) NULL,           /*Offers or requests*/
	"PostDate" date NOT NULL ,
	"Headline" nvarchar (100) NULL,
	"Description" nvarchar (255) NULL,
	"ContactInfo" nvarchar (200) NULL ,
	"Photo" varbinary(max) NULL ,
	"PhotoPath" nvarchar (255) NULL ,
	"Hyperlink" nvarchar (255) NULL ,
	CONSTRAINT "PK_Promos" PRIMARY KEY CLUSTERED
	(
		"PromoId"
	),
	CONSTRAINT "FK_Promos_User" FOREIGN KEY 
	(
		"UserId"
	) REFERENCES "dbo"."Users" (
		"UserId"
	)
)
GO
GO
 CREATE  INDEX "ContactName" ON "dbo"."Promos"("ContactName")
GO
GO
 CREATE  INDEX "PromoType" ON "dbo"."Promos"("PromoType")
GO


CREATE TABLE "PromosCategories" (
	"PromoId" int NOT NULL ,
	"CategoryId" int NOT NULL,
	CONSTRAINT "PK_PromosCategories" PRIMARY KEY CLUSTERED 
	(
		"PromoId",
		"CategoryId"
	),
	CONSTRAINT "FK_PC_Promos" FOREIGN KEY 
	(
		"PromoId"
	) REFERENCES "dbo"."Promos" (
		"PromoId"
	),
	CONSTRAINT "FK_PC_Categories" FOREIGN KEY 
	(
		"CategoryId"
	) REFERENCES "dbo"."Categories" (
		"CategoryId"
	)
)
GO


/*Data insert*/

set quoted_identifier on
go
ALTER TABLE "Users" NOCHECK CONSTRAINT ALL
go
INSERT "Users" VALUES('Skywalker','Luke','Lukie Baby','12/08/2018','56 Tattooine Dr.','Mos Eiseley', 'NY','12209','lukeS@university.edu', NULL, '7658908765',NULL,NULL,NULL)
INSERT "Users" VALUES('Solo','Han','Hanso','12/20/2018','22 Millenium Dr.','Coruscant', 'FL','22409','hanso1@university.edu', NULL, '4563338765',NULL,NULL,NULL)
go
ALTER TABLE "Users" CHECK CONSTRAINT ALL
go

set quoted_identifier on
go
ALTER TABLE "Categories" NOCHECK CONSTRAINT ALL
go
INSERT "Categories" VALUES('Club Recruitment', 'For club recruitment promotions')
INSERT "Categories" VALUES('Peer Tutoring', 'Offer or request tutoring from other students')
INSERT "Categories" VALUES('Moving In', 'Post about items that you want')
INSERT "Categories" VALUES('Moving Out', 'Post items you want to get rid of')
INSERT "Categories" VALUES('Live Music', 'Promote live music events')
go
ALTER TABLE "Categories" CHECK CONSTRAINT ALL
go

set quoted_identifier on
go
ALTER TABLE "Promos" NOCHECK CONSTRAINT ALL
go
INSERT "Promos" VALUES(101,'Luke S','offer','06/10/2022','CS161 Tutoring Available','Happy to tutor anyone in CS161. Strong with Python. $15 per hour','lukeS@university.edu', NULL, NULL, NULL)
go
ALTER TABLE "Promos" CHECK CONSTRAINT ALL
go

set quoted_identifier on
go
ALTER TABLE "PromosCategories" NOCHECK CONSTRAINT ALL
go
INSERT "PromosCategories" VALUES(1, 2)
go
ALTER TABLE "PromosCategories" CHECK CONSTRAINT ALL
go