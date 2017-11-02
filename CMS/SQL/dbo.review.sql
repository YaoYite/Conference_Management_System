CREATE TABLE [dbo].[review]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[rating] int not null,
	[comment] nvarchar(256) not null,
	[user_id] nvarchar(128) not null,
	[paper_id] int not null,
	constraint [review_user] foreign key ([user_id]) references [dbo].[AspNetUsers] ([Id]) ,
	constraint [review_paper] foreign key ([paper_id]) references [dbo].[paper] ([Id]) 
)
