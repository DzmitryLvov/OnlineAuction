
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/07/2013 23:01:25
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
IF OBJECT_ID(N'[dbo].[FK_CategorySubCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubCategories] DROP CONSTRAINT [FK_CategorySubCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_SubCategoryLot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lots] DROP CONSTRAINT [FK_SubCategoryLot];
GO
IF OBJECT_ID(N'[dbo].[FK_UserBookmark]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookmarks] DROP CONSTRAINT [FK_UserBookmark];
GO
IF OBJECT_ID(N'[dbo].[FK_LotBookmark]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookmarks] DROP CONSTRAINT [FK_LotBookmark];
GO
IF OBJECT_ID(N'[dbo].[FK_UserLot]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lots] DROP CONSTRAINT [FK_UserLot];
GO
IF OBJECT_ID(N'[dbo].[FK_UserBet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bets] DROP CONSTRAINT [FK_UserBet];
GO
IF OBJECT_ID(N'[dbo].[FK_LotBet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bets] DROP CONSTRAINT [FK_LotBet];
GO
IF OBJECT_ID(N'[dbo].[FK_LotLotPhoto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LotPhotos] DROP CONSTRAINT [FK_LotLotPhoto];
GO
IF OBJECT_ID(N'[dbo].[FK_UserComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_UserComment];
GO
IF OBJECT_ID(N'[dbo].[FK_LotComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_LotComment];
GO
IF OBJECT_ID(N'[dbo].[FK_LocationUserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserDatas] DROP CONSTRAINT [FK_LocationUserData];
GO
IF OBJECT_ID(N'[dbo].[FK_UserUserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserUserData];
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
IF OBJECT_ID(N'[dbo].[UserDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDatas];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[SubCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubCategories];
GO
IF OBJECT_ID(N'[dbo].[Bookmarks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookmarks];
GO
IF OBJECT_ID(N'[dbo].[Bets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bets];
GO
IF OBJECT_ID(N'[dbo].[LotPhotos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LotPhotos];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Locations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Locations];
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
    [LastActivityDate] datetime  NOT NULL,
    [LastLoginDate] datetime  NOT NULL,
    [LastPasswordChangedDate] datetime  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [IsOnLine] bit  NULL,
    [IsLockedOut] bit  NOT NULL,
    [LastLockedOutDate] datetime  NOT NULL,
    [FailedPasswordAttemptCount] int  NOT NULL,
    [FailedPasswordAttemptWindowStart] datetime  NOT NULL,
    [FailedPasswordAnswerAttemptCount] int  NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [Role_ID] int  NULL
);
GO

-- Creating table 'Lots'
CREATE TABLE [dbo].[Lots] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LotName] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [StartCurrency] bigint  NOT NULL,
    [ActualDate] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [SubCategoryID] int  NOT NULL,
    [OwnerId] int  NOT NULL,
    [ViewCount] nvarchar(max)  NOT NULL,
    [LotTypeID] int  NOT NULL
);
GO

-- Creating table 'UserDatas'
CREATE TABLE [dbo].[UserDatas] (
    [Phone] nvarchar(max)  NULL,
    [FirstName] nvarchar(max)  NULL,
    [LastName] nvarchar(max)  NULL,
    [LocationID] int  NOT NULL,
    [BrithDate] datetime  NULL,
    [PhotoLink] nvarchar(max)  NULL,
    [ID] int  NOT NULL,
    [User_ID] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubCategories'
CREATE TABLE [dbo].[SubCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NOT NULL,
    [SubCategoryName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Bookmarks'
CREATE TABLE [dbo].[Bookmarks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [LotID] int  NOT NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'Bets'
CREATE TABLE [dbo].[Bets] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [LotID] int  NOT NULL,
    [BetValue] int  NOT NULL,
    [BetDate] datetime  NOT NULL
);
GO

-- Creating table 'LotPhotos'
CREATE TABLE [dbo].[LotPhotos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LotID] int  NOT NULL,
    [PhotoLink] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [LotID] int  NOT NULL,
    [CommentText] nvarchar(max)  NOT NULL,
    [CommentDate] datetime  NOT NULL
);
GO

-- Creating table 'Locations'
CREATE TABLE [dbo].[Locations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [LocationName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LotTypes'
CREATE TABLE [dbo].[LotTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
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

-- Creating primary key on [ID] in table 'UserDatas'
ALTER TABLE [dbo].[UserDatas]
ADD CONSTRAINT [PK_UserDatas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [PK_SubCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Bookmarks'
ALTER TABLE [dbo].[Bookmarks]
ADD CONSTRAINT [PK_Bookmarks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Bets'
ALTER TABLE [dbo].[Bets]
ADD CONSTRAINT [PK_Bets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'LotPhotos'
ALTER TABLE [dbo].[LotPhotos]
ADD CONSTRAINT [PK_LotPhotos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Locations'
ALTER TABLE [dbo].[Locations]
ADD CONSTRAINT [PK_Locations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'LotTypes'
ALTER TABLE [dbo].[LotTypes]
ADD CONSTRAINT [PK_LotTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
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

-- Creating foreign key on [CategoryID] in table 'SubCategories'
ALTER TABLE [dbo].[SubCategories]
ADD CONSTRAINT [FK_CategorySubCategory]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategorySubCategory'
CREATE INDEX [IX_FK_CategorySubCategory]
ON [dbo].[SubCategories]
    ([CategoryID]);
GO

-- Creating foreign key on [SubCategoryID] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_SubCategoryLot]
    FOREIGN KEY ([SubCategoryID])
    REFERENCES [dbo].[SubCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SubCategoryLot'
CREATE INDEX [IX_FK_SubCategoryLot]
ON [dbo].[Lots]
    ([SubCategoryID]);
GO

-- Creating foreign key on [UserID] in table 'Bookmarks'
ALTER TABLE [dbo].[Bookmarks]
ADD CONSTRAINT [FK_UserBookmark]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserBookmark'
CREATE INDEX [IX_FK_UserBookmark]
ON [dbo].[Bookmarks]
    ([UserID]);
GO

-- Creating foreign key on [LotID] in table 'Bookmarks'
ALTER TABLE [dbo].[Bookmarks]
ADD CONSTRAINT [FK_LotBookmark]
    FOREIGN KEY ([LotID])
    REFERENCES [dbo].[Lots]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotBookmark'
CREATE INDEX [IX_FK_LotBookmark]
ON [dbo].[Bookmarks]
    ([LotID]);
GO

-- Creating foreign key on [OwnerId] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_UserLot]
    FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLot'
CREATE INDEX [IX_FK_UserLot]
ON [dbo].[Lots]
    ([OwnerId]);
GO

-- Creating foreign key on [UserID] in table 'Bets'
ALTER TABLE [dbo].[Bets]
ADD CONSTRAINT [FK_UserBet]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserBet'
CREATE INDEX [IX_FK_UserBet]
ON [dbo].[Bets]
    ([UserID]);
GO

-- Creating foreign key on [LotID] in table 'Bets'
ALTER TABLE [dbo].[Bets]
ADD CONSTRAINT [FK_LotBet]
    FOREIGN KEY ([LotID])
    REFERENCES [dbo].[Lots]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotBet'
CREATE INDEX [IX_FK_LotBet]
ON [dbo].[Bets]
    ([LotID]);
GO

-- Creating foreign key on [LotID] in table 'LotPhotos'
ALTER TABLE [dbo].[LotPhotos]
ADD CONSTRAINT [FK_LotLotPhoto]
    FOREIGN KEY ([LotID])
    REFERENCES [dbo].[Lots]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotLotPhoto'
CREATE INDEX [IX_FK_LotLotPhoto]
ON [dbo].[LotPhotos]
    ([LotID]);
GO

-- Creating foreign key on [UserID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_UserComment]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserComment'
CREATE INDEX [IX_FK_UserComment]
ON [dbo].[Comments]
    ([UserID]);
GO

-- Creating foreign key on [LotID] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_LotComment]
    FOREIGN KEY ([LotID])
    REFERENCES [dbo].[Lots]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotComment'
CREATE INDEX [IX_FK_LotComment]
ON [dbo].[Comments]
    ([LotID]);
GO

-- Creating foreign key on [LocationID] in table 'UserDatas'
ALTER TABLE [dbo].[UserDatas]
ADD CONSTRAINT [FK_LocationUserData]
    FOREIGN KEY ([LocationID])
    REFERENCES [dbo].[Locations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LocationUserData'
CREATE INDEX [IX_FK_LocationUserData]
ON [dbo].[UserDatas]
    ([LocationID]);
GO

-- Creating foreign key on [User_ID] in table 'UserDatas'
ALTER TABLE [dbo].[UserDatas]
ADD CONSTRAINT [FK_UserUserData]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserData'
CREATE INDEX [IX_FK_UserUserData]
ON [dbo].[UserDatas]
    ([User_ID]);
GO

-- Creating foreign key on [LotTypeID] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_LotTypeLot]
    FOREIGN KEY ([LotTypeID])
    REFERENCES [dbo].[LotTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LotTypeLot'
CREATE INDEX [IX_FK_LotTypeLot]
ON [dbo].[Lots]
    ([LotTypeID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------