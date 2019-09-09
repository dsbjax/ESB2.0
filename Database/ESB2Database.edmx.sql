
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server Compact Edition
-- --------------------------------------------------
-- Date Created: 08/30/2019 11:28:37
-- Generated from EDMX file: C:\Users\Dan\OneDrive\GitHub\VS2017\ESB2\Database\ESB2Database.edmx
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- NOTE: if the constraint does not exist, an ignorable error will be reported.
-- --------------------------------------------------

    ALTER TABLE [UserLog] DROP CONSTRAINT [FK_UserLogEventUser];
GO
    ALTER TABLE [EquipmentGroupings] DROP CONSTRAINT [FK_EquipmentSystemEquipmentGrouping];
GO
    ALTER TABLE [EquipmentListing] DROP CONSTRAINT [FK_EquipmentGroupingEquipment];
GO
    ALTER TABLE [Outages] DROP CONSTRAINT [FK_OutageUser];
GO
    ALTER TABLE [OutageEquipment] DROP CONSTRAINT [FK_OutageEquipment_Outage];
GO
    ALTER TABLE [OutageEquipment] DROP CONSTRAINT [FK_OutageEquipment_Equipment];
GO
    ALTER TABLE [OutageUpdates] DROP CONSTRAINT [FK_OutageOutageUpdate];
GO
    ALTER TABLE [OutageUpdates] DROP CONSTRAINT [FK_OutageUpdateUser];
GO
    ALTER TABLE [StatusPageGroupingEquipment] DROP CONSTRAINT [FK_StatusPageGroupingEquipment_StatusPageGrouping];
GO
    ALTER TABLE [StatusPageGroupingEquipment] DROP CONSTRAINT [FK_StatusPageGroupingEquipment_Equipment];
GO
    ALTER TABLE [StatusPageGroupings] DROP CONSTRAINT [FK_StatusPageStatusPageGrouping];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- NOTE: if the table does not exist, an ignorable error will be reported.
-- --------------------------------------------------

    DROP TABLE [Users];
GO
    DROP TABLE [UserLog];
GO
    DROP TABLE [EquipmentSystems];
GO
    DROP TABLE [EquipmentGroupings];
GO
    DROP TABLE [EquipmentListing];
GO
    DROP TABLE [Outages];
GO
    DROP TABLE [OutageUpdates];
GO
    DROP TABLE [StatusPageGroupings];
GO
    DROP TABLE [StatusPages];
GO
    DROP TABLE [OutageEquipment];
GO
    DROP TABLE [StatusPageGroupingEquipment];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(4000)  NOT NULL,
    [Lastname] nvarchar(4000)  NOT NULL,
    [Firstname] nvarchar(4000)  NOT NULL,
    [Password] varbinary(8000)  NOT NULL,
    [UserPermission] int  NOT NULL,
    [AccountStatus] int  NOT NULL,
    [AccountLocked] datetime  NOT NULL,
    [FailedLoginCounter] int  NOT NULL
);
GO

-- Creating table 'UserLog'
CREATE TABLE [UserLog] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Timestamp] datetime  NOT NULL,
    [UserEvent] int  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'EquipmentSystems'
CREATE TABLE [EquipmentSystems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nomenclature] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL
);
GO

-- Creating table 'EquipmentGroupings'
CREATE TABLE [EquipmentGroupings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [EquipmentSystemId] int  NOT NULL
);
GO

-- Creating table 'EquipmentListing'
CREATE TABLE [EquipmentListing] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nomenclature] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [EquipmentGroupingId] int  NOT NULL,
    [EquipmentStatus] int  NOT NULL
);
GO

-- Creating table 'Outages'
CREATE TABLE [Outages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [OutageType] int  NOT NULL,
    [Completed] bit  NOT NULL,
    [Canceled] bit  NOT NULL,
    [Start] datetime  NOT NULL,
    [End] datetime  NULL,
    [CreatedBy_Id] int  NOT NULL
);
GO

