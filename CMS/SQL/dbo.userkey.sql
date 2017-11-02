CREATE TABLE [dbo].[userkey] (
    [user_id] NVARCHAR (128) NOT NULL,
    [key_id] int NOT NULL,
    CONSTRAINT [PK_userkey] PRIMARY KEY CLUSTERED ([user_id] ASC, [key_id] ASC),
    CONSTRAINT [FK_userkey_keyword] FOREIGN KEY ([key_id]) REFERENCES [dbo].[keyword] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_userkey_user] FOREIGN KEY ([user_id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
