CREATE TABLE [dbo].[assign]
(
	[paper_id] int NOT NULL,
    [reviewer_id] nvarchar(128) NOT NULL,
    CONSTRAINT [PK_assign] PRIMARY KEY CLUSTERED ([paper_id] ASC, [reviewer_id] ASC),
    CONSTRAINT [FK_assign_paper] FOREIGN KEY ([paper_id]) REFERENCES [dbo].[paper] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_assign_user] FOREIGN KEY ([reviewer_id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
)
