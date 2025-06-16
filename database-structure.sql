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
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'IsActive', N'Name', N'PasswordHash', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([Id], [CreatedAt], [Email], [IsActive], [Name], [PasswordHash], [Username])
VALUES (1, '2025-06-16T14:43:07.8508226Z', N'user1@topmed.com', CAST(1 AS bit), N'Maria', N'$2a$11$P3xtyxCFM7.9oI9gz.T74OH6.hw1vnoq8PlmbkHrQBpWs66TfuGIa', N'user_1'),
(2, '2025-06-16T14:43:08.1123267Z', N'user2@topmed.com', CAST(1 AS bit), N'João', N'$2a$11$FRztAeiIBSSXl8YZ/VpsBOozyeSRviWFG11F5E6FbbATU7ILey7ym', N'user_2');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'Email', N'IsActive', N'Name', N'PasswordHash', N'Username') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250616144310_InitialMigration', N'9.0.6');

COMMIT;
GO

