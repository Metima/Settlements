CREATE TABLE [dbo].[settlements]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [name] NCHAR(100) NOT NULL, CONSTRAINT [CK_name] UNIQUE (name)
)
