CREATE TABLE [dbo].[PasswordGenerator] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Password] VARCHAR (20) UNIQUE NOT NULL,
    CONSTRAINT [PK_PasswordGenerator] PRIMARY KEY CLUSTERED ([Id] ASC)
);