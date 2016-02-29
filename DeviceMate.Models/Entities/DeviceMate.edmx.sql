
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/09/2013 13:06:46
-- Generated from EDMX file: D:\Projects\DeviceMate\DeviceMate.Objects\Entities\DeviceMate.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DeviceMate];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_DeviceColor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_DeviceColor];
GO
IF OBJECT_ID(N'[dbo].[FK_DeviceDeviceType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_DeviceDeviceType];
GO
IF OBJECT_ID(N'[dbo].[FK_DeviceOSDeviceType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeviceOS] DROP CONSTRAINT [FK_DeviceOSDeviceType];
GO
IF OBJECT_ID(N'[dbo].[FK_StatusAdminUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AdminUsers] DROP CONSTRAINT [FK_StatusAdminUser];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamDeviceHold]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DeviceHold] DROP CONSTRAINT [FK_TeamDeviceHold];
GO
IF OBJECT_ID(N'[dbo].[FK_DeviceOSDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_DeviceOSDevice];
GO
IF OBJECT_ID(N'[dbo].[FK_DeviceHoldDevice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_DeviceHoldDevice];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Devices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Devices];
GO
IF OBJECT_ID(N'[dbo].[Colors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Colors];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[AdminUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdminUsers];
GO
IF OBJECT_ID(N'[dbo].[Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Status];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[DeviceOS]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeviceOS];
GO
IF OBJECT_ID(N'[dbo].[DeviceType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeviceType];
GO
IF OBJECT_ID(N'[dbo].[DeviceHold]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeviceHold];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Devices'
CREATE TABLE [dbo].[Devices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] nvarchar(20)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [ColorId] int  NULL,
    [DeviceTypeId] int  NOT NULL,
    [DeviceOSId] int  NULL,
    [DeviceHoldId] int  NOT NULL
);
GO

-- Creating table 'Colors'
CREATE TABLE [dbo].[Colors] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'AdminUsers'
CREATE TABLE [dbo].[AdminUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(100)  NULL,
    [Email] nvarchar(100)  NOT NULL,
    [StatusId] int  NOT NULL,
    [ModifiedDate] datetime  NOT NULL,
    [TeamId] int  NOT NULL
);
GO

-- Creating table 'Status'
CREATE TABLE [dbo].[Status] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [ApplicationName] nvarchar(20)  NOT NULL
);
GO

-- Creating table 'DeviceOS'
CREATE TABLE [dbo].[DeviceOS] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Version] nvarchar(50)  NOT NULL,
    [DeviceTypeId] int  NOT NULL
);
GO

-- Creating table 'DeviceType'
CREATE TABLE [dbo].[DeviceType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DeviceHold'
CREATE TABLE [dbo].[DeviceHold] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HoldDate] datetime  NOT NULL,
    [TeamId] int  NOT NULL,
    [Email] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [PK_Devices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [PK_Colors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdminUsers'
ALTER TABLE [dbo].[AdminUsers]
ADD CONSTRAINT [PK_AdminUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Status'
ALTER TABLE [dbo].[Status]
ADD CONSTRAINT [PK_Status]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeviceOS'
ALTER TABLE [dbo].[DeviceOS]
ADD CONSTRAINT [PK_DeviceOS]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeviceType'
ALTER TABLE [dbo].[DeviceType]
ADD CONSTRAINT [PK_DeviceType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeviceHold'
ALTER TABLE [dbo].[DeviceHold]
ADD CONSTRAINT [PK_DeviceHold]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ColorId] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [FK_DeviceColor]
    FOREIGN KEY ([ColorId])
    REFERENCES [dbo].[Colors]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceColor'
CREATE INDEX [IX_FK_DeviceColor]
ON [dbo].[Devices]
    ([ColorId]);
GO

-- Creating foreign key on [DeviceTypeId] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [FK_DeviceDeviceType]
    FOREIGN KEY ([DeviceTypeId])
    REFERENCES [dbo].[DeviceType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceDeviceType'
CREATE INDEX [IX_FK_DeviceDeviceType]
ON [dbo].[Devices]
    ([DeviceTypeId]);
GO

-- Creating foreign key on [DeviceTypeId] in table 'DeviceOS'
ALTER TABLE [dbo].[DeviceOS]
ADD CONSTRAINT [FK_DeviceOSDeviceType]
    FOREIGN KEY ([DeviceTypeId])
    REFERENCES [dbo].[DeviceType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceOSDeviceType'
CREATE INDEX [IX_FK_DeviceOSDeviceType]
ON [dbo].[DeviceOS]
    ([DeviceTypeId]);
GO

-- Creating foreign key on [StatusId] in table 'AdminUsers'
ALTER TABLE [dbo].[AdminUsers]
ADD CONSTRAINT [FK_StatusAdminUser]
    FOREIGN KEY ([StatusId])
    REFERENCES [dbo].[Status]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StatusAdminUser'
CREATE INDEX [IX_FK_StatusAdminUser]
ON [dbo].[AdminUsers]
    ([StatusId]);
GO

-- Creating foreign key on [TeamId] in table 'DeviceHold'
ALTER TABLE [dbo].[DeviceHold]
ADD CONSTRAINT [FK_TeamDeviceHold]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamDeviceHold'
CREATE INDEX [IX_FK_TeamDeviceHold]
ON [dbo].[DeviceHold]
    ([TeamId]);
GO

-- Creating foreign key on [DeviceOSId] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [FK_DeviceOSDevice]
    FOREIGN KEY ([DeviceOSId])
    REFERENCES [dbo].[DeviceOS]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceOSDevice'
CREATE INDEX [IX_FK_DeviceOSDevice]
ON [dbo].[Devices]
    ([DeviceOSId]);
GO

-- Creating foreign key on [DeviceHoldId] in table 'Devices'
ALTER TABLE [dbo].[Devices]
ADD CONSTRAINT [FK_DeviceHoldDevice]
    FOREIGN KEY ([DeviceHoldId])
    REFERENCES [dbo].[DeviceHold]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DeviceHoldDevice'
CREATE INDEX [IX_FK_DeviceHoldDevice]
ON [dbo].[Devices]
    ([DeviceHoldId]);
GO

-- Creating foreign key on [TeamId] in table 'AdminUsers'
ALTER TABLE [dbo].[AdminUsers]
ADD CONSTRAINT [FK_TeamAdminUser]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamAdminUser'
CREATE INDEX [IX_FK_TeamAdminUser]
ON [dbo].[AdminUsers]
    ([TeamId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------