-- Creating table 'OutageUpdates'
CREATE TABLE [OutageUpdates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Timestamp] datetime  NOT NULL,
    [Update] nvarchar(4000)  NOT NULL,
    [OutageId] int  NOT NULL,
    [UpdateBy_Id] int  NOT NULL
);
GO

-- Creating table 'StatusPageGroupings'
CREATE TABLE [StatusPageGroupings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsStatusBar] bit  NOT NULL,
    [Title] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [StatusPageId] int  NOT NULL,
    [Index] int  NOT NULL
);
GO

-- Creating table 'StatusPages'
CREATE TABLE [StatusPages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(4000)  NOT NULL,
    [Description] nvarchar(4000)  NOT NULL,
    [BackgroundImage] nvarchar(4000)  NOT NULL,
    [IsDisplayed] bit  NOT NULL,
    [Index] int  NOT NULL
);
GO

-- Creating table 'OutageEquipment'
CREATE TABLE [OutageEquipment] (
    [OutageEquipment_Equipment_Id] int  NOT NULL,
    [Equipment_Id] int  NOT NULL
);
GO

-- Creating table 'StatusPageGroupingEquipment'
CREATE TABLE [StatusPageGroupingEquipment] (
    [StatusPageGroupingEquipment_Equipment_Id] int  NOT NULL,
    [Equipment_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'UserLog'
ALTER TABLE [UserLog]
ADD CONSTRAINT [PK_UserLog]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'EquipmentSystems'
ALTER TABLE [EquipmentSystems]
ADD CONSTRAINT [PK_EquipmentSystems]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'EquipmentGroupings'
ALTER TABLE [EquipmentGroupings]
ADD CONSTRAINT [PK_EquipmentGroupings]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'EquipmentListing'
ALTER TABLE [EquipmentListing]
ADD CONSTRAINT [PK_EquipmentListing]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'Outages'
ALTER TABLE [Outages]
ADD CONSTRAINT [PK_Outages]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'OutageUpdates'
ALTER TABLE [OutageUpdates]
ADD CONSTRAINT [PK_OutageUpdates]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'StatusPageGroupings'
ALTER TABLE [StatusPageGroupings]
ADD CONSTRAINT [PK_StatusPageGroupings]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [Id] in table 'StatusPages'
ALTER TABLE [StatusPages]
ADD CONSTRAINT [PK_StatusPages]
    PRIMARY KEY ([Id] );
GO

-- Creating primary key on [OutageEquipment_Equipment_Id], [Equipment_Id] in table 'OutageEquipment'
ALTER TABLE [OutageEquipment]
ADD CONSTRAINT [PK_OutageEquipment]
    PRIMARY KEY ([OutageEquipment_Equipment_Id], [Equipment_Id] );
GO

-- Creating primary key on [StatusPageGroupingEquipment_Equipment_Id], [Equipment_Id] in table 'StatusPageGroupingEquipment'
ALTER TABLE [StatusPageGroupingEquipment]
ADD CONSTRAINT [PK_StatusPageGroupingEquipment]
    PRIMARY KEY ([StatusPageGroupingEquipment_Equipment_Id], [Equipment_Id] );
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'UserLog'
ALTER TABLE [UserLog]
ADD CONSTRAINT [FK_UserLogEventUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserLogEventUser'
CREATE INDEX [IX_FK_UserLogEventUser]
ON [UserLog]
    ([User_Id]);
GO

-- Creating foreign key on [EquipmentSystemId] in table 'EquipmentGroupings'
ALTER TABLE [EquipmentGroupings]
ADD CONSTRAINT [FK_EquipmentSystemEquipmentGrouping]
    FOREIGN KEY ([EquipmentSystemId])
    REFERENCES [EquipmentSystems]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentSystemEquipmentGrouping'
CREATE INDEX [IX_FK_EquipmentSystemEquipmentGrouping]
ON [EquipmentGroupings]
    ([EquipmentSystemId]);
GO

-- Creating foreign key on [EquipmentGroupingId] in table 'EquipmentListing'
ALTER TABLE [EquipmentListing]
ADD CONSTRAINT [FK_EquipmentGroupingEquipment]
    FOREIGN KEY ([EquipmentGroupingId])
    REFERENCES [EquipmentGroupings]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentGroupingEquipment'
CREATE INDEX [IX_FK_EquipmentGroupingEquipment]
ON [EquipmentListing]
    ([EquipmentGroupingId]);
GO

-- Creating foreign key on [CreatedBy_Id] in table 'Outages'
ALTER TABLE [Outages]
ADD CONSTRAINT [FK_OutageUser]
    FOREIGN KEY ([CreatedBy_Id])
    REFERENCES [Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutageUser'
CREATE INDEX [IX_FK_OutageUser]
ON [Outages]
    ([CreatedBy_Id]);
GO

-- Creating foreign key on [OutageEquipment_Equipment_Id] in table 'OutageEquipment'
ALTER TABLE [OutageEquipment]
ADD CONSTRAINT [FK_OutageEquipment_Outage]
    FOREIGN KEY ([OutageEquipment_Equipment_Id])
    REFERENCES [Outages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Equipment_Id] in table 'OutageEquipment'
ALTER TABLE [OutageEquipment]
ADD CONSTRAINT [FK_OutageEquipment_Equipment]
    FOREIGN KEY ([Equipment_Id])
    REFERENCES [EquipmentListing]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutageEquipment_Equipment'
CREATE INDEX [IX_FK_OutageEquipment_Equipment]
ON [OutageEquipment]
    ([Equipment_Id]);
GO

-- Creating foreign key on [OutageId] in table 'OutageUpdates'
ALTER TABLE [OutageUpdates]
ADD CONSTRAINT [FK_OutageOutageUpdate]
    FOREIGN KEY ([OutageId])
    REFERENCES [Outages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutageOutageUpdate'
CREATE INDEX [IX_FK_OutageOutageUpdate]
ON [OutageUpdates]
    ([OutageId]);
GO

-- Creating foreign key on [UpdateBy_Id] in table 'OutageUpdates'
ALTER TABLE [OutageUpdates]
ADD CONSTRAINT [FK_OutageUpdateUser]
    FOREIGN KEY ([UpdateBy_Id])
    REFERENCES [Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OutageUpdateUser'
CREATE INDEX [IX_FK_OutageUpdateUser]
ON [OutageUpdates]
    ([UpdateBy_Id]);
GO

-- Creating foreign key on [StatusPageGroupingEquipment_Equipment_Id] in table 'StatusPageGroupingEquipment'
ALTER TABLE [StatusPageGroupingEquipment]
ADD CONSTRAINT [FK_StatusPageGroupingEquipment_StatusPageGrouping]
    FOREIGN KEY ([StatusPageGroupingEquipment_Equipment_Id])
    REFERENCES [StatusPageGroupings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Equipment_Id] in table 'StatusPageGroupingEquipment'
ALTER TABLE [StatusPageGroupingEquipment]
ADD CONSTRAINT [FK_StatusPageGroupingEquipment_Equipment]
    FOREIGN KEY ([Equipment_Id])
    REFERENCES [EquipmentListing]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StatusPageGroupingEquipment_Equipment'
CREATE INDEX [IX_FK_StatusPageGroupingEquipment_Equipment]
ON [StatusPageGroupingEquipment]
    ([Equipment_Id]);
GO

-- Creating foreign key on [StatusPageId] in table 'StatusPageGroupings'
ALTER TABLE [StatusPageGroupings]
ADD CONSTRAINT [FK_StatusPageStatusPageGrouping]
    FOREIGN KEY ([StatusPageId])
    REFERENCES [StatusPages]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StatusPageStatusPageGrouping'
CREATE INDEX [IX_FK_StatusPageStatusPageGrouping]
ON [StatusPageGroupings]
    ([StatusPageId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------