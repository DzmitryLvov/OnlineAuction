
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/07/2013 11:28:02
-- Generated from EDMX file: E:\Prog\OnlineAuction\OnlineAuction\Buisness\Data\DataBaseModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OnlineAuctionDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Users__RoleID__1B0907CE]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK__Users__RoleID__1B0907CE];
GO
IF OBJECT_ID(N'[dbo].[FK_LotTypeLot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lots] DROP CONSTRAINT [FK_LotTypeLot];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Lots]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lots];
GO
IF OBJECT_ID(N'[dbo].[LotTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LotTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Rolename] nvarchar(255)  NOT NULL,
    [IsDeleted] bit  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(255)  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Comment] nvarchar(255)  NOT NULL,
    [Password] nvarchar(128)  NOT NULL,
    [PasswordQuestion] nvarchar(255)  NULL,
    [PasswordAnswer] nvarchar(255)  NULL,
    [IsApproved] bit  NOT NULL,
    [LastActivityDate] datetime2(7)  NOT NULL,
    [LastLoginDate] datetime2(7)  NOT NULL,
    [LastPasswordChangedDate] datetime2(7)  NOT NULL,
    [CreationDate] datetime2(7)  NOT NULL,
    [IsOnLine] bit  NULL,
    [IsLockedOut] bit  NOT NULL,
    [LastLockedOutDate] datetime2(7)  NOT NULL,
    [FailedPasswordAttemptCount] int  NOT NULL,
    [FailedPasswordAttemptWindowStart] datetime2(7)  NOT NULL,
    [FailedPasswordAnswerAttemptCount] int  NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] datetime2(7)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Location] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Role_ID] int  NULL
);
GO

-- Creating table 'Lots'
CREATE TABLE [dbo].[Lots] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Lotname] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Currency] bigint  NOT NULL,
    [ActualDate] datetime2(7)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [LeaderName] nchar(50)  NULL,
    [OwnerName] nvarchar(50)  NOT NULL,
    [TypeId] int  NOT NULL
);
GO

-- Creating table 'LotTypes'
CREATE TABLE [dbo].[LotTypes] (
    [TypeID] int IDENTITY(1,1) NOT NULL,
    [TypeName] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [PK_Lots]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [TypeID] in table 'LotTypes'
ALTER TABLE [dbo].[LotTypes]
ADD CONSTRAINT [PK_LotTypes]
    PRIMARY KEY CLUSTERED ([TypeID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Role_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK__Users__RoleID__1B0907CE]
    FOREIGN KEY ([Role_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__Users__RoleID__1B0907CE'
CREATE INDEX [IX_FK__Users__RoleID__1B0907CE]
ON [dbo].[Users]
    ([Role_ID]);
GO

-- Creating foreign key on [TypeId] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_LotTypeLot]
    FOREIGN KEY ([TypeId])
    REFERENCES [dbo].[LotTypes]
        ([TypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotTypeLot'
CREATE INDEX [IX_FK_LotTypeLot]
ON [dbo].[Lots]
    ([TypeId]);
GO

INSERT INTO Roles (Rolename, IsDeleted) VALUES ("admin","false")
INSERT INTO Roles (Rolename, IsDeleted) VALUES ("user","false")

INSERT INTO LotTypes (TypeName) VALUES ("Cars")
INSERT INTO LotTypes (TypeName) VALUES ("Computers")
INSERT INTO LotTypes (TypeName) VALUES ("Mobile phones")
INSERT INTO LotTypes (TypeName) VALUES ("Pets")
INSERT INTO LotTypes (TypeName) VALUES ("Other")
-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------