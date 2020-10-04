
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/15/2020 19:28:23
-- Generated from EDMX file: C:\Users\tonym\Documents\Monash\WorkSpaces\VisualStudioWS\PlayMore-V2.0\PlayMore-V2.0\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-PlayMore-V2.0-20200915071304];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CoachGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Coaches1] DROP CONSTRAINT [FK_CoachGame];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkshopGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Workshops1] DROP CONSTRAINT [FK_WorkshopGame];
GO
IF OBJECT_ID(N'[dbo].[FK_CoachWorkshop]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Workshops1] DROP CONSTRAINT [FK_CoachWorkshop];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkshopBooking]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Bookings] DROP CONSTRAINT [FK_WorkshopBooking];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Coaches1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Coaches1];
GO
IF OBJECT_ID(N'[dbo].[Games]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Games];
GO
IF OBJECT_ID(N'[dbo].[Workshops1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Workshops1];
GO
IF OBJECT_ID(N'[dbo].[Bookings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Bookings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Coaches1'
CREATE TABLE [dbo].[Coaches1] (
    [CoachId] int IDENTITY(1,1) NOT NULL,
    [CoachName] nvarchar(max)  NOT NULL,
    [CoachEmail] nvarchar(max)  NOT NULL,
    [GameGameId] int  NOT NULL
);
GO

-- Creating table 'Games'
CREATE TABLE [dbo].[Games] (
    [GameId] int IDENTITY(1,1) NOT NULL,
    [GameName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Workshops1'
CREATE TABLE [dbo].[Workshops1] (
    [WorkshopId] int IDENTITY(1,1) NOT NULL,
    [WorkshopDate] datetime  NOT NULL,
    [WorkshopLocation] nvarchar(max)  NOT NULL,
    [GameGameId] int  NOT NULL,
    [CoachCoachId] int  NOT NULL
);
GO

-- Creating table 'Bookings'
CREATE TABLE [dbo].[Bookings] (
    [BookId] int IDENTITY(1,1) NOT NULL,
    [BookedBy_Userid] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [WorkshopWorkshopId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CoachId] in table 'Coaches1'
ALTER TABLE [dbo].[Coaches1]
ADD CONSTRAINT [PK_Coaches1]
    PRIMARY KEY CLUSTERED ([CoachId] ASC);
GO

-- Creating primary key on [GameId] in table 'Games'
ALTER TABLE [dbo].[Games]
ADD CONSTRAINT [PK_Games]
    PRIMARY KEY CLUSTERED ([GameId] ASC);
GO

-- Creating primary key on [WorkshopId] in table 'Workshops1'
ALTER TABLE [dbo].[Workshops1]
ADD CONSTRAINT [PK_Workshops1]
    PRIMARY KEY CLUSTERED ([WorkshopId] ASC);
GO

-- Creating primary key on [BookId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [PK_Bookings]
    PRIMARY KEY CLUSTERED ([BookId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GameGameId] in table 'Coaches1'
ALTER TABLE [dbo].[Coaches1]
ADD CONSTRAINT [FK_CoachGame]
    FOREIGN KEY ([GameGameId])
    REFERENCES [dbo].[Games]
        ([GameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachGame'
CREATE INDEX [IX_FK_CoachGame]
ON [dbo].[Coaches1]
    ([GameGameId]);
GO

-- Creating foreign key on [GameGameId] in table 'Workshops1'
ALTER TABLE [dbo].[Workshops1]
ADD CONSTRAINT [FK_WorkshopGame]
    FOREIGN KEY ([GameGameId])
    REFERENCES [dbo].[Games]
        ([GameId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkshopGame'
CREATE INDEX [IX_FK_WorkshopGame]
ON [dbo].[Workshops1]
    ([GameGameId]);
GO

-- Creating foreign key on [CoachCoachId] in table 'Workshops1'
ALTER TABLE [dbo].[Workshops1]
ADD CONSTRAINT [FK_CoachWorkshop]
    FOREIGN KEY ([CoachCoachId])
    REFERENCES [dbo].[Coaches1]
        ([CoachId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CoachWorkshop'
CREATE INDEX [IX_FK_CoachWorkshop]
ON [dbo].[Workshops1]
    ([CoachCoachId]);
GO

-- Creating foreign key on [WorkshopWorkshopId] in table 'Bookings'
ALTER TABLE [dbo].[Bookings]
ADD CONSTRAINT [FK_WorkshopBooking]
    FOREIGN KEY ([WorkshopWorkshopId])
    REFERENCES [dbo].[Workshops1]
        ([WorkshopId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkshopBooking'
CREATE INDEX [IX_FK_WorkshopBooking]
ON [dbo].[Bookings]
    ([WorkshopWorkshopId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------