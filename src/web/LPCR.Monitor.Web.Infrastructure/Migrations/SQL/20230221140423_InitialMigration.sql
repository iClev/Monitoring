IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [JobScheduleTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_JobScheduleTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [JobStatusTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_JobStatusTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [LogTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] VARCHAR(20) NOT NULL,
    CONSTRAINT [PK_LogTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Jobs] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [Name] VARCHAR(50) NOT NULL,
    [Description] VARCHAR(255) NULL,
    [ProcessorName] VARCHAR(255) NOT NULL,
    [IsActive] BIT NOT NULL,
    [ScheduleTypeId] int NOT NULL DEFAULT 1,
    [Schedule] VARCHAR(20) NULL,
    CONSTRAINT [PK_Jobs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Jobs_JobScheduleTypes_ScheduleTypeId] FOREIGN KEY ([ScheduleTypeId]) REFERENCES [JobScheduleTypes] ([Id])
);
GO

CREATE TABLE [SystemLogs] (
    [Id] int NOT NULL IDENTITY,
    [LogTypeId] int NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    [Message] nvarchar(max) NOT NULL,
    [Exception] nvarchar(max) NULL,
    CONSTRAINT [PK_SystemLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SystemLogs_LogTypes_LogTypeId] FOREIGN KEY ([LogTypeId]) REFERENCES [LogTypes] ([Id])
);
GO

CREATE TABLE [JobRuns] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [JobId] uniqueidentifier NOT NULL,
    [StatusId] int NOT NULL,
    [Created] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    [Started] DATETIME NULL,
    [Completed] DATETIME NULL,
    [Paylaod] nvarchar(max) NULL,
    [CanRetry] BIT NOT NULL,
    CONSTRAINT [PK_JobRuns] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JobRuns_JobStatusTypes_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [JobStatusTypes] ([Id]),
    CONSTRAINT [FK_JobRuns_Jobs_JobId] FOREIGN KEY ([JobId]) REFERENCES [Jobs] ([Id])
);
GO

CREATE TABLE [JobRunLogs] (
    [Id] int NOT NULL IDENTITY,
    [JobRunId] uniqueidentifier NOT NULL,
    [LogTypeId] int NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    [Message] nvarchar(max) NOT NULL,
    [Exception] nvarchar(max) NULL,
    CONSTRAINT [PK_JobRunLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JobRunLogs_JobRuns_JobRunId] FOREIGN KEY ([JobRunId]) REFERENCES [JobRuns] ([Id]),
    CONSTRAINT [FK_JobRunLogs_LogTypes_LogTypeId] FOREIGN KEY ([LogTypeId]) REFERENCES [LogTypes] ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[JobScheduleTypes]'))
    SET IDENTITY_INSERT [JobScheduleTypes] ON;
INSERT INTO [JobScheduleTypes] ([Id], [Name])
VALUES (1, 'Scheduled'),
(2, 'OnDemand');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[JobScheduleTypes]'))
    SET IDENTITY_INSERT [JobScheduleTypes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[JobStatusTypes]'))
    SET IDENTITY_INSERT [JobStatusTypes] ON;
INSERT INTO [JobStatusTypes] ([Id], [Name])
VALUES (1, 'Unknown'),
(2, 'Pending'),
(3, 'Queued'),
(4, 'Running'),
(5, 'Completed'),
(6, 'Errored'),
(7, 'Canceled');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[JobStatusTypes]'))
    SET IDENTITY_INSERT [JobStatusTypes] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[LogTypes]'))
    SET IDENTITY_INSERT [LogTypes] ON;
INSERT INTO [LogTypes] ([Id], [Name])
VALUES (1, 'Information'),
(2, 'Warning'),
(3, 'Error'),
(4, 'Critical');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[LogTypes]'))
    SET IDENTITY_INSERT [LogTypes] OFF;
GO

CREATE INDEX [IX_JobRunLogs_JobRunId] ON [JobRunLogs] ([JobRunId]);
GO

CREATE INDEX [IX_JobRunLogs_LogTypeId] ON [JobRunLogs] ([LogTypeId]);
GO

CREATE INDEX [IX_JobRuns_JobId] ON [JobRuns] ([JobId]);
GO

CREATE INDEX [IX_JobRuns_StatusId] ON [JobRuns] ([StatusId]);
GO

CREATE INDEX [IX_Jobs_ScheduleTypeId] ON [Jobs] ([ScheduleTypeId]);
GO

CREATE INDEX [IX_SystemLogs_LogTypeId] ON [SystemLogs] ([LogTypeId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230221140423_InitialMigration', N'7.0.3');
GO

COMMIT;
GO

