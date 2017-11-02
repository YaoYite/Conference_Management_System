CREATE TABLE [dbo].[keyword] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [key_name] nVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_keyword] PRIMARY KEY CLUSTERED ([Id] ASC)
);

