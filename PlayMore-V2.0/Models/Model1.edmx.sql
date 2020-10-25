
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/21/2020 23:13:45
-- Generated from EDMX file: C:\Users\tonym\Documents\Monash\WorkSpaces\VisualStudioWS\PlayMore-V5.0\PlayMore-V5.0\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-PlayMore-V5.0-20201018105414];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CoachWorkshop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Workshops] DROP CONSTRAINT [FK_CoachWorkshop];
GO
IF OBJECT_ID(N'[dbo].[FK_BookingWorkshop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_BookingWorkshop];
GO
IF OBJECT_ID(N'[dbo].[FK_GameWorkshop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Workshops] DROP CONSTRAINT [FK_GameWorkshop];
GO
IF OBJECT_ID(N'[dbo].[FK_CoachGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coaches] DROP CONSTRAINT [FK_CoachGame];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Coaches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coaches];
GO
IF OBJECT_ID(N'[dbo].[Workshops]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Workshops];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Coaches'
CREATE TABLE [dbo].[Coaches] (
    [CoachId] int IDENTITY(1,1) NOT NULL,
    [CoachFName] nvarchar(max)  NOT NULL,
    [CoachLName] nvarchar(max)  NOT NULL,
    [CoachEmail] nvarchar(max)  NOT NULL,
    [GameGameId] int  NOT NULL
);
GO

-- Creating table 'Workshops'
CREATE TABLE [dbo].[Workshops] (
    [WorkshopId] int IDENTITY(1,1) NOT NULL,
    [WorkshopDate] datetime  NOT NULL,
    [WorkshopLocation] nvarchar(max)  NOT NULL,
    [WSLocLattitude] nvarchar(max)  NOT NULL,
    [WSLocLongitude] nvarchar(max)  NOT NULL,
    [WorkShopFees] nvarchar(max)  NOT NULL,
    [CoachCoachId] int  NOT NULL,
    [GameGameId] int  NOT NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [GameId] int IDENTITY(1,1) NOT NULL,
    [GameName] nvarchar(max)  NOT NULL,
    [GameDescription] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [BookId] int IDENTITY(1,1) NOT NULL,
    [BookedBy_Userid] nvarchar(max)  NOT NULL,
    [BookFName] nvarchar(max)  NOT NULL,
    [BookLName] nvarchar(max)  NOT NULL,
    [BookAge] nvarchar(max)  NOT NULL,
    [WorkshopWorkshopId] int  NOT NULL
);
GO

-- Creating table 'Ratings'
CREATE TABLE [dbo].[Ratings] (
    [RatingId] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [RatingGiven] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CoachId] in table 'Coaches'
ALTER TABLE [dbo].[Coaches]
ADD CONSTRAINT [PK_Coaches]
    PRIMARY KEY CLUSTERED ([CoachId] ASC);
GO

-- Creating primary key on [WorkshopId] in table 'Workshops'
ALTER TABLE [dbo].[Workshops]
ADD CONSTRAINT [PK_Workshops]
    PRIMARY KEY CLUSTERED ([WorkshopId] ASC);
GO

-- Creating primary key on [GameId] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([GameId] ASC);
GO

-- Creating primary key on [BookId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([BookId] ASC);
GO

-- Creating primary key on [RatingId] in table 'Ratings'
ALTER TABLE [dbo].[Ratings]
ADD CONSTRAINT [PK_Ratings]
    PRIMARY KEY CLUSTERED ([RatingId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CoachCoachId] in table 'Workshops'
ALTER TABLE [dbo].[Workshops]
ADD CONSTRAINT [FK_CoachWorkshop]
    FOREIGN KEY ([CoachCoachId])
    REFERENCES [dbo].[Coaches]
        ([CoachId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachWorkshop'
CREATE INDEX [IX_FK_CoachWorkshop]
ON [dbo].[Workshops]
    ([CoachCoachId]);
GO

-- Creating foreign key on [WorkshopWorkshopId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_BookingWorkshop]
    FOREIGN KEY ([WorkshopWorkshopId])
    REFERENCES [dbo].[Workshops]
        ([WorkshopId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BookingWorkshop'
CREATE INDEX [IX_FK_BookingWorkshop]
ON [dbo].[Bookings]
    ([WorkshopWorkshopId]);
GO

-- Creating foreign key on [GameGameId] in table 'Workshops'
ALTER TABLE [dbo].[Workshops]
ADD CONSTRAINT [FK_GameWorkshop]
    FOREIGN KEY ([GameGameId])
    REFERENCES [dbo].[Games]
        ([GameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GameWorkshop'
CREATE INDEX [IX_FK_GameWorkshop]
ON [dbo].[Workshops]
    ([GameGameId]);
GO

-- Creating foreign key on [GameGameId] in table 'Coaches'
ALTER TABLE [dbo].[Coaches]
ADD CONSTRAINT [FK_CoachGame]
    FOREIGN KEY ([GameGameId])
    REFERENCES [dbo].[Games]
        ([GameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachGame'
CREATE INDEX [IX_FK_CoachGame]
ON [dbo].[Coaches]
    ([GameGameId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------