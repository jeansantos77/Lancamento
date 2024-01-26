CREATE DATABASE dbLancamentos
GO

USE dbLancamentos
GO


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

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(250) NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Lancamentos] (
    [Id] int NOT NULL IDENTITY,
    [Data] datetime2 NOT NULL,
    [Descricao] nvarchar(250) NOT NULL,
    [Tipo] nvarchar(1) NOT NULL,
    [Valor] decimal(18,2) NOT NULL,
    [UsuarioId] int NOT NULL,
    CONSTRAINT [PK_Lancamentos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Lancamentos_Usuarios_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [Usuarios] ([Id])
);
GO

CREATE INDEX [IX_Lancamentos_UsuarioId] ON [Lancamentos] ([UsuarioId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240125224931_InitialCreate', N'5.0.17');
GO

COMMIT;
GO