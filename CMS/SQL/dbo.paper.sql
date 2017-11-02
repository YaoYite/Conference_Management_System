CREATE TABLE [dbo].[paper]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[title] nvarchar(50) not null,
	[body] nvarchar(256) not null,
	[assigned] bit not null,
	[accepted] bit not null,
	[user_id] nvarchar(128) not null,
	[conf_id] int not null,
	constraint [paper_user] foreign key ([user_id]) references [dbo].[AspNetUsers] ([Id]) ,
	constraint [paper_conf] foreign key ([conf_id]) references [dbo].[Conf] ([Id]) 
)
