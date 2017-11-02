CREATE TABLE [dbo].[Conf] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [name]         nVARCHAR (50)   NOT NULL,
    [topic]        nVARCHAR (256)  NOT NULL,
    [chair_id]     NVARCHAR (128) NOT NULL,
    [sub_deadline] DATETIME       NULL,
    [rev_deadline] DATETIME       NULL,
    CONSTRAINT [PK_Conf] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Conf_User] FOREIGN KEY ([chair_id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

