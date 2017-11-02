CREATE TABLE [dbo].[paperkey]
(
	[paper_id] int NOT NULL,
    [key_id] int NOT NULL,
    CONSTRAINT [PK_paperkey] PRIMARY KEY CLUSTERED ([paper_id] ASC, [key_id] ASC),
    CONSTRAINT [FK_paperkey_paper] FOREIGN KEY ([paper_id]) REFERENCES [dbo].[paper] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_paperkey_key] FOREIGN KEY ([key_id]) REFERENCES [dbo].[keyword] ([Id]) ON DELETE CASCADE
)